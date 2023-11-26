using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(AvatarWizard))]
    public class AvatarWizardEditor : Editor 
    {
        AvatarWizard _wizard;

        void OnEnable()
        {
            _wizard = (AvatarWizard)target;
        }

        public override void OnInspectorGUI()
        {
            RuntimeAnimatorController GenerateOverrideController(RuntimeAnimatorController source, string layer)
            {
                var path = AssetDatabase.GetAssetPath(source);
                var newPath = _wizard.CreateEnv().GeneratedAssetPath(GeneratedAssetLocator.GeneratedOverrideControllerPath(layer));
                EnsureDirectory(newPath);
                AssetDatabase.CopyAsset(path, newPath);
                return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(newPath);
            }

            var env = _wizard.CreateEnv();

            var overrideGestureLabel = new GUIContent("Override Gesture", "Gestureレイヤーで使用するAnimatorControllerを選択します。\nGenerate: EmoteWizardが生成するものを使用\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(AvatarWizard.overrideGesture)), overrideGestureLabel);
            if (_wizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Override)
            {
                CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObject.FindProperty(nameof(AvatarWizard.overrideGestureController)), () => GenerateOverrideController(VrcSdkAssetLocator.HandsLayerController1(), "Gesture"));
            }
            var overrideActionLabel = new GUIContent("Override Action", "Actionレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault: デフォルトを使用");
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(AvatarWizard.overrideAction)), overrideActionLabel);
            if (_wizard.overrideAction == AvatarWizard.OverrideGeneratedControllerType1.Override)
            {
                CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObject.FindProperty(nameof(AvatarWizard.overrideActionController)), () => GenerateOverrideController(VrcSdkAssetLocator.ActionLayerController(), "Action"));
            }
            var overrideSittingLabel = new GUIContent("Override Sitting", "Sittingレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(AvatarWizard.overrideSitting)), overrideSittingLabel);
            if (_wizard.overrideSitting == AvatarWizard.OverrideControllerType2.Override)
            {
                CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObject.FindProperty(nameof(AvatarWizard.overrideSittingController)), () => GenerateOverrideController(VrcSdkAssetLocator.SittingLayerController1(), "Sitting"));
            }

            EmoteWizardGUILayout.OutputUIArea(env, () =>
            {
                void EditAnimator(AnimatorController animatorController)
                {
                    var animator = _wizard.CreateEnv().ProvideProxyAnimator();
                    animator.runtimeAnimatorController = animatorController;
                    if (!animatorController) return;
                    Selection.SetActiveObjectWithContext(animator.gameObject, animatorController);
                }

                var gestureController = env.GetContext<GestureLayerContext>()?.OutputAsset as AnimatorController;
                var fxController = env.GetContext<FxLayerContext>()?.OutputAsset as AnimatorController;
                var actionController = env.GetContext<ActionLayerContext>()?.OutputAsset as AnimatorController;

                if (env.AvatarDescriptor)
                {
                    var avatarAnimator = env.AvatarDescriptor.EnsureComponent<Animator>();
                    EmoteWizardGUILayout.RequireAnotherContext<ParametersContext, ParametersWizard>(_wizard, () =>
                    {
                        if (GUILayout.Button("Generate Everything and Update Avatar"))
                        {
                            _wizard.GetContext(_wizard.CreateEnv()).BuildAvatar();
                        }
                    });

                    if (avatarAnimator.runtimeAnimatorController == null)
                    {
                        // do nothing
                    }
                    else if (avatarAnimator.runtimeAnimatorController == gestureController)
                    {
                        EditorGUILayout.HelpBox("Editing Gesture Controller on avatar.", MessageType.Warning);
                        EditorGUILayout.HelpBox("選択されたAnimatorを利用して、Gestureレイヤーのアニメーションを編集中です。", MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == fxController)
                    {
                        EditorGUILayout.HelpBox("Editing FX Controller on avatar.", MessageType.Warning);
                        EditorGUILayout.HelpBox("選択されたAnimatorを利用して、FXレイヤーのアニメーションを編集中です。", MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == actionController)
                    {
                        EditorGUILayout.HelpBox("Editing Action Controller on avatar.", MessageType.Warning);
                        EditorGUILayout.HelpBox("選択されたAnimatorを利用して、Actionレイヤーのアニメーションを編集中です。", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Unknown Animator Controller is present.", MessageType.Warning);
                        EditorGUILayout.HelpBox("選択されたAnimatorに不明なAnimator Controllerが刺さっています。", MessageType.Warning);
                    }
                }

                if (env.AvatarDescriptor)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(gestureController == null || _wizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Default1 || _wizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Default2))
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

                        using (new EditorGUI.DisabledScope(actionController == null || _wizard.overrideAction == AvatarWizard.OverrideGeneratedControllerType1.Default))
                        {
                            if (GUILayout.Button("Edit Action"))
                            {
                                EditAnimator(actionController);
                            }
                        }
                    }

                    if (GUILayout.Button("Remove Animator Controller"))
                    {
                        EditAnimator(null);
                    }
                }
            });

            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(env, Tutorial);
        }
        
        static string Tutorial => 
            string.Join("\n",
                "VRCAvatarDescriptorのセットアップを行います。",
                "Animatorコンポーネントが存在する場合、それを利用してアバターのアニメーションを編集できます。");
    }
}