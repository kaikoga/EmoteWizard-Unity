using UnityEditor;

namespace Silksprite.EmoteWizard.Legacy
{
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("古いコンポーネントです。", MessageType.Warning);
        }
    }
}