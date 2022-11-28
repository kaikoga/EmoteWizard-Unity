using UnityEditor;

namespace Silksprite.EmoteWizard.Legacy
{
    [CustomEditor(typeof(ActionWizard))]
    public class ActionWizardEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("古いコンポーネントです。", MessageType.Warning);
        }
    }
}