using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class ActionEmoteDrawer : TypedDrawerWithContext<ActionEmote, ActionEmoteDrawerContext>
    {
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(ActionEmote property, int index) => property.name;

        public override void OnGUI(Rect position, ref ActionEmote property, GUIContent label)
        {
            var context = EnsureContext();
            var name = property.name;

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            TypedGUI.TextField(position.UISliceV(0), "Name", ref property.name);
            TypedGUI.IntField(position.UISliceV(1), "Index", ref property.emoteIndex);
            using (new HideLabelsScope())
            {
            }
        }

        public override float GetPropertyHeight(ActionEmote property, GUIContent label)
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}