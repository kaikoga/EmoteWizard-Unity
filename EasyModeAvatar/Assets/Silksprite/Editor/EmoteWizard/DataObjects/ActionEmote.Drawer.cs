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
    public class ActionEmoteDrawer : TypedDrawerWithContext<ActionEmote, ActionEmoteDrawerContext>
    {
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(ActionEmote property, int index) => property.name;

        public override void OnGUI(Rect position, ref ActionEmote property, GUIContent label)
        {
            var context = EnsureContext();

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            var y = 0;

            TypedGUI.TextField(position.UISliceV(y++), "Name", ref property.name);
            if (!context.IsDefaultAfk)
            {
                using (new InvalidValueScope(property.emoteIndex == 0))
                {
                    TypedGUI.IntField(position.UISliceV(y++), "Select Value", ref property.emoteIndex);
                }
            }

            TypedGUI.Toggle(position.UISliceV(y++), "Has Exit Time", ref property.hasExitTime);
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            using (new LabelWidthScope(80f))
            {
                if (context.State.EditLayerBlend) TypedGUI.FloatField(position.UISliceV(y++), "Blend In", ref property.blendIn);

                if (context.State.EditTransition)
                {
                    TransitionField(position, y++, context.FixedTransitionDuration, ref property.entryTransitionDuration);
                    ClipField(position, y++, ref property.entryClip, ref property.entryClipExitTime);
                    using (new EditorGUI.DisabledScope(property.entryClip == null))
                    {
                        TransitionField(position, y++, context.FixedTransitionDuration, ref property.postEntryTransitionDuration);
                    }
                }

                ClipField(position, y++, ref property.clip, ref property.clipExitTime);

                if (context.State.EditTransition)
                {
                    TransitionField(position, y++, context.FixedTransitionDuration, ref property.exitTransitionDuration);
                    ClipField(position, y++, ref property.exitClip, ref property.exitClipExitTime);
                    using (new EditorGUI.DisabledScope(property.exitClip == null))
                    {
                        TransitionField(position, y++, context.FixedTransitionDuration, ref property.postExitTransitionDuration);
                    }
                }

                if (context.State.EditLayerBlend) TypedGUI.FloatField(position.UISliceV(y), "Blend Out", ref property.blendOut);
            }
        }

        static void TransitionField(Rect position, int y, bool fixedTransitionDuration, ref float propertyField)
        {
            GUI.Label(position.UISliceV(y), "Transition");
            using (new LabelWidthScope(1f))
            {
                if (fixedTransitionDuration)
                {
                    TypedGUI.FloatField(position.UISlice(0.6f, 0.3f, y), "Transition", ref propertyField);
                    GUI.Label(position.UISlice(0.9f, 0.05f, y), "s");
                }
                else
                {
                    TypedGUI.FloatField(position.UISlice(0.6f, 0.3f, y), "Transition", ref propertyField);
                }
            }
        }

        static void ClipField(Rect position, int y, ref Motion propertyClipField, ref float propertyExitTimeField)
        {
            TypedGUI.AssetField(position.UISlice(0.0f, 0.65f, y), "Entry", ref propertyClipField);
            using (new EditorGUI.DisabledScope(propertyClipField == null))
            {
                using (new LabelWidthScope(1f))
                {
                    TypedGUI.FloatField(position.UISlice(0.65f, 0.2f, y), " ", ref propertyExitTimeField);
                    if (!propertyClipField) return;
                    var realExitTime = propertyExitTimeField * propertyClipField.averageDuration;
                    GUI.Label(position.UISlice(0.85f, 0.15f, y), $"{realExitTime:F2}s");
                }
            }
        }

        public override float GetPropertyHeight(ActionEmote property, GUIContent label)
        {
            var context = EnsureContext();
            var lines = 4f;
            if (context.State.EditLayerBlend) lines += 2f;
            if (context.State.EditTransition) lines += 6f;
            return BoxHeight(LineHeight(lines));
        }
    }
}