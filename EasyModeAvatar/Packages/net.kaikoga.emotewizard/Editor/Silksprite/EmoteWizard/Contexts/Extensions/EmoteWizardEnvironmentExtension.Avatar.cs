using System;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static partial class EmoteWizardEnvironmentExtension
    {
        public static void BuildAvatar(this EmoteWizardEnvironment environment)
        {
            var avatarDescriptor = environment.AvatarDescriptor;
            var avatarAnimator = avatarDescriptor.EnsureComponent<Animator>();
            avatarAnimator.runtimeAnimatorController = null;
            
            var parameters = environment.GetContext<ParametersContext>().Snapshot(); 

            RuntimeAnimatorController SelectGestureController()
            {
                switch (environment.OverrideGesture)
                {
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType2.Generate:
                        return environment.GetContext<GestureLayerContext>().BuildOutputAsset(parameters);
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType2.Override:
                        return environment.OverrideGestureController;
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
                switch (environment.OverrideAction)
                {
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType1.Generate:
                        return environment.GetContext<ActionLayerContext>().BuildOutputAsset(parameters);
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType1.Override:
                        return environment.OverrideActionController;
                    case EmoteWizardEnvironment.OverrideGeneratedControllerType1.Default:
                        return VrcSdkAssetLocator.ActionLayerController();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectFxController()
            {
                return environment.GetContext<FxLayerContext>().BuildOutputAsset(parameters);
            }

            RuntimeAnimatorController SelectSittingController()
            {
                switch (environment.OverrideSitting)
                {
                    case EmoteWizardEnvironment.OverrideControllerType2.Override:
                        return environment.OverrideSittingController;
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
            avatarDescriptor.expressionsMenu = environment.GetContext<ExpressionContext>()?.BuildOutputAsset();
            avatarDescriptor.expressionParameters = environment.GetContext<ParametersContext>()?.BuildOutputAsset();
        }
    }
}