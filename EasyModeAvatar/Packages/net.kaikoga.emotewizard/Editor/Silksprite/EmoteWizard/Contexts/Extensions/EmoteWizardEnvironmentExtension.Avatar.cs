using System;
using System.Linq;
using nadena.dev.ndmf;
using nadena.dev.ndmf.util;
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
                var layers = _environment.AvatarDescriptor.baseAnimationLayers
                    .Concat(_environment.AvatarDescriptor.specialAnimationLayers)
                    .Select(layer => layer.animatorController)
                    .Where(layer => layer != null)
                    .Distinct()
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