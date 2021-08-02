using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class ActionEmoteListHeaderDrawer : ListHeaderDrawerWithContext<ActionEmote, ActionEmoteDrawerContext>
    {
        protected override void DrawHeaderContent(Rect position)
        {
            var context = EnsureContext();
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.0f, 0.8f, 0), "Clip");
                GUI.Label(position.UISlice(0.8f, 0.2f, 0), "ExitTime");
                TypedGUI.ToggleLeft(position.UISliceV(1), "Edit Layer Blend", ref context.State.EditLayerBlend);
                TypedGUI.ToggleLeft(position.UISliceV(2), "Edit Transition", ref context.State.EditTransition);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}