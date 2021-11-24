using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class EditorExtension
    {
        public static void CopyPasteJsonButtons(this Editor editor)
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Copy JSON"))
                {
                    EditorGUIUtility.systemCopyBuffer = EditorJsonUtility.ToJson(editor.target, true);
                }
                if (GUILayout.Button("Paste JSON"))
                {
                    var json = EditorGUIUtility.systemCopyBuffer;
                    EditorJsonUtility.FromJsonOverwrite(json, editor.target);
                }
            }
        }
    }
}