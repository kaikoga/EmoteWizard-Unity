using JetBrains.Annotations;
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
            {
                TypedGUI.EnumPopup(position, "Override", ref item.target);
            }
        }
    }
}
