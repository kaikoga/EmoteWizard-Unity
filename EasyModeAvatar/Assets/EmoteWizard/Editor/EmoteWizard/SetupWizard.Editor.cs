using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard
{
    [CustomEditor(typeof(SetupWizard))]
    public class SetupWizardEditor : Editor
    {
        SetupWizard setupWizard;

        void OnEnable()
        {
            setupWizard = target as SetupWizard;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Wizards"))
            {
                EnsureComponent<GestureWizard>();
                EnsureComponent<FxWizard>();
            }
            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroyImmediate(setupWizard);
            }
        }

        void EnsureComponent<T>()
            where T : Component
        {
            var go = setupWizard.gameObject;
            if (!go.GetComponent<T>()) go.AddComponent<T>();
        }
    }
}