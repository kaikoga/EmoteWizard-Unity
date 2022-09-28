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
    public class AnimationMixinListHeaderDrawer : ListHeaderDrawerWithContext<AnimationMixin, AnimationMixinDrawerContext>
    {
        protected override void DrawHeaderContent(Rect position)
        {
            var context = EnsureContext();
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.0f, 0.3f, 0), "Name");
                GUI.Label(position.UISlice(0.3f, 0.3f, 0), "Kind");
                GUI.Label(position.UISlice(0.6f, 0.4f, 0), "Asset");

                TypedGUI.ToggleLeft(position.UISliceV(1), "Edit Conditions", ref context.State.EditConditions);
                TypedGUI.ToggleLeft(position.UISliceV(2), "Edit Controls", ref context.State.EditControls);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}