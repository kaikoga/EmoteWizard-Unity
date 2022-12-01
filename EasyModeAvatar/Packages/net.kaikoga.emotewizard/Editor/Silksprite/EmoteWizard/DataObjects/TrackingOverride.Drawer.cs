using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(TrackingOverride))]
    public class TrackingOverrideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.PropertyField(position, property.FindPropertyRelative("target"), new GUIContent("Override"));
            }
        }
    }
}
