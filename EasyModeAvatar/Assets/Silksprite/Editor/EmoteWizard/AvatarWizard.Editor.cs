using System;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(AvatarWizard))]
    public class AvatarWizardEditor : Editor 
    {
        AvatarWizard avatarWizard;

        void OnEnable()
        {
            avatarWizard = target as AvatarWizard;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = avatarWizard.EmoteWizardRoot;

            var overrideGesture = serializedObj.FindProperty("overrideGesture");
            const string overrideGestureTooltip = "Gestureレイヤーで使用するAnimatorControllerを選択します。\nGenerate: EmoteWizardが生成するものを使用\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）";
            EditorGUILayout.PropertyField(overrideGesture, new GUIContent(overrideGesture.displayName, overrideGestureTooltip));
            if (avatarWizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Override)
            {
                EditorGUILayout.PropertyField(serializedObj.FindProperty("overrideGestureController"));
            }
            var overrideSitting = serializedObj.FindProperty("overrideSitting");
            const string overrideSittingTooltip = "Sittingレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）";
            EditorGUILayout.PropertyField(overrideSitting, new GUIContent(overrideSitting.displayName, overrideSittingTooltip));
            if (avatarWizard.overrideSitting == AvatarWizard.OverrideControllerType2.Override)
            {
                EditorGUILayout.PropertyField(serializedObj.FindProperty("overrideSittingController"));
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

                var avatarDescriptor = avatarWizard.avatarDescriptor;
                var avatarDescriptorProperty = serializedObj.FindProperty("avatarDescriptor");
                const string avatarDescriptorTooltip = "ここで指定したアバターの設定が上書きされます。";
                EditorGUILayout.PropertyField(avatarDescriptorProperty, new GUIContent(avatarDescriptorProperty.displayName, avatarDescriptorTooltip));
                if (avatarDescriptor == null)
                {
                    EditorGUILayout.HelpBox("VRCAvatarDescriptor is missing. Some functions might not work.", MessageType.Error);
                }
                var gestureController = emoteWizardRoot.GetComponent<GestureWizard>()?.outputAsset as AnimatorController;
                var fxController = emoteWizardRoot.GetComponent<FxWizard>()?.outputAsset as AnimatorController;

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

                var proxyAnimator = serializedObj.FindProperty("proxyAnimator");
                const string proxyAnimatorTooltip = "アバターのアニメーションを編集する際に使用するAnimatorを別途選択できます。";
                EditorGUILayout.PropertyField(proxyAnimator, new GUIContent(proxyAnimator.displayName, proxyAnimatorTooltip));
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

            serializedObj.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "VRCAvatarDescriptorの更新を行います。\nAnimatorコンポーネントが存在するなら、それを使ってアバターのアニメーションの編集を開始することができます。");
        }

        void UpdateAvatar(VRCAvatarDescriptor avatarDescriptor)
        {
            var emoteWizardRoot = avatarWizard.EmoteWizardRoot;

            RuntimeAnimatorController SelectGestureController()
            {
                switch (avatarWizard.overrideGesture)
                {
                    case AvatarWizard.OverrideGeneratedControllerType2.Generate:
                        return emoteWizardRoot.GetComponent<GestureWizard>()?.outputAsset;
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

            RuntimeAnimatorController SelectFxController()
            {
                return emoteWizardRoot.GetComponent<FxWizard>()?.outputAsset;
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
                    animatorController = null,
                    isDefault = true,
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
            avatarDescriptor.expressionsMenu = emoteWizardRoot.GetComponent<ExpressionWizard>()?.outputAsset;
            avatarDescriptor.expressionParameters = emoteWizardRoot.GetComponent<ParametersWizard>()?.outputAsset;
        }
    }
}