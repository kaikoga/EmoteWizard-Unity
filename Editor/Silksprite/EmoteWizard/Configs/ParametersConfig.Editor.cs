using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ParametersConfig))]
    public class ParametersConfigEditor : Editor
    {
        ParametersConfig _config;

        SerializedProperty _serializedOutputAsset;

        void OnEnable()
        {
            _config = (ParametersConfig)target;

            _serializedOutputAsset = serializedObject.FindProperty(nameof(ParametersConfig.outputAsset));
        }

        public override void OnInspectorGUI()
        {
            var env = _config.CreateEnv();

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
                if (GUILayout.Button("Generate Expression Parameters"))
                {
                    _config.GetContext(_config.CreateEnv()).BuildOutputAsset();
                }

                EditorGUILayout.PropertyField(_serializedOutputAsset);
            });
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(env, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Expression Parametersを管理するために必要なコンポーネントです。");
    }
}