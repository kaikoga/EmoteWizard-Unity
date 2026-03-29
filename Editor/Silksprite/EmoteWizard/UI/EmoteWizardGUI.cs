using Silksprite.EmoteWizard.DataObjects;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using Silksprite.Loch.IMGUI.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.UI
{
    public static class EmoteWizardGUI
    {
        public static void PropAsParameterValue(Rect position, LocalizedProperty lop, ParameterItemKind itemKind)
        {
            switch (itemKind)
            {
                case ParameterItemKind.HandSign:
                    using (new ShowMixedValueScope(lop))
                    {
                        EditorGUI.BeginChangeCheck();
                        var handSignValue = (HandSign)(int)lop.Property.floatValue;
                        handSignValue = LEditorGUI.EnumPopup(position, lop.Loc, handSignValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            lop.Property.floatValue = (int)handSignValue;
                        }
                    }
                    break;
                default:
                    LEditorGUI.Prop(position, lop);
                    break;
            }
        }
    }
}
