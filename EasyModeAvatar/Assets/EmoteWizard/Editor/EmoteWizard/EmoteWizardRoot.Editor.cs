using EmoteWizard.Extensions;
using EmoteWizard.Tools;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using static EmoteWizard.Tools.EmoteWizardEditorTools;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : Editor
    {
        EmoteWizardRoot emoteWizardRoot;

        void OnEnable()
        {
            emoteWizardRoot = target as EmoteWizardRoot;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            using (new GUILayout.HorizontalScope())
            {
                emoteWizardRoot.generatedAssetRoot =
                    EditorGUILayout.TextField("Generated Assets Root", emoteWizardRoot.generatedAssetRoot);
                if (GUILayout.Button("Browse"))
                {
                    SelectFolder("Select Generated Assets Root", ref emoteWizardRoot.generatedAssetRoot);
                }
            }

            EditorGUILayout.PropertyField(serializedObj.FindProperty("generatedAssetPrefix"));
            PropertyFieldWithGenerate(serializedObj.FindProperty("emptyClip"), () => emoteWizardRoot.ProvideEmptyClip());

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.PropertyField(serializedObj.FindProperty("useReorderUI"));
            }

            if (!emoteWizardRoot.GetComponent<SetupWizard>())
            {
                if (GUILayout.Button("Setup"))
                {
                    emoteWizardRoot.EnsureComponent<SetupWizard>();
                }
            }
            
            OutputUIArea(() =>
            {
                void EditAnimator(AnimatorController animatorController)
                {
                    var animator = emoteWizardRoot.proxyAnimator ? emoteWizardRoot.proxyAnimator : emoteWizardRoot.avatarDescriptor.EnsureComponent<Animator>();
                    emoteWizardRoot.proxyAnimator = animator;
                    animator.runtimeAnimatorController = animatorController;
                    Selection.SetActiveObjectWithContext(animator.gameObject, animatorController);
                }

                var avatarDescriptor = emoteWizardRoot.avatarDescriptor;
                EditorGUILayout.PropertyField(serializedObj.FindProperty("avatarDescriptor"));
                var gestureController = emoteWizardRoot.GetComponent<GestureWizard>()?.outputAsset as AnimatorController;
                var fxController = emoteWizardRoot.GetComponent<FxWizard>()?.outputAsset as AnimatorController;

                if (avatarDescriptor)
                {
                    var avatarAnimator = emoteWizardRoot.avatarDescriptor.EnsureComponent<Animator>();
                    if (GUILayout.Button("Update Avatar"))
                    {
                        avatarAnimator.runtimeAnimatorController = null;
                        UpdateAvatar(avatarDescriptor);
                    }

                    if (avatarAnimator.runtimeAnimatorController == gestureController)
                    {
                        EditorGUILayout.HelpBox("Editing Gesture Controller on avatar.", MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == fxController)
                    {
                        EditorGUILayout.HelpBox("Editing FX Controller on avatar.", MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController)
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
                    }
                }
            });

            serializedObj.ApplyModifiedProperties();
        }

        void UpdateAvatar(VRCAvatarDescriptor avatarDescriptor)
        {
            var gestureController = emoteWizardRoot.GetComponent<GestureWizard>()?.outputAsset;
            var sittingController = VrcSdkAssetLocator.SittingLayerController2();

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