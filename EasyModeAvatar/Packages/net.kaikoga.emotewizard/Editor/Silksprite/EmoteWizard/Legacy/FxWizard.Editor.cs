using UnityEditor;

namespace Silksprite.EmoteWizard.Legacy
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("古いコンポーネントです。", MessageType.Warning);
        }
    }
}