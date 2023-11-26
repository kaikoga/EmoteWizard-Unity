using System;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class AvatarContextExtension
    {
        public static void BuildAvatar(this AvatarContext context)
        {
            var env = context.Environment;
            
            var avatarDescriptor = env.AvatarDescriptor;
            var avatarAnimator = avatarDescriptor.EnsureComponent<Animator>();
            avatarAnimator.runtimeAnimatorController = null;
            
            var parameters = env.GetContext<ParametersContext>().Snapshot(); 

            RuntimeAnimatorController SelectGestureController()
            {
                switch (env.OverrideGesture)
                {
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType2.Generate:
                        return env.GetContext<GestureLayerContext>().BuildOutputAsset(parameters);
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType2.Override:
                        return env.OverrideGestureController;
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType2.Default1:
                        return VrcSdkAssetLocator.HandsLayerController1();
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType2.Default2:
                        return VrcSdkAssetLocator.HandsLayerController2();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectActionController()
            {
                switch (env.OverrideAction)
                {
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType1.Generate:
                        return env.GetContext<ActionLayerContext>().BuildOutputAsset(parameters);
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType1.Override:
                        return env.OverrideActionController;
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType1.Default:
                        return VrcSdkAssetLocator.ActionLayerController();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectFxController()
            {
                return env.GetContext<FxLayerContext>().BuildOutputAsset(parameters);
            }

            RuntimeAnimatorController SelectSittingController()
            {
                switch (env.OverrideSitting)
                {
                    case EmoteWizardEnvironment.OverrideControllerType2.Override:
                        return env.OverrideSittingController;
                    case EmoteWizardEnvironment.OverrideControllerType2.Default1:
                        return VrcSdkAssetLocator.SittingLayerController1();
                    case EmoteWizardEnvironment.OverrideControllerType2.Default2:
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
            avatarDescriptor.expressionsMenu = env.GetContext<ExpressionContext>()?.BuildOutputAsset();
            avatarDescriptor.expressionParameters = env.GetContext<ParametersContext>()?.BuildOutputAsset();
        }
    }
}