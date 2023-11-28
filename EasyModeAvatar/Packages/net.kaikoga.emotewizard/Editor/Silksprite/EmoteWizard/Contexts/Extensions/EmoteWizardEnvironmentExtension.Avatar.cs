using System;
using System.Linq;
using nadena.dev.ndmf;
using nadena.dev.ndmf.util;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static partial class EmoteWizardEnvironmentExtension
    {
        public static void CleanupAvatar(this EmoteWizardEnvironment environment)
        {
            var avatarDescriptor = environment.AvatarDescriptor;
            if (avatarDescriptor.TryGetComponent<Animator>(out var avatarAnimator))
            {
                avatarAnimator.runtimeAnimatorController = null;
            }
            
            CustomizeAnimationLayers(avatarDescriptor, null, null, null, null);

            avatarDescriptor.customExpressions = true;
            avatarDescriptor.expressionsMenu = null;
            avatarDescriptor.expressionParameters = null;

        }

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

                CustomizeAnimationLayers(avatarDescriptor, gestureController, actionController, fxController, sittingController);

                avatarDescriptor.customExpressions = true;
                avatarDescriptor.expressionsMenu = environment.GetContext<ExpressionContext>()?.BuildOutputAsset();
                avatarDescriptor.expressionParameters = environment.GetContext<ParametersContext>()?.BuildOutputAsset();
            }
        }

        static void CustomizeAnimationLayers(VRCAvatarDescriptor avatarDescriptor, RuntimeAnimatorController gestureController, RuntimeAnimatorController actionController, RuntimeAnimatorController fxController, RuntimeAnimatorController sittingController)
        {
            var baseLayer = avatarDescriptor.FindCustomAnimLayer(VRCAvatarDescriptor.AnimLayerType.Base);
            var additiveLayer = avatarDescriptor.FindCustomAnimLayer(VRCAvatarDescriptor.AnimLayerType.Additive);
            var tPoseLayer = avatarDescriptor.FindCustomAnimLayer(VRCAvatarDescriptor.AnimLayerType.TPose);
            var ikPoseLayer = avatarDescriptor.FindCustomAnimLayer(VRCAvatarDescriptor.AnimLayerType.IKPose);

            avatarDescriptor.customizeAnimationLayers = true;
            avatarDescriptor.baseAnimationLayers = new[]
            {
                baseLayer,
                additiveLayer,
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = gestureController,
                    isDefault = !gestureController,
                    isEnabled = !gestureController,
                    mask = VrcSdkAssetLocator.HandsOnly(),
                    type = VRCAvatarDescriptor.AnimLayerType.Gesture
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = actionController,
                    isDefault = !actionController,
                    isEnabled = !actionController,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.Action
                },
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = fxController,
                    isDefault = !fxController,
                    isEnabled = !fxController,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.FX
                }
            };

            avatarDescriptor.specialAnimationLayers = new[]
            {
                new VRCAvatarDescriptor.CustomAnimLayer
                {
                    animatorController = sittingController,
                    isDefault = !sittingController,
                    isEnabled = !sittingController,
                    mask = null,
                    type = VRCAvatarDescriptor.AnimLayerType.Sitting
                },
                tPoseLayer,
                ikPoseLayer
            };
        }

        class ManualBundleGeneratedAssetsScope : IDisposable
        {
            readonly EmoteWizardEnvironment _environment;
            readonly GameObject _gameObject;
            readonly BuildContext _buildContext;

            public ManualBundleGeneratedAssetsScope(EmoteWizardEnvironment environment, bool manualBuild)
            {
                _environment = environment;
                if (manualBuild && !environment.PersistGeneratedAssets)
                {
                    _gameObject = new GameObject("Temporary");
                    _buildContext = new BuildContext(_gameObject, environment.GeneratedAssetPath("Temporary"));
                }
            }

            void IDisposable.Dispose()
            {
                if (_buildContext == null) return;

                // manually try to persist volatile layers because layers are what Emote Wizard generates
                var layers = _environment.AvatarDescriptor.AllAnimationLayers()
                    .Where(layer => !EditorUtility.IsPersistent(layer));
                // we are sure these are not prefabs
                foreach (var layer in layers)
                {
                    foreach (var asset in layer.ReferencedAssets(traverseSaved: false, includeScene: true))
                    {
                        AssetDatabase.AddObjectToAsset(asset, _buildContext.AssetContainer);
                        asset.hideFlags = HideFlags.None; // match Modular Avatar behavior 
                    }
                }

                AssetDatabase.SaveAssets();
                Object.DestroyImmediate(_gameObject);
            }
        }
    }
}