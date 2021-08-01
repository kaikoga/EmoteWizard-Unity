using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class ActionEmoteListHeaderDrawer : ListHeaderDrawerWithContext<ActionEmote, ActionEmoteDrawerContext>
    {
        protected override void DrawHeaderContent(Rect position)
        {
            var context = EnsureContext();
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(1f));
        }
    }
}