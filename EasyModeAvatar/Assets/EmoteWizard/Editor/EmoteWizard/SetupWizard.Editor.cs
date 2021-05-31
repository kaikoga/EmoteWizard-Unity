using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;

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
                setupWizard.EnsureComponent<GestureWizard>();
                setupWizard.EnsureComponent<FxWizard>();
                setupWizard.EnsureComponent<ExpressionWizard>();
                setupWizard.EnsureComponent<ParametersWizard>();
            }
            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroyImmediate(setupWizard);
            }
        }
    }
}