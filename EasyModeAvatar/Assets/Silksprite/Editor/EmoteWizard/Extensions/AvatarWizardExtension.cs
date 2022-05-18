using System;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AvatarWizardExtension
    {
        public static Animator ProvideProxyAnimator(this AvatarWizard avatarWizard)
        {
            var animator = avatarWizard.proxyAnimator ? avatarWizard.proxyAnimator : avatarWizard.avatarDescriptor.EnsureComponent<Animator>();
            avatarWizard.proxyAnimator = animator;
            return animator;
        }

        public static void BuildAvatar(this AvatarWizard avatarWizard)
        {
            var avatarDescriptor = avatarWizard.avatarDescriptor;
            var avatarAnimator = avatarDescriptor.EnsureComponent<Animator>();
            avatarAnimator.runtimeAnimatorController = null;
            
            var emoteWizardRoot = avatarWizard.EmoteWizardRoot;
            var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();
            parametersWizard.TryRefreshParameters();

            RuntimeAnimatorController SelectGestureController()
            {
                switch (avatarWizard.overrideGesture)
                {
                    case AvatarWizard.OverrideGeneratedControllerType2.Generate:
                        return emoteWizardRoot.GetWizard<GestureWizard>()?.BuildOutputAsset(parametersWizard);
                    case AvatarWizard.OverrideGeneratedControllerType2.Override:
                        return avatarWizard.overrideGestureController;
                    case AvatarWizard.OverrideGeneratedControllerType2.Default1:
                        return VrcSdkAssetLocator.HandsLayerController1();
                    case AvatarWizard.OverrideGeneratedControllerType2.Default2:
                        return VrcSdkAssetLocator.HandsLayerController2();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectActionController()
            {
                switch (avatarWizard.overrideAction)
                {
                    case AvatarWizard.OverrideGeneratedControllerType1.Generate:
                        return emoteWizardRoot.GetWizard<ActionWizard>()?.BuildOutputAsset();
                    case AvatarWizard.OverrideGeneratedControllerType1.Override:
                        return avatarWizard.overrideActionController;
                    case AvatarWizard.OverrideGeneratedControllerType1.Default:
                        return VrcSdkAssetLocator.ActionLayerController();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectFxController()
            {
                return emoteWizardRoot.GetWizard<FxWizard>()?.BuildOutputAsset(parametersWizard);
            }

            RuntimeAnimatorController SelectSittingController()
            {
                switch (avatarWizard.overrideSitting)
                {
                    case AvatarWizard.OverrideControllerType2.Override:
                        return avatarWizard.overrideSittingController;
                    case AvatarWizard.OverrideControllerType2.Default1:
                        return VrcSdkAssetLocator.SittingLayerController1();
                    case AvatarWizard.OverrideControllerType2.Default2:
                        return VrcSdkAssetLocator.SittingLayerController2();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var gestureController = SelectGestureController();
            var actionController = SelectActionController();
            var fxController = SelectFxController();
            var sittingController = SelectSittingController();

            avatarDescriptor.customizeAnimationLayers = true;
            avatarDescriptor.baseAnimationLayers = new[]
            {
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = null,
                    isDefault = true,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.Base
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = null,
                    isDefault = true,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.Additive
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = gestureController,
                    isDefault = gestureController == null,
                    isEnabled = false,
                    mask = VrcSdkAssetLocator.HandsOnly(),
                    type = VRCAvatarDescriptor.AnimLayerType.Gesture
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = actionController,
                    isDefault = actionController == null,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.Action
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = fxController,
                    isDefault = fxController == null,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.FX
                }
            };
            
            avatarDescriptor.specialAnimationLayers = new[]
            {
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = sittingController,
                    isDefault = false,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.Sitting
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = null,
                    isDefault = true,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.TPose
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = null,
                    isDefault = false,
                    isEnabled = false,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.IKPose
                }
            };
            avatarDescriptor.customExpressions = true;
            avatarDescriptor.expressionsMenu = emoteWizardRoot.GetWizard<ExpressionWizard>()?.BuildOutputAsset();
            avatarDescriptor.expressionParameters = emoteWizardRoot.GetWizard<ParametersWizard>()?.BuildOutputAsset();
        }
    }
}