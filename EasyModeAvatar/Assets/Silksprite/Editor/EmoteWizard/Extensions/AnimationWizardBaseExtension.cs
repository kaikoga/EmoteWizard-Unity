using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimationWizardBaseExtension
    {
        public static void RepopulateDefaultEmotes(this AnimationWizardBase animationWizardBase)
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            animationWizardBase.legacyEmotes = newEmotes;
        }

        public static void RepopulateDefaultEmotes14(this AnimationWizardBase animationWizardBase)
        {
            var newEmotes = Enumerable.Empty<Emote>()
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther),
                        control = EmoteControl.Populate(handSign)
                    }))
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.NotEqual),
                        control = EmoteControl.Populate(handSign)
                    }))
                .ToList();
            animationWizardBase.legacyEmotes = newEmotes;
        }

        public static void RepopulateParameterEmotes(this AnimationWizardBase animationWizardBase, ParametersWizard parametersWizard)
        {
            parametersWizard.TryRefreshParameters();
            animationWizardBase.legacyParameterEmotes = new List<ParameterEmote>();
            animationWizardBase.RefreshParameters(parametersWizard);
        }


        public static void BuildResetClip(this AnimationWizardBase animationWizardBase, AnimationClip targetClip)
        {
            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(animationWizardBase.CollectBaseMixins().SelectMany(e => e.AllClips()))
                .Concat(animationWizardBase.CollectEmotes().SelectMany(e => e.AllClips()))
                .Concat(animationWizardBase.CollectParameterEmotes().SelectMany(p => p.AllClips()))
                .Concat(animationWizardBase.CollectMixins().SelectMany(p => p.AllClips()))
                .Where(c => c != null).ToList();
            var curveBindings = allClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            var objectReferenceCurveBindings = allClips.SelectMany(AnimationUtility.GetObjectReferenceCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            
            var vrcAvatarDescriptor = animationWizardBase.EmoteWizardRoot.GetWizard<AvatarWizard>()?.avatarDescriptor;
            var avatar = vrcAvatarDescriptor != null ? vrcAvatarDescriptor.gameObject : null;

            targetClip.ClearCurves();
            targetClip.frameRate = 60f;
            foreach (var curveBinding in curveBindings)
            {
                var value = 0f;
                if (avatar)
                {
                    AnimationUtility.GetFloatValue(avatar, curveBinding, out value);
                }
                targetClip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, AnimationCurve.Constant(0f, 1 / 60f, value));
            }
            foreach (var curveBinding in objectReferenceCurveBindings)
            {
                Object value = null;
                if (avatar)
                {
                    AnimationUtility.GetObjectReferenceValue(avatar, curveBinding, out value);
                }
                AnimationUtility.SetObjectReferenceCurve(targetClip, curveBinding, new []
                {
                    new ObjectReferenceKeyframe { time = 0, value = value },
                    new ObjectReferenceKeyframe { time = 1 / 60f, value = value }
                });
            }
        }

        public static void GenerateParameterEmoteClipsFromTargets(this AnimationWizardBase animationWizardBase, ParameterEmoteDrawerContext context, string emoteName)
        {
            var proxyAnimator = context.EmoteWizardRoot.GetWizard<AvatarWizard>()?.ProvideProxyAnimator();
            if (proxyAnimator == null)
            {
                Debug.LogError("Requires AvatarWizard.proxyAnimator to find relative path of targets.");
                return;
            }
            var animatorRoot = proxyAnimator.transform;
            
            var parameterEmote = animationWizardBase.legacyParameterEmotes.First(parameter => parameter.name == emoteName);
            var targets = parameterEmote.states.SelectMany(state => state.targets.Where(t => t != null)).Distinct().ToList();
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