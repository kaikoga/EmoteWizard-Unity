using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : Editor
    {
        EmoteWizardRoot _root;
        bool _isSetup;

        SerializedProperty _serializedAvatarDescriptor;
        SerializedProperty _serializedProxyAnimator;
        SerializedProperty _serializedPersistGeneratedAssets;
        SerializedProperty _serializedGeneratedAssetRoot;
        SerializedProperty _serializedGeneratedAssetPrefix;
        SerializedProperty _serializedEmptyClip;
        SerializedProperty _serializedGenerateTrackingControlLayer;
        SerializedProperty _serializedShowTutorial;

        void OnEnable()
        {
            _root = (EmoteWizardRoot)target;

            _serializedAvatarDescriptor = serializedObject.FindProperty(nameof(EmoteWizardRoot.avatarDescriptor));
            _serializedProxyAnimator = serializedObject.FindProperty(nameof(EmoteWizardRoot.proxyAnimator));
            _serializedPersistGeneratedAssets = serializedObject.FindProperty(nameof(EmoteWizardRoot.persistGeneratedAssets));
            _serializedGeneratedAssetRoot = serializedObject.FindProperty(nameof(EmoteWizardRoot.generatedAssetRoot));
            _serializedGeneratedAssetPrefix = serializedObject.FindProperty(nameof(EmoteWizardRoot.generatedAssetPrefix));
            _serializedEmptyClip = serializedObject.FindProperty(nameof(EmoteWizardRoot.emptyClip));
            _serializedGenerateTrackingControlLayer = serializedObject.FindProperty(nameof(EmoteWizardRoot.generateTrackingControlLayer));
            _serializedShowTutorial = serializedObject.FindProperty(nameof(EmoteWizardRoot.showTutorial));
        }

        public override void OnInspectorGUI()
        {
            var env = _root.ToEnv();
            EmoteWizardGUILayout.ConfigUIArea(() =>
            {
                EditorGUILayout.PropertyField(_serializedShowTutorial);
            });

            if (EmoteWizardGUILayout.Undoable("Add Empty Data Source") is IUndoable undoable)
            {
                undoable.AddChildComponentAndSelect<EmoteWizardDataSourceFactory>(_root, "New Source");
            }
            _isSetup = EditorGUILayout.Foldout(_isSetup, "Setup");
            if (_isSetup)
            {
                if (SetupGUI.OnInspectorGUI(env)) return;
            }

            EmoteWizardGUILayout.Header("Avatar");
            var avatarDescriptorLabel = new GUIContent("Avatar Descriptor", "ここで指定したアバターの設定が上書きされます。");
            EditorGUILayout.PropertyField(_serializedAvatarDescriptor, avatarDescriptorLabel);
            var avatarDescriptor = env.AvatarDescriptor;
            if (!avatarDescriptor)
            {
                EditorGUILayout.HelpBox("VRCAvatarDescriptorを設定してください", MessageType.Error);
            }
            else if (!_serializedAvatarDescriptor.objectReferenceValue)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.ObjectField("Detected Avatar Descriptor", env.AvatarDescriptor, typeof(VRCAvatarDescriptor), true);
                }
            }

            EmoteWizardGUILayout.Header("Assets Generation");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_serializedPersistGeneratedAssets);
            if (EditorGUI.EndChangeCheck() && !_serializedPersistGeneratedAssets.boolValue)
            {
                env.DisconnectAllOutputAssets();
            }

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(_serializedGeneratedAssetRoot);
                if (GUILayout.Button("Browse"))
                {
                    SelectFolder("Select Generated Assets Root", _serializedGeneratedAssetRoot);
                }
            }
            EditorGUILayout.PropertyField(_serializedGeneratedAssetPrefix);

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
                CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedEmptyClip, () => _root.ToEnv().ProvideEmptyClip());
            });
            if (GUILayout.Button("Disconnect Output Assets"))
            {
                _root.ToEnv().DisconnectAllOutputAssets();
            }

            EmoteWizardGUILayout.Header("Options");
            EditorGUILayout.PropertyField(_serializedGenerateTrackingControlLayer);
            {
                var overrideGestureLabel = new GUIContent("Override Gesture", "Gestureレイヤーで使用するAnimatorControllerを選択します。\nGenerate: EmoteWizardが生成するものを使用\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EmoteWizardRoot.overrideGesture)), overrideGestureLabel);
                if (env.OverrideGesture == OverrideGeneratedControllerType2.Override)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EmoteWizardRoot.overrideGestureController)));
                }
                var overrideActionLabel = new GUIContent("Override Action", "Actionレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault: デフォルトを使用");
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EmoteWizardRoot.overrideAction)), overrideActionLabel);
                if (env.OverrideAction == OverrideGeneratedControllerType1.Override)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EmoteWizardRoot.overrideActionController)));
                }
                var overrideSittingLabel = new GUIContent("Override Sitting", "Sittingレイヤーで使用するAnimatorControllerを選択します。\nOverride: AnimationControllerを手動指定\nDefault 1: デフォルトを使用（male）\nDefault 2: デフォルトを使用（female）");
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EmoteWizardRoot.overrideSitting)), overrideSittingLabel);
                if (env.OverrideSitting == OverrideControllerType2.Override)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EmoteWizardRoot.overrideSittingController)));
                }
            }

            EmoteWizardGUILayout.Header("Avatar Output");

            var proxyAnimatorLabel = new GUIContent("Proxy Animator", "アバターのアニメーションを編集する際に使用するAnimatorを別途選択できます。");
            EditorGUILayout.PropertyField(_serializedProxyAnimator, proxyAnimatorLabel);

            if (avatarDescriptor)
            {
                EmoteWizardGUILayout.OutputUIArea(true, null, () =>
                {
                    void EditAnimator(RuntimeAnimatorController animatorController)
                    {
                        var animator = _root.ToEnv().ProvideProxyAnimator();
                        animator.runtimeAnimatorController = animatorController;
                        if (!animatorController) return;
                        Selection.SetActiveObjectWithContext(animator.gameObject, animatorController);
                    }

                    var gestureController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Gesture);
                    var fxController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.FX);
                    var actionController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Action);
                    var editorController = env.GetContext<EditorLayerContext>().OutputAsset;

                    var avatarAnimator = RuntimeUndoable.Instance.EnsureComponent<Animator>(env.AvatarDescriptor);
                    if (GUILayout.Button("Disconnect Avatar Output Assets"))
                    {
                        _root.ToEnv().CleanupAvatar();
                    }
                    if (GUILayout.Button("Generate Everything and Update Avatar"))
                    {
                        var undoable = new EditorUndoable("Generate Everything and Update Avatar");
                        undoable.EnsureComponent<EditorLayerConfig>(_root);
                        _root.ToEnv().BuildAvatar(undoable, true);
                    }

                    if (avatarAnimator.runtimeAnimatorController == null)
                    {
                        // do nothing
                    }
                    else if (avatarAnimator.runtimeAnimatorController == editorController)
                    {
                        EditorGUILayout.HelpBox("Editing clips on avatar using Editor Animator.", MessageType.Warning);
                        EditorGUILayout.HelpBox("Editor Animatorを利用して、アニメーションを編集中です。", MessageType.Warning);
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

                    using (new GUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(editorController == null))
                        {
                            if (GUILayout.Button("Edit"))
                            {
                                EditAnimator(editorController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(gestureController == null || env.OverrideGesture == OverrideGeneratedControllerType2.Default1 || env.OverrideGesture == OverrideGeneratedControllerType2.Default2))
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

                        using (new EditorGUI.DisabledScope(actionController == null || env.OverrideAction == OverrideGeneratedControllerType1.Default))
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
                });
            }
 
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(_root.ToEnv(), Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの全体的な設定を行うコンポーネントです。",
                "EmoteWizardが生成したアセットはGenerated Assets Rootで指定したディレクトリの中に入ります。",
                "",
                "基本的な使い方：",
                "（上から順番に操作するのがお勧めです）",
                "1. Emote Wizard RootのSetupボタンを押す",
                "2. Setup WizardのGenerate Wizardsボタンを押して必要なコンポーネントを生成する",
                "3. Setup WizardのQuick Setupを押してプリセットを生成する",
                "4. Avatar Wizardを設定する",
                "5. FX Sourcesに表情や着せ替えの項目を設定する",
                "6. ハンドサインを差し替える場合はGesture Sourcesに設定する",
                "7. 必要に応じてAction SourcesからエモートとAFKモーションを編集する",
                "8. 外部アセットと連携する場合はExpression SourcesとParameter Sourcesを設定する",
                "9. Avatar WizardのUpdate Avatarを押してアバターを更新する",
                "10. Avatar WizardのEdit FXボタンなどを押してアニメーションを編集する");
    }
}