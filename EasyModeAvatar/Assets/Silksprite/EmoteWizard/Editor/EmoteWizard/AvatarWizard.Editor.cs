using System;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
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
            
            EditorGUILayout.PropertyField(serializedObj.FindProperty("overrideGesture"));
            if (avatarWizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Override)
            {
                EditorGUILayout.PropertyField(serializedObj.FindProperty("overrideGestureController"));
            }
            EditorGUILayout.PropertyField(serializedObj.FindProperty("overrideSitting"));
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

                var emoteWizardRoot = avatarWizard.EmoteWizardRoot;

                var avatarDescriptor = avatarWizard.avatarDescriptor;
                EditorGUILayout.PropertyField(serializedObj.FindProperty("avatarDescriptor"));
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
                EditorGUILayout.PropertyField(serializedObj.FindProperty("proxyAnimator"));
                if (avatarDescriptor)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Edit Gesture"))
                        {
                            EditAnimator(gestureController);
                        }
                        if (GUILayout.Button("Edit FX"))
                        {
                            EditAnimator(fxController);
                        }
                        if (GUILayout.Button("Remove Controller"))
                        {
                            EditAnimator(null);
                        }
                    }
                }
            });

            serializedObj.ApplyModifiedProperties();
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
                    isDefault = false,
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
                    animatorController = emoteWizardRoot.GetComponent<FxWizard>()?.outputAsset,
                    isDefault = false,
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