using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class ParameterItemDrawer : TypedDrawerWithContext<ParameterItem, ParameterItemDrawerContext>
    {
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(ParameterItem property, int index) => $"{property.name} ({property.itemKind})";

        public override void OnGUI(Rect position, ref ParameterItem item, GUIContent label)
        {
            var context = EnsureContext();

            // GUI.backgroundColor = defaultParameter ? Color.gray : Color.white;  
            GUI.Box(position, GUIContent.none);
            // GUI.backgroundColor = Color.white;

            position = position.InsideBox();
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            using (new HideLabelsScope())
            {
                TypedGUI.ToggleLeft(position.UISlice(0.00f, 0.10f, 0), new GUIContent(" "), ref item.enabled);
                EditorGUI.BeginDisabledGroup(context.DefaultParameters);
                TypedGUI.TextField(position.UISlice(0.10f, 0.35f, 0), new GUIContent(" "), ref item.name);
                TypedGUI.EnumPopup(position.UISlice(0.45f, 0.20f, 0), new GUIContent(" "), ref item.itemKind);
                TypedGUI.FloatField(position.UISlice(0.65f, 0.20f, 0), new GUIContent(" "), ref item.defaultValue);
                TypedGUI.Toggle(position.UISlice(0.85f, 0.15f, 0), new GUIContent(" "), ref item.saved);
            }

            // TypedGUI.ListField(position.UISliceV(1, -1), "Usages", ref item.usages);
            TypedGUI.TypedField(position.UISliceV(1, -1), ref item.usages, "Usages");
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(ParameterItem item, GUIContent label)
        {
            var usages = item.usages;
            var usagesLines = IsExpandedTracker.GetIsExpanded(usages) ? usages.Count + 2f : 1f;
            return BoxHeight(LineHeight(1f + usagesLines));
        }
    }
}