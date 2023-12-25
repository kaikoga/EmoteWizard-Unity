using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(AnimatorLayerConfigBase), true)]
    public class AnimatorLayerConfigBaseEditor : Editor
    {
        AnimatorLayerConfigBase _config;

        SerializedProperty _serializedDefaultAvatarMask;
        SerializedProperty _serializedOutputAsset;
        SerializedProperty _serializedHasResetClip;
        SerializedProperty _serializedResetClip;

        void OnEnable()
        {
            _config = (AnimatorLayerConfigBase)target;

            _serializedDefaultAvatarMask = serializedObject.FindProperty(nameof(AnimatorLayerConfigBase.defaultAvatarMask));
            _serializedOutputAsset = serializedObject.FindProperty(nameof(AnimatorLayerConfigBase.outputAsset));
            _serializedHasResetClip = serializedObject.FindProperty("hasResetClip");
            _serializedResetClip = serializedObject.FindProperty(nameof(AnimatorLayerConfigBase.resetClip));
        }

        public override void OnInspectorGUI()
        {
            var env = _config.CreateEnv();

            using (new ObjectChangeScope(_config))
            {
                if (_config.LayerKind == LayerKind.Gesture)
                {
                    CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedDefaultAvatarMask, () =>
                    {
                        var avatarMask = env.EnsureAsset<AvatarMask>(GeneratedPaths.GestureDefaultMask);
                        return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
                    });
                }
                else
                {
                    EditorGUILayout.PropertyField(_serializedDefaultAvatarMask);
                }

                EditorGUILayout.PropertyField(_serializedHasResetClip);

                EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        _config.GetContext(_config.CreateEnv()).BuildOutputAsset(env.GetContext<ParametersContext>().Snapshot());
                    }

                    EditorGUILayout.PropertyField(_serializedOutputAsset);
                    using (new EditorGUI.DisabledScope(!_config.hasResetClip))
                    {
                        EditorGUILayout.PropertyField(_serializedResetClip);
                    }
                });
            }
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(env, Tutorial);
            EmoteWizardGUILayout.Tutorial(env, Tutorial2);
        }

        string Tutorial => 
            string.Join("\n",
                $"{_config.LayerKind} Layerの設定を行い、Animation Controllerを生成します。");

        string Tutorial2 => 
            string.Join("\n",
                "Write Defaultsオフでセットアップされます。",
                $"Has Reset Clipがオンの場合、各アニメーションで使われているパラメータをリセットするアニメーションが{_config.LayerKind} Layerの一番上に追加されます。");
    }
}