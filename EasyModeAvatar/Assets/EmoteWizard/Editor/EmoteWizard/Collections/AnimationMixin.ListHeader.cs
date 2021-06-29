using EmoteWizard.Collections.Base;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections
{
    public class AnimationMixinListHeaderDrawer : ListHeaderDrawer
    {
        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.0f, 0.3f, 0), "Name");
                GUI.Label(position.Slice(0.3f, 0.3f, 0), "Kind");
                GUI.Label(position.Slice(0.6f, 0.4f, 0), "Asset");
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(1f));
        }
    }
}