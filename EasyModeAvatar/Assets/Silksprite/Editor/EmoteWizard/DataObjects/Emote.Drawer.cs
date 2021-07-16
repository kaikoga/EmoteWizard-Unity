using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(Emote))]
    public class EmoteDrawer : HybridDrawerWithContext<Emote, EmoteDrawerContext>
    {
        public static bool EditConditions = true;
        public static bool EditAnimations = true;
        public static bool EditParameters = false;

        public static EmoteDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, bool advancedAnimations) => StartContext(new EmoteDrawerContext(emoteWizardRoot, parametersWizard, advancedAnimations));

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
                    using (EmoteConditionDrawer.StartContext(context.EmoteWizardRoot, context.ParametersWizard))
                    {
                        EditorGUI.PropertyField(cursor, conditions, true);
                        cursor.y += EditorGUI.GetPropertyHeight(conditions, true) + EditorGUIUtility.standardVerticalSpacing;    
                    }
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

                using (EmoteParameterDrawer.StartContext(context.EmoteWizardRoot, context.ParametersWizard, EditParameters))
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

            using (EmoteParameterDrawer.StartContext(context.EmoteWizardRoot, context.ParametersWizard, EditParameters))
            {
                h += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("parameter"), true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return BoxHeight(h);
        }
        
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(Emote property, int index) => property.ToStateName();

        public override void OnGUI(Rect position, ref Emote property, GUIContent label)
        {
            var context = EnsureContext();

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            var cursor = position.UISliceV(0, 1);
            if (EditConditions)
            {
                using (new HideLabelsScope())
                {
                    TypedGUI.TypedField(cursor, ref property.gesture1, new GUIContent(" "));
                    cursor.y += LineTop(1f);
                    TypedGUI.TypedField(cursor, ref property.gesture2, new GUIContent(" "));
                    cursor.y += LineTop(1f);
                }

                using (EmoteConditionDrawer.StartContext(context.EmoteWizardRoot, context.ParametersWizard))
                {
                    TypedGUI.TypedField(cursor, ref property.conditions, "Conditions");
                    cursor.y += TypedGUI.GetPropertyHeight(property.conditions, "Conditions") + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else
            {
                var emoteLabel = property.ToStateName();
                if (property.conditions.Count > 0) emoteLabel += " *";

                GUI.Label(cursor, emoteLabel, new GUIStyle {fontStyle = FontStyle.Bold});
                cursor.y += LineTop(1f);
            }

            if (EditAnimations)
            {
                if (context.AdvancedAnimations)
                {
                    TypedGUI.AssetField(cursor, "Clip Left", ref property.clipLeft);
                    cursor.y += LineTop(1f);
                    TypedGUI.AssetField(cursor, "Clip Right", ref property.clipRight);
                    cursor.y += LineTop(1f);
                }
                else
                {
                    TypedGUI.AssetField(cursor, "Clip", ref property.clipLeft);
                    cursor.y += LineTop(1f);
                }
            }

            using (EmoteParameterDrawer.StartContext(context.EmoteWizardRoot, context.ParametersWizard, EditParameters))
            {
                TypedGUI.TypedField(cursor, ref property.parameter, "Parameter");
            }
        }
        
        public override float GetPropertyHeight(Emote property, GUIContent label)
        {
            var context = EnsureContext();
            
            var h = 0f;
            if (EditConditions)
            {
                h += LineHeight(2f) + TypedGUI.GetPropertyHeight(property.conditions, "Conditions") + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                h += LineHeight(1f) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (EditAnimations)
            {
                h += LineHeight(context.AdvancedAnimations ? 2f : 1f) + EditorGUIUtility.standardVerticalSpacing;
            }

            using (EmoteParameterDrawer.StartContext(context.EmoteWizardRoot, context.ParametersWizard, EditParameters))
            {
                h += TypedGUI.GetPropertyHeight(property.parameter, "Parameter") + EditorGUIUtility.standardVerticalSpacing;
            }

            return BoxHeight(h);
        }
    }
}