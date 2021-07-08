using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Collections.Base;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizard.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class ExpressionItemListDrawerBase : ListDrawerBase
    {
        public override string HeaderName => "Expression Items";
        public override string PagerItemName(SerializedProperty property, int index) => property.FindPropertyRelative("path").stringValue;

        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.0f, 0.4f, 0), "Icon");
                GUI.Label(position.UISlice(0.4f, 0.6f, 0), "Path");
                
                GUI.Label(position.UISlice(0.0f, 0.4f, 1), "Parameter");
                GUI.Label(position.UISlice(0.4f, 0.2f, 1), "Value");
                GUI.Label(position.UISlice(0.6f, 0.4f, 1), "ControlType");
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}