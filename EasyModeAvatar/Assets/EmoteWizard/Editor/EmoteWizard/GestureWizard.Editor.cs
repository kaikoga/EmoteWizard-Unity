using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : Editor
    {
        GestureWizard gestureWizard;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create"))
            {
                Debug.Log(gestureWizard);
            }
        }
    }
}