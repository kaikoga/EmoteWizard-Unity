using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParameterEmoteExtension
    {
        public static IEnumerable<AnimationClip> AllClips(this ParameterEmote parameterEmote)
        {
            return parameterEmote.states.Select(state => state.clip);
        }

        public static void GenerateParameterEmoteClipsFromTargets(this ParameterEmote parameterEmote, Component component, ParameterEmoteDrawerContext context)
        {
            var proxyAnimator = context.EmoteWizardRoot.GetWizard<AvatarWizard>()?.ProvideProxyAnimator();
            if (proxyAnimator == null)
            {
                Debug.LogError("Requires AvatarWizard.proxyAnimator to find relative path of targets.");
                return;
            }
            var animatorRoot = proxyAnimator.transform;
            
            var targets = parameterEmote.states.SelectMany(state => state.targets.Where(t => t != null)).Distinct().ToList();
            foreach (var state in parameterEmote.states)
            {
                var relativePath = GeneratedAssetLocator.ParameterEmoteStateClipPath(context.Layer, parameterEmote.name, state.value);
                var clip = context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                clip.ClearCurves();

                foreach (var target in targets)
                {
                    var path = target.transform.GetRelativePathFrom(animatorRoot);
                    var value = state.targets.Contains(target) ? 1f : 0f;
                    clip.SetCurve(path, typeof(GameObject), "m_IsActive", AnimationCurve.Constant(0f, 1 / 60f, value));   
                }

                state.clip = clip;
                EditorUtility.SetDirty(clip);
            }
            EditorUtility.SetDirty(component);
        }
    }
}