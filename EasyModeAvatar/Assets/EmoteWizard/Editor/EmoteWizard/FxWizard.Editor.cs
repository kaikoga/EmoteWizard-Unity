using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : Editor
    {
        FxWizard fxWizard;

        void OnEnable()
        {
            fxWizard = target as FxWizard;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create"))
            {
                Debug.Log(fxWizard);
            }
        }
    }
}