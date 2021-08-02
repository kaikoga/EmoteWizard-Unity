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
            TypedGUI.IntField(position.UISliceV(y++), "Index", ref property.emoteIndex);
            TypedGUI.Toggle(position.UISliceV(y++), "Has Exit Time", ref property.hasExitTime);
            TypedGUI.FloatField(position.UISliceV(y++), "Blend In", ref property.blendIn);
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            using (new LabelWidthScope(100f))
            {
                TypedGUI.FloatField(position.UISlice(0.0f, 0.8f, y++), "Transition", ref property.entryTransitionDuration);

                TypedGUI.AssetField(position.UISlice(0.0f, 0.8f, y), "Entry", ref property.entryClip);
                using (new EditorGUI.DisabledScope(property.entryClip == null))
                {
                    using (new LabelWidthScope(1f))
                    {
                        TypedGUI.FloatField(position.UISlice(0.8f, 0.2f, y++), " ", ref property.entryClipExitTime);
                    }
                    TypedGUI.FloatField(position.UISlice(0.0f, 0.8f, y++), "Transition", ref property.postEntryTransitionDuration);
                }

                TypedGUI.AssetField(position.UISlice(0.0f, 0.8f, y), "Clip", ref property.clip);
                using (new LabelWidthScope(1f))
                {
                    using (new EditorGUI.DisabledScope(!property.hasExitTime))
                    {
                        TypedGUI.FloatField(position.UISlice(0.8f, 0.2f, y++), " ", ref property.clipExitTime);
                    }
                }

                TypedGUI.FloatField(position.UISlice(0.0f, 0.8f, y++), "Transition", ref property.exitTransitionDuration);

                TypedGUI.AssetField(position.UISlice(0.0f, 0.8f, y), "Exit", ref property.exitClip);
                using (new EditorGUI.DisabledScope(property.exitClip == null))
                {
                    using (new LabelWidthScope(1f))
                    {
                        TypedGUI.FloatField(position.UISlice(0.8f, 0.2f, y++), " ", ref property.exitClipExitTime);
                    }
                    TypedGUI.FloatField(position.UISlice(0.0f, 0.8f, y++), "Transition", ref property.postExitTransitionDuration);
                }
            }
            TypedGUI.FloatField(position.UISliceV(y), "Blend Out", ref property.blendOut);
        }

        public override float GetPropertyHeight(ActionEmote property, GUIContent label)
        {
            return BoxHeight(LineHeight(12f));
        }
    }
}