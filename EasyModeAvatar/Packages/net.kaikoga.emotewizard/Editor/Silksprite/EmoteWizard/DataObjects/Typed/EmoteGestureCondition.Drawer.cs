using JetBrains.Annotations;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.UI.Base;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Typed
{
    [UsedImplicitly]
    public class EmoteGestureConditionDrawer : TypedDrawerBase<EmoteGestureCondition>
    {
        public override void OnGUI(Rect position, ref EmoteGestureCondition property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                TypedGUI.EnumPopup(position.UISliceH(0.00f, 0.33f), new GUIContent(" "), ref property.parameter);
                TypedGUI.EnumPopup(position.UISliceH(0.33f, 0.33f), new GUIContent(" "), ref property.mode);
                TypedGUI.EnumPopup(position.UISliceH(0.66f, 0.33f), new GUIContent(" "), ref property.handSign);
            }
        }
    }
}