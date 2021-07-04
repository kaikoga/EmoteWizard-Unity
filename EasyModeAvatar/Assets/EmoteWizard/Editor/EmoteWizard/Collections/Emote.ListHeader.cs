using EmoteWizard.Collections.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections
{
    public class EmoteListHeaderDrawer : ListHeaderDrawer
    {
        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                EmoteDrawer.EditConditions = EditorGUI.ToggleLeft(position.UISliceV(0), "Edit Conditions", EmoteDrawer.EditConditions);
                EmoteDrawer.EditAnimations = EditorGUI.ToggleLeft(position.UISliceV(1), "Edit Animations", EmoteDrawer.EditAnimations);
                EmoteDrawer.EditParameters = EditorGUI.ToggleLeft(position.UISliceV(2), "Edit Parameters", EmoteDrawer.EditParameters);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}