using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class EmoteListHeaderDrawer : ListHeaderDrawerBase
    {
        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                TypedGUI.ToggleLeft(position.UISliceV(0), "Edit Conditions", ref EmoteDrawer.EditConditions);
                TypedGUI.ToggleLeft(position.UISlice(0.0f, 0.5f, 1), "Edit Animations", ref EmoteDrawer.EditAnimations);
                TypedGUI.ToggleLeft(position.UISliceV(2), "Edit Parameters", ref EmoteDrawer.EditParameters);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}