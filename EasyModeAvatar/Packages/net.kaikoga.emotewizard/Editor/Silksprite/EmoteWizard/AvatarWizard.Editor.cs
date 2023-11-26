using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
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

            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(env, Tutorial);
        }
        
        static string Tutorial => 
            string.Join("\n",
                "VRCAvatarDescriptorのセットアップを行います。",
                "Animatorコンポーネントが存在する場合、それを利用してアバターのアニメーションを編集できます。");
    }
}