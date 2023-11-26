using System;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static partial class EmoteWizardEnvironmentExtension
    {
        public static void BuildAvatar(this EmoteWizardEnvironment environment, bool manualBuild)
        {
            var avatarDescriptor = environment.AvatarDescriptor;
            var avatarAnimator = avatarDescriptor.EnsureComponent<Animator>();
            avatarAnimator.runtimeAnimatorController = null;

            using (new ManualBundleGeneratedAssetsScope(environment, manualBuild))
            {
                var parameters = environment.GetContext<ParametersContext>().Snapshot(); 

                RuntimeAnimatorController SelectGestureController()
                {
                    switch (environment.OverrideGesture)
                    {
                        case OverrideGeneratedControllerType2.Generate:
                            return environment.GetContext<GestureLayerContext>().BuildOutputAsset(parameters);
                        case OverrideGeneratedControllerType2.Override:
                            return environment.OverrideGestureController;
                        case OverrideGeneratedControllerType2.Default1:
                            return VrcSdkAssetLocator.HandsLayerController1();
                        case OverrideGeneratedControllerType2.Default2:
                            return VrcSdkAssetLocator.HandsLayerController2();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                RuntimeAnimatorController SelectActionController()
                {
                    switch (environment.OverrideAction)
                    {
                        case OverrideGeneratedControllerType1.Generate:
                            return environment.GetContext<ActionLayerContext>().BuildOutputAsset(parameters);
                        case OverrideGeneratedControllerType1.Override:
                            return environment.OverrideActionController;
                        case OverrideGeneratedControllerType1.Default:
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
                        case OverrideControllerType2.Override:
                            return environment.OverrideSittingController;
                        case OverrideControllerType2.Default1:
                            return VrcSdkAssetLocator.SittingLayerController1();
                        case OverrideControllerType2.Default2:
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

        class ManualBundleGeneratedAssetsScope : IDisposable
        {
            readonly GameObject _gameObject;
            readonly BuildContext _buildContext;

            public ManualBundleGeneratedAssetsScope(EmoteWizardEnvironment environment, bool manualBuild)
            {
                if (manualBuild && !environment.PersistGeneratedAssets)
                {
                    _gameObject = new GameObject("Dummy");
                    _buildContext = new BuildContext(_gameObject, environment.GeneratedAssetPath("Ephemeral"));
                }
            }

            void IDisposable.Dispose()
            {
                _buildContext?.Serialize();
                Object.DestroyImmediate(_gameObject);
            }
        }
    }
}