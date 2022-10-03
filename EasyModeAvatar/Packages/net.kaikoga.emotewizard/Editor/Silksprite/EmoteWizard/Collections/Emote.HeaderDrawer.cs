using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class EmoteListHeaderDrawer : ListHeaderDrawerWithContext<Emote, EmoteDrawerContext>
    {
        protected override void DrawHeaderContent(Rect position)
        {
            var context = EnsureContext();
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                TypedGUI.ToggleLeft(position.UISliceV(0), "Edit Conditions", ref context.State.EditConditions);
                TypedGUI.ToggleLeft(position.UISlice(0.0f, 0.5f, 1), "Edit Animations", ref context.State.EditAnimations);
                TypedGUI.ToggleLeft(position.UISliceV(2), "Edit Controls", ref context.State.EditControls);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}