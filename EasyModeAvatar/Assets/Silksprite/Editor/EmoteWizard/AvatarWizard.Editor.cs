using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
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

            var emoteWizardRoot = avatarWizard.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(avatarWizard))
            {
                var overrideGestureLabel = new GUIContent("Override Gesture", "Gestureレイヤーで使用するAnimatorControllerを選択します。\nGenerate: EmoteWizardが生成するものを使用\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
                TypedGUILayout.EnumPopup(overrideGestureLabel, ref avatarWizard.overrideGesture);
                if (avatarWizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Override)
                {
                    CustomTypedGUILayout.AssetFieldWithGenerate("Override Gesture Controller", ref avatarWizard.overrideGestureController, () => GenerateOverrideController(VrcSdkAssetLocator.HandsLayerController1(), "Gesture"));
                }
                var overrideActionLabel = new GUIContent("Override Action", "Actionレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault: デフォルトを使用");
                TypedGUILayout.EnumPopup(overrideActionLabel, ref avatarWizard.overrideAction);
                if (avatarWizard.overrideAction == AvatarWizard.OverrideGeneratedControllerType1.Override)
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
                    var actionController = emoteWizardRoot.GetWizard<ActionWizard>()?.outputAsset as AnimatorController;

                    if (avatarDescriptor)
                    {
                        var avatarAnimator = avatarWizard.avatarDescriptor.EnsureComponent<Animator>();
                        EmoteWizardGUILayout.RequireAnotherWizard<ParametersWizard>(avatarWizard, () =>
                        {
                            if (GUILayout.Button("Generate Everything and Update Avatar"))
                            {
                                avatarWizard.BuildAvatar();
                            }
                        });

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
                        else if (avatarAnimator.runtimeAnimatorController == actionController)
                        {
                            EditorGUILayout.HelpBox("Editing Action Controller on avatar.", MessageType.Warning);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("Unknown Animator Controller is present.", MessageType.Warning);
                        }
                    }

                    var proxyAnimatorLabel = new GUIContent("Proxy Animator", "アバターのアニメーションを編集する際に使用するAnimatorを別途選択できます。");
                    TypedGUILayout.ReferenceField(proxyAnimatorLabel, ref avatarWizard.proxyAnimator);

                    if (avatarDescriptor)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            using (new EditorGUI.DisabledScope(gestureController == null || avatarWizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Default1 || avatarWizard.overrideGesture == AvatarWizard.OverrideGeneratedControllerType2.Default2))
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

                            using (new EditorGUI.DisabledScope(actionController == null || avatarWizard.overrideAction == AvatarWizard.OverrideGeneratedControllerType1.Default))
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
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }
        
        static string Tutorial => 
            string.Join("\n",
                "VRCAvatarDescriptorの更新を行います。",
                "Animatorコンポーネントが存在するなら、それを使ってアバターのアニメーションの編集を開始することができます。");
    }
}