using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class EmoteListDrawerBase : ListDrawerBase
    {
        public override string HeaderName => "Emotes";

        public override string PagerItemName(SerializedProperty property, int index) => PagerNameGeneratorUtils.AsEmoteName(property, index);

        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                EmoteDrawer.EditConditions = EditorGUI.ToggleLeft(position.UISliceV(0), "Edit Conditions", EmoteDrawer.EditConditions);
                EmoteDrawer.EditAnimations = EditorGUI.ToggleLeft(position.UISlice(0.0f, 0.5f, 1), "Edit Animations", EmoteDrawer.EditAnimations);
                EmoteDrawer.EditParameters = EditorGUI.ToggleLeft(position.UISliceV(2), "Edit Parameters", EmoteDrawer.EditParameters);
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(3f));
        }
    }
}