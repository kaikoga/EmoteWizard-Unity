using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizard.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteParameter))]
    public class EmoteParameterDrawer : PropertyDrawerWithContext<EmoteParameterDrawerContext>
    {
        public static EmoteParameterDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, bool isEditing) => PropertyDrawerWithContext<EmoteParameterDrawerContext>.StartContext(new EmoteParameterDrawerContext(emoteWizardRoot, isEditing));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var normalizedTimeEnabled = property.FindPropertyRelative("normalizedTimeEnabled");
                var normalizedTimeLeft = property.FindPropertyRelative("normalizedTimeLeft");
                var normalizedTimeRight = property.FindPropertyRelative("normalizedTimeRight");
                if (context.IsEditing)
                {
                    EditorGUI.PropertyField(position.UISliceV(0), normalizedTimeEnabled, new GUIContent("Normalized Time"));
                    using (new EditorGUI.IndentLevelScope())
                    using (new EditorGUI.DisabledScope(!normalizedTimeEnabled.boolValue))
                    {
                        EditorGUI.PropertyField(position.UISliceV( 1), normalizedTimeLeft, new GUIContent("Parameter Left"));
                        EditorGUI.PropertyField(position.UISliceV(2), normalizedTimeRight, new GUIContent("Parameter Right"));
                    }
                }
                else
                {
                    var parameterLabel = "";
                    if (normalizedTimeEnabled.boolValue)
                    {
                        parameterLabel += $"Normalized Time:{normalizedTimeLeft.stringValue}/{normalizedTimeRight.stringValue})";
                    }
                    GUI.Label(position, parameterLabel);
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            if (context.IsEditing)
            {
                return LineHeight(3f);
            }
            if (property.FindPropertyRelative("normalizedTimeEnabled").boolValue)
            {
                return LineHeight(1f);
            }
            return LineHeight(0f);
        }
    }
}