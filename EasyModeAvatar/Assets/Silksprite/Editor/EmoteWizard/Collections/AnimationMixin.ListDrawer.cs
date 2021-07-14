using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class AnimationMixinListDrawer : ListDrawerBase
    {
        public override string PagerItemName(SerializedProperty property, int index) => property.FindPropertyRelative("name").stringValue;

        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.0f, 0.3f, 0), "Name");
                GUI.Label(position.UISlice(0.3f, 0.3f, 0), "Kind");
                GUI.Label(position.UISlice(0.6f, 0.4f, 0), "Asset");
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(1f));
        }
    }
}