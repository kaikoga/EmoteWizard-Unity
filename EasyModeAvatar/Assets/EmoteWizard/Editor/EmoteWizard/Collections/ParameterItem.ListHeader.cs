using EmoteWizard.Collections.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections
{
    public class ParameterItemListHeaderDrawer : ListHeaderDrawer
    {
        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.00f, 0.40f, 0), "Name");
                GUI.Label(position.Slice(0.40f, 0.25f, 0), "Type");
                GUI.Label(position.Slice(0.65f, 0.20f, 0), "Default");
                GUI.Label(position.Slice(0.85f, 0.15f, 0), "Saved");

                GUI.Label(position.Slice(0.00f, 0.20f, 1), "Value");
                ParameterStateDrawer.DrawGestureClip = GUI.Toggle(position.Slice(0.20f, 0.40f, 1), ParameterStateDrawer.DrawGestureClip, "Gesture Clip");
                ParameterStateDrawer.DrawFxClip = GUI.Toggle(position.Slice(0.60f, 0.40f, 1), ParameterStateDrawer.DrawFxClip, "FX Clip");
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}