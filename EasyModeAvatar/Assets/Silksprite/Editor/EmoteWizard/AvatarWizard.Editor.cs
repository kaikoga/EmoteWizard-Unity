using System;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(AvatarWizard))]
    public class AvatarWizardEditor : Editor 
    {
        AvatarWizard avatarWizard;

        void OnEnable()
        {
            avatarWizard = (AvatarWizard) target;
        }

        public override void OnInspectorGUI()
        {
            RuntimeAnimatorController GenerateOverrideController(RuntimeAnimatorController source, string layer)
            {
                var path = AssetDatabase.GetAssetPath(source);
                var newPath = avatarWizard.EmoteWizardRoot.GeneratedAssetPath(GeneratedAssetLocator.GeneratedOverrideControllerPath(layer));
                EnsureDirectory(newPath);
                AssetDatabase.CopyAsset(path, newPath);
                return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(newPath);
            }

            using (new ObjectChangeScope(avatarWizard))
            {
                var emoteWizardRoot = avatarWizard.EmoteWizardRoot;

                var overrideGestureLabel = new GUIContent("Override Gesture", "Gestureレイヤーで使用するAnimatorControllerを選択します。\nGenerate: EmoteWizardが生成するものを使用\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
                TypedGUILayout.EnumPopup(overrideGestureLabel, ref avatarWizard.overrideGesture);
                if (avatarWizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Override)
                {
                    CustomTypedGUILayout.AssetFieldWithGenerate("Override Gesture Controller", ref avatarWizard.overrideGestureController, () => GenerateOverrideController(VrcSdkAssetLocator.HandsLayerController1(), "Gesture"));
                }
                var overrideActionLabel = new GUIContent("Override Action", "Actionレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault: デフォルトを使用");
                TypedGUILayout.EnumPopup(overrideActionLabel, ref avatarWizard.overrideAction);
                if (avatarWizard.overrideAction == AvatarWizard.OverrideControllerType1.Override)
                {
                    CustomTypedGUILayout.AssetFieldWithGenerate("Override Action Controller", ref avatarWizard.overrideActionController, () => GenerateOverrideController(VrcSdkAssetLocator.ActionLayerController(), "Action"));
                }
                var overrideSittingLabel = new GUIContent("Override Sitting", "Sittingレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
                TypedGUILayout.EnumPopup(overrideSittingLabel, ref avatarWizard.overrideSitting);
                if (avatarWizard.overrideSitting == AvatarWizard.OverrideControllerType2.Override)
                {
                    CustomTypedGUILayout.AssetFieldWithGenerate("Override Sitting Controller", ref avatarWizard.overrideSittingController, () => GenerateOverrideController(VrcSdkAssetLocator.SittingLayerController1(), "Sitting"));
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    void EditAnimator(AnimatorController animatorController)
                    {
                        var animator = avatarWizard.ProvideProxyAnimator();
                        animator.runtimeAnimatorController = animatorController;
                        if (!animatorController) return;
                        Selection.SetActiveObjectWithContext(animator.gameObject, animatorController);
                    }

                    var avatarDescriptorLabel = new GUIContent("Avatar Descriptor", "ここで指定したアバターの設定が上書きされます。");
                    TypedGUILayout.ReferenceField(avatarDescriptorLabel, ref avatarWizard.avatarDescriptor);

                    var avatarDescriptor = avatarWizard.avatarDescriptor;
                    if (avatarDescriptor == null)
                    {
                        EditorGUILayout.HelpBox("VRCAvatarDescriptor is missing. Some functions might not work.", MessageType.Error);
                    }
                    var gestureController = emoteWizardRoot.GetWizard<GestureWizard>()?.outputAsset as AnimatorController;
                    var fxController = emoteWizardRoot.GetWizard<FxWizard>()?.outputAsset as AnimatorController;

                    if (avatarDescriptor)
                    {
                        var avatarAnimator = avatarWizard.avatarDescriptor.EnsureComponent<Animator>();
                        if (GUILayout.Button("Update Avatar"))
                        {
                            avatarAnimator.runtimeAnimatorController = null;
                            UpdateAvatar(avatarDescriptor);
                        }

                        if (avatarAnimator.runtimeAnimatorController == null)
                        {
                            // do nothing
                        }
                        else if (avatarAnimator.runtimeAnimatorController == gestureController)
                        {
                            EditorGUILayout.HelpBox("Editing Gesture Controller on avatar.", MessageType.Warning);
                        }
                        else if (avatarAnimator.runtimeAnimatorController == fxController)
                        {
                            EditorGUILayout.HelpBox("Editing FX Controller on avatar.", MessageType.Warning);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("Animator Controller is present.", MessageType.Warning);
                        }
                    }

                    var proxyAnimatorLabel = new GUIContent("Proxy Animator", "アバターのアニメーションを編集する際に使用するAnimatorを別途選択できます。");
                    TypedGUILayout.ReferenceField(proxyAnimatorLabel, ref avatarWizard.proxyAnimator);

                    if (avatarDescriptor)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            using (new EditorGUI.DisabledScope(gestureController == null))
                            {
                                if (GUILayout.Button("Edit Gesture"))
                                {
                                    EditAnimator(gestureController);
                                }
                            }

                            using (new EditorGUI.DisabledScope(fxController == null))
                            {
                                if (GUILayout.Button("Edit FX"))
                                {
                                    EditAnimator(fxController);
                                }
                            }

                            if (GUILayout.Button("Remove Controller"))
                            {
                                EditAnimator(null);
                            }
                        }
                    }
                });
                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "VRCAvatarDescriptorの更新を行います。\nAnimatorコンポーネントが存在するなら、それを使ってアバターのアニメーションの編集を開始することができます。");
            }
        }

        void UpdateAvatar(VRCAvatarDescriptor avatarDescriptor)
        {
            var emoteWizardRoot = avatarWizard.EmoteWizardRoot;

            RuntimeAnimatorController SelectGestureController()
            {
                switch (avatarWizard.overrideGesture)
                {
                    case AvatarWizard.OverrideGeneratedControllerType2.Generate:
                        return emoteWizardRoot.GetWizard<GestureWizard>()?.outputAsset;
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
                    case AvatarWizard.OverrideControllerType1.Override:
                        return avatarWizard.overrideActionController;
                    case AvatarWizard.OverrideControllerType1.Default:
                        return VrcSdkAssetLocator.ActionLayerController();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RuntimeAnimatorController SelectFxController()
            {
                return emoteWizardRoot.GetWizard<FxWizard>()?.outputAsset;
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
            avatarDescriptor.expressionsMenu = emoteWizardRoot.GetWizard<ExpressionWizard>()?.outputAsset;
            avatarDescriptor.expressionParameters = emoteWizardRoot.GetWizard<ParametersWizard>()?.outputAsset;
        }
    }
}