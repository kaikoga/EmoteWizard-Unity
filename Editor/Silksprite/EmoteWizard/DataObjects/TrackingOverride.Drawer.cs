using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(TrackingOverride))]
    public class TrackingOverrideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = property.Lop(nameof(TrackingOverride.target), Loc("TrackingOverride::target"));
            using (new EditorGUI.IndentLevelScope())
            {
                LEditorGUI.Prop(position, target);
            }
        }
    }
}
