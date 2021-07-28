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
    public class ParameterEmoteListHeaderDrawer : ListHeaderDrawerWithContext<ParameterEmote, ParameterEmoteDrawerContext>
    {
        protected override void DrawHeaderContent(Rect position)
        {
            var context = EnsureContext();

            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.1f, 0.2f, 0), "Value");
                GUI.Label(position.UISlice(0.3f, 0.7f, 0), "Motion");
                
                TypedGUI.ToggleLeft(position.UISliceV(1), "Edit Transition Targets", ref context.State.EditTargets);
                TypedGUI.ToggleLeft(position.UISliceV(2), "Edit Transition Controls", ref context.State.EditControls);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}