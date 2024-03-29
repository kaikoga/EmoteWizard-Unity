using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Scopes;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class EmoteWizardEnvironmentExtension
    {
        public static void CleanupVrcAvatar(this EmoteWizardEnvironment environment)
        {
            var avatarDescriptor = environment.AvatarRoot.GetComponent<VRCAvatarDescriptor>();
            if (!avatarDescriptor) return;

            if (avatarDescriptor.TryGetComponent<Animator>(out var avatarAnimator))
            {
                avatarAnimator.runtimeAnimatorController = null;
            }
            
            CustomizeAnimationLayers(avatarDescriptor, null, null, null, null);

            avatarDescriptor.customExpressions = true;
            avatarDescriptor.expressionsMenu = null;
            avatarDescriptor.expressionParameters = null;

        }

        public static void BuildVrcAvatar(this EmoteWizardEnvironment environment, IUndoable undoable, bool manualBuild)
        {
            var avatarDescriptor = environment.AvatarRoot.GetComponent<VRCAvatarDescriptor>();
            if (!avatarDescriptor) return;

            var avatarAnimator = undoable.EnsureComponent<Animator>(avatarDescriptor);
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

                if (manualBuild)
                {
                    environment.GetContext<EditorLayerContext>().BuildOutputAsset(parameters);
                }
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
    }

    class ManualBundleGeneratedAssetsScope : ManualBundleGeneratedAssetsScopeBase
    {
        public ManualBundleGeneratedAssetsScope(EmoteWizardEnvironment environment, bool manualBuild) : base(environment, manualBuild)
        {
        }

        protected override IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment)
        {
            // manually try to persist volatile layers because layers are what Emote Wizard generates
            var avatarDescriptor = environment.AvatarRoot.GetComponent<VRCAvatarDescriptor>();
            var layers = avatarDescriptor.AllAnimationLayers()
                .Where(layer => !EditorUtility.IsPersistent(layer));
            return layers;
        }
    }
}