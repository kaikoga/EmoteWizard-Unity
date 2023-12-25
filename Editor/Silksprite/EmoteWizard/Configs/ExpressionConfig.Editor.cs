using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ExpressionConfig))]
    public class ExpressionConfigEditor : Editor
    {
        ExpressionConfig _config;

        SerializedProperty _serializedBuildAsSubAsset;
        SerializedProperty _serializedOutputAsset;

        void OnEnable()
        {
            _config = (ExpressionConfig)target;

            _serializedBuildAsSubAsset = serializedObject.FindProperty(nameof(ExpressionConfig.buildAsSubAsset));
            _serializedOutputAsset = serializedObject.FindProperty(nameof(ExpressionConfig.outputAsset));
        }

        public override void OnInspectorGUI()
        {
            var env = _config.CreateEnv();

            EditorGUILayout.PropertyField(_serializedBuildAsSubAsset);

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
                if (GUILayout.Button("Generate Expression Menu"))
                {
                    _config.GetContext(_config.CreateEnv()).BuildOutputAsset();
                }

                EditorGUILayout.PropertyField(_serializedOutputAsset);
            });

            serializedObject.ApplyModifiedProperties();
        }
    }
}