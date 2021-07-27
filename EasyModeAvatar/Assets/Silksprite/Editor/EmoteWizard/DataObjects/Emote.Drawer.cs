using System;
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
    public class EmoteDrawer : TypedDrawerWithContext<Emote, EmoteDrawerContext>
    {
        public static bool EditConditions = true;
        public static bool EditAnimations = true;
        public static bool EditParameters = false;

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

                using (new EmoteConditionDrawerContext(context.EmoteWizardRoot, context.ParametersWizard).StartContext())
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

            using (new EmoteParameterDrawerContext(context.EmoteWizardRoot, context.ParametersWizard, EditParameters).StartContext())
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

            using (new EmoteParameterDrawerContext(context.EmoteWizardRoot, context.ParametersWizard, EditParameters).StartContext())
            {
                h += TypedGUI.GetPropertyHeight(property.parameter, "Parameter") + EditorGUIUtility.standardVerticalSpacing;
            }

            return BoxHeight(h);
        }
    }
}