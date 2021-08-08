using Silksprite.EmoteWizard.Extensions;
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
    public class ParameterEmoteDrawer : TypedDrawerWithContext<ParameterEmote, ParameterEmoteDrawerContext>
    {
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(ParameterEmote property, int index) => $"{property.name} ({property.emoteKind})";

        public override void OnGUI(Rect position, ref ParameterEmote property, GUIContent label)
        {
            var context = EnsureContext();

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();

            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                using (new HideLabelsScope())
                {
                    TypedGUI.Toggle(position.UISlice(0.0f, 0.1f, 0), " ", ref property.enabled);
                }
                EditorGUI.BeginDisabledGroup(!property.enabled);
                TypedGUI.TextField(position.UISlice(0.1f, 0.9f, 0),  "Name", ref property.name);
                using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.parameter)))
                {
                    TypedGUI.TextField(position.UISlice(0.0f, 0.8f, 1), "Parameter", ref property.parameter);
                }

                using (new HideLabelsScope())
                {
                    TypedGUI.EnumPopup(position.UISlice(0.8f, 0.2f, 1), "Value Kind", ref property.valueKind);
                }

                TypedGUI.EnumPopup(position.UISliceV(2), "Emote Kind", ref property.emoteKind);
            }

            if (property.emoteKind != ParameterEmoteKind.Unused)
            {
                using (var sub = context.ParameterEmoteStateDrawerContext(property.name, property.emoteKind == ParameterEmoteKind.Transition).StartContext())
                {
                    TypedGUI.TypedField(position.UISliceV(3, -3), ref property.states, "States");
                    if (sub.Context.EditTargets && IsExpandedTracker.GetIsExpanded(property.states))
                    {
                        if (GUI.Button(position.UISliceV(-1), "Generate clips from targets"))
                        {
                            context.AnimationWizardBase.GenerateParameterEmoteClipsFromTargets(context, property.name);
                        }
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(ParameterEmote property, GUIContent label)
        {
            var context = EnsureContext();

            var statesHeight = 0f;
            if (property.emoteKind != ParameterEmoteKind.Unused)
            {
                using (var sub = context.ParameterEmoteStateDrawerContext(property.name, property.emoteKind == ParameterEmoteKind.Transition).StartContext())
                {
                    if (sub.Context.EditTargets) statesHeight += LineTop(1f);
                    statesHeight += TypedGUI.GetPropertyHeight(property.states, "States");
                }
            }

            return BoxHeight(LineTop(3f) + statesHeight);
        }
    }
}