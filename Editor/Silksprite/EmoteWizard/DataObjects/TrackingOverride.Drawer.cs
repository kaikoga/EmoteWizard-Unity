using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

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
                EmoteWizardGUI.Prop(position, target);
            }
        }
    }
}
