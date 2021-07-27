using JetBrains.Annotations;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.UI.Base;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [UsedImplicitly]
    public class TrackingOverrideDrawer : TypedDrawerBase<TrackingOverride>
    {
        public override void OnGUI(Rect position, ref TrackingOverride item, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                TypedGUI.EnumPopup(position.UISliceH(0.0f, 0.5f), " ", ref item.target);
                TypedGUI.EnumPopup(position.UISliceH(0.5f, 0.5f), " ", ref item.type);
            }
        }
    }
}
