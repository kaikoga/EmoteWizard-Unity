using System;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class ParameterEmoteListDrawerBase : ListDrawerBase
    {
        public override string HeaderName => "Parameter Emotes";
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
                GUI.Label(position.UISlice(0.0f, 0.2f, 0), "Name");
                GUI.Label(position.UISlice(0.2f, 0.8f, 0), "Motion");
                
                ParameterEmoteDrawer.EditTargets = EditorGUI.ToggleLeft(position.UISliceV(1), "Edit Targets (Transition only)", ParameterEmoteDrawer.EditTargets);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}