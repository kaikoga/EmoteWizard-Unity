using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
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

            ConfigUIArea(() => { emoteWizardRoot.useReorderUI = EditorGUILayout.ToggleLeft("Use Reorder UI", emoteWizardRoot.useReorderUI); });

            if (!emoteWizardRoot.GetComponent<SetupWizard>())
            {
                if (GUILayout.Button("Setup"))
                {
                    emoteWizardRoot.EnsureComponent<SetupWizard>();
                }
            }

            serializedObj.ApplyModifiedProperties();
        }
    }
}