using EmoteWizard.Collections.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections
{
    public class ParameterEmoteListHeaderDrawer : ListHeaderDrawer
    {
        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.0f, 0.2f, 0), "Name");
                GUI.Label(position.UISlice(0.2f, 0.8f, 0), "Motion");
                
                ParameterEmoteDrawer.EditTargets = EditorGUI.ToggleLeft(position.UISliceV(1), "Edit Targets", ParameterEmoteDrawer.EditTargets);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}