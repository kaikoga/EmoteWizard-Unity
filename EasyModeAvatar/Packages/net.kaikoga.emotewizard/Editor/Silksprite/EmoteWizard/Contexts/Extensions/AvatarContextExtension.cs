using System;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class AvatarContextExtension
    {
        public static Animator ProvideProxyAnimator(this AvatarContext context)
        {
            var animator = context.ProxyAnimator ? context.ProxyAnimator : context.AvatarDescriptor.EnsureComponent<Animator>();
            context.ProxyAnimator = animator;
            return animator;
        }

        public static void BuildAvatar(this AvatarContext context)
        {
            var avatarDescriptor = context.AvatarDescriptor;
            var avatarAnimator = avatarDescriptor.EnsureComponent<Animator>();
            avatarAnimator.runtimeAnimatorController = null;
            
            var parameters = context.Environment.GetContext<ParametersContext>().Snapshot(); 

            RuntimeAnimatorController SelectGestureController()
            {
                switch (context.OverrideGesture)
                {
                    case AvatarWizard.OverrideGeneratedControllerType2.Generate:
                        return context.Environment.GetContext<GestureLayerContext>().BuildOutputAsset(parameters);
                    case AvatarWizard.OverrideGeneratedControllerType2.Override:
                        return context.OverrideGestureController;
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
                switch (context.OverrideAction)
                {
                    case AvatarWizard.OverrideGeneratedControllerType1.Generate:
                        return context.Environment.GetContext<ActionLayerContext>().BuildOutputAsset(parameters);
                    case AvatarWizard.OverrideGeneratedControllerType1.Override:
                        return context.OverrideActionController;
                    case AvatarWizard.OverrideGeneratedControllerType1.Default:
                        return VrcSdkAssetLocator.ActionLayerController();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectFxController()
            {
                return context.Environment.GetContext<FxLayerContext>().BuildOutputAsset(parameters);
            }

            RuntimeAnimatorController SelectSittingController()
            {
                switch (context.OverrideSitting)
                {
                    case AvatarWizard.OverrideControllerType2.Override:
                        return context.OverrideSittingController;
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
            avatarDescriptor.expressionsMenu = context.Environment.GetContext<ExpressionContext>()?.BuildOutputAsset();
            avatarDescriptor.expressionParameters = context.Environment.GetContext<ParametersContext>()?.BuildOutputAsset();
        }
    }
}