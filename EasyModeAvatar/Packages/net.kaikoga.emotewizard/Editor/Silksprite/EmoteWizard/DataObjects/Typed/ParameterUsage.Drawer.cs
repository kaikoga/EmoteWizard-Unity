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
    public class ParameterUsageDrawer : TypedDrawerBase<ParameterUsage>
    {
        public override void OnGUI(Rect position, ref ParameterUsage item, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                TypedGUI.EnumPopup(position.UISliceH(0.0f, 0.6f), "Value", ref item.usageKind);
                using (new HideLabelsScope())
                {
                    TypedGUI.FloatField(position.UISliceH(0.6f, 0.4f), " ", ref item.value);
                }
            }
        }
    }
}