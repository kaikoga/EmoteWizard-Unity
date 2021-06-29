using EmoteWizard.Collections.Base;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections
{
    public class ExpressionItemListHeaderDrawer : ListHeaderDrawer
    {
        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.0f, 0.4f, 0), "Icon");
                GUI.Label(position.Slice(0.4f, 0.6f, 0), "Path");
                
                GUI.Label(position.Slice(0.0f, 0.4f, 1), "Parameter");
                GUI.Label(position.Slice(0.4f, 0.2f, 1), "Value");
                GUI.Label(position.Slice(0.6f, 0.4f, 1), "ControlType");
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}