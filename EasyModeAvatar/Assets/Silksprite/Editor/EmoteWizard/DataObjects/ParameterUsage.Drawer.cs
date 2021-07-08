using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterUsage))]
    public class ParameterUsageDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.0f, 0.6f), property.FindPropertyRelative("usageKind"), new GUIContent("Value"));
                using (new HideLabelsScope())
                {
                    EditorGUI.PropertyField(position.UISliceH(0.6f, 0.4f), property.FindPropertyRelative("value"));
                }
            }
        }
    }
}