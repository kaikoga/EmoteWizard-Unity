using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class ParameterEmoteListDrawerBase : ListDrawerBase
    {
        public override string PagerItemName(SerializedProperty property, int index)
        {
            var name = property.FindPropertyRelative("name").stringValue;
            var kind = (ParameterEmoteKind)property.FindPropertyRelative("emoteKind").intValue;
            return $"{name} ({kind})";                    
        }

        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.1f, 0.2f, 0), "Value");
                GUI.Label(position.UISlice(0.3f, 0.7f, 0), "Motion");
                
                TypedGUI.ToggleLeft(position.UISliceV(1), "Edit Targets (Transition only)", ref ParameterEmoteDrawer.EditTargets);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}