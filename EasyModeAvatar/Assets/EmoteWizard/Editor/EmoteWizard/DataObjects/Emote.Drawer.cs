using EmoteWizard.Base;
using EmoteWizard.DataObjects.DrawerContexts;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(Emote))]
    public class EmoteDrawer : PropertyDrawerWithContext<EmoteDrawerContext>
    {
        public static bool EditConditions = true;
        public static bool EditAnimations = true;
        public static bool EditParameters = false;

        public static EmoteDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, bool advancedAnimations) => PropertyDrawerWithContext<EmoteDrawerContext>.StartContext(new EmoteDrawerContext(emoteWizardRoot, advancedAnimations));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            var cursor = position.UISliceV(0, 1);
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var gesture1 = property.FindPropertyRelative("gesture1");
                var gesture2 = property.FindPropertyRelative("gesture2");
                var conditions = property.FindPropertyRelative("conditions");
                if (EditConditions)
                {
                    using (new HideLabelsScope())
                    {
                        EditorGUI.PropertyField(cursor, gesture1, new GUIContent(" "));
                        cursor.y += LineTop(1f);
                        EditorGUI.PropertyField(cursor, gesture2, new GUIContent(" "));
                        cursor.y += LineTop(1f);
                    }
                    EditorGUI.PropertyField(cursor, conditions, true);
                    cursor.y += EditorGUI.GetPropertyHeight(conditions, true) + EditorGUIUtility.standardVerticalSpacing;    
                }
                else
                {
                    var emoteLabel = Emote.BuildStateName(
                        (GestureConditionMode) gesture1.FindPropertyRelative("mode").intValue,
                        (HandSign) gesture1.FindPropertyRelative("handSign").intValue,
                        (GestureConditionMode) gesture2.FindPropertyRelative("mode").intValue,
                        (HandSign) gesture2.FindPropertyRelative("handSign").intValue);
                    if (conditions.arraySize > 0) emoteLabel += " *";

                    GUI.Label(cursor, emoteLabel, new GUIStyle { fontStyle = FontStyle.Bold });
                    cursor.y += LineTop(1f);
                }

                if (EditAnimations)
                {
                    if (context.AdvancedAnimations)
                    {
                        EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipLeft"));
                        cursor.y += LineTop(1f);
                        EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipRight"));
                        cursor.y += LineTop(1f);
                    }
                    else
                    {
                        EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipLeft"), new GUIContent("Clip"));
                        cursor.y += LineTop(1f);
                    }
                }

                using (EmoteParameterDrawer.StartContext(null, EditParameters))
                {
                    var parameter = property.FindPropertyRelative("parameter");
                    EditorGUI.PropertyField(cursor, parameter, true);
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);
            
            var h = 0f;
            if (EditConditions)
            {
                h += LineHeight(2f) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions"), true) + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                h += LineHeight(1f) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (EditAnimations)
            {
                h += LineHeight(context.AdvancedAnimations ? 2f : 1f) + EditorGUIUtility.standardVerticalSpacing;
            }

            using (EmoteParameterDrawer.StartContext(null, EditParameters))
            {
                h += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("parameter"), true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return BoxHeight(h);
        }
    }
}