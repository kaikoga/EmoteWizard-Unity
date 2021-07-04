using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects.DrawerContexts;
using EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class AnimationWizardBaseExtension
    {
        public static void GenerateParameterEmoteClipsFromTargets(this AnimationWizardBase animationWizardBase, ParameterEmoteDrawerContext context, string emoteName)
        {
            var proxyAnimator = context.EmoteWizardRoot.GetComponent<AvatarWizard>()?.ProvideProxyAnimator();
            if (proxyAnimator == null)
            {
                Debug.LogError("Requires AvatarWizard.proxyAnimator to find relative path of targets.");
                return;
            }
            var animatorRoot = proxyAnimator.transform;
            
            var parameterEmote = animationWizardBase.parameters.First(parameter => parameter.name == emoteName);
            var targets = parameterEmote.states.SelectMany(state => state.targets).Distinct().ToList();
            foreach (var state in parameterEmote.states)
            {
                var relativePath = GeneratedAssetLocator.ParameterEmoteStateClipPath(context.Layer, emoteName, state.value);
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
            EditorUtility.SetDirty(animationWizardBase);
        }
    }
}