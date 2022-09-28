using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
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
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(Emote property, int index) => property.ToStateName();

        public override void OnGUI(Rect position, ref Emote property, GUIContent label)
        {
            var context = EnsureContext();

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            var cursor = position.UISliceV(0, 1);
            if (context.State.EditConditions)
            {
                using (new HideLabelsScope())
                {
                    TypedGUI.Toggle(cursor.UISliceH(0.0f, 0.1f), new GUIContent(" "), ref property.overrideEnabled);
                }
                using (new EditorGUI.DisabledScope(!property.overrideEnabled))
                {
                    TypedGUI.TypedField(cursor.UISliceH(0.1f, 0.9f), ref property.overrideIndex, new GUIContent("HandSign Override"));
                }
                cursor.y += LineTop(1f);

                using (new HideLabelsScope())
                {
                    TypedGUI.TypedField(cursor, ref property.gesture1, new GUIContent(" "));
                    cursor.y += LineTop(1f);
                    TypedGUI.TypedField(cursor, ref property.gesture2, new GUIContent(" "));
                    cursor.y += LineTop(1f);
                }

                using (context.EmoteConditionDrawerContext().StartContext())
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

            if (context.State.EditAnimations)
            {
                var fileName = property.ToStateName(true);
                if (context.AdvancedAnimations)
                {
                    CustomTypedGUI.AssetFieldWithGenerate(cursor, "Clip Left", ref property.clipLeft,
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(context.Layer, fileName, "Left");
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                    cursor.y += LineTop(1f);

                    CustomTypedGUI.AssetFieldWithGenerate(cursor, "Clip Right", ref property.clipRight,
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(context.Layer, fileName, "Right");
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                    cursor.y += LineTop(1f);
                }
                else
                {
                    CustomTypedGUI.AssetFieldWithGenerate(cursor, "Clip", ref property.clipLeft,
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(context.Layer, fileName);
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                    cursor.y += LineTop(1f);
                }
            }

            using (context.EmoteParameterDrawerContext().StartContext())
            {
                TypedGUI.TypedField(cursor, ref property.control, "Control");
                cursor.y += TypedGUI.GetPropertyHeight(property.control, "Control") + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        
        public override float GetPropertyHeight(Emote property, GUIContent label)
        {
            var context = EnsureContext();
            
            var h = 0f;
            if (context.State.EditConditions)
            {
                h += LineHeight(3f) + TypedGUI.GetPropertyHeight(property.conditions, "Conditions") + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                h += LineHeight(1f) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (context.State.EditAnimations)
            {
                h += LineHeight(context.AdvancedAnimations ? 2f : 1f) + EditorGUIUtility.standardVerticalSpacing;
            }

            using (context.EmoteParameterDrawerContext().StartContext())
            {
                h += TypedGUI.GetPropertyHeight(property.control, "Control") + EditorGUIUtility.standardVerticalSpacing;
            }

            return BoxHeight(h);
        }
    }
}