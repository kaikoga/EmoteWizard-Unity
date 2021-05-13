using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.EmoteWizardEditorTools;

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
            base.OnInspectorGUI();

            using (new GUILayout.HorizontalScope())
            {
                emoteWizardRoot.generatedAssetRoot = EditorGUILayout.TextField("Generated Assets Root", emoteWizardRoot.generatedAssetRoot);
                if (GUILayout.Button("Browse"))
                {
                    SelectFolder("Select Generated Assets Root", ref emoteWizardRoot.generatedAssetRoot);
                }
            }
            if (GUILayout.Button("Generate Wizards"))
            {
                EnsureComponent<GestureWizard>();
                EnsureComponent<FxWizard>();
            }
        }

        void EnsureComponent<T>()
            where T : Component
        {
            var go = emoteWizardRoot.gameObject;
            if (!go.GetComponent<T>()) go.AddComponent<T>();
        }
    }
}