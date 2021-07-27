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
                TypedGUI.Toggle(position.UISliceV(0), "Enabled", ref property.enabled);
                TypedGUI.TextField(position.UISliceV(1), "Name", ref property.name);
                using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.parameter)))
                {
                    TypedGUI.TextField(position.UISlice(0.0f, 0.8f, 2), "Parameter", ref property.parameter);
                }

                using (new HideLabelsScope())
                {
                    TypedGUI.EnumPopup(position.UISlice(0.8f, 0.2f, 2), "Value Kind", ref property.valueKind);
                }

                TypedGUI.EnumPopup(position.UISliceV(3), "Emote Kind", ref property.emoteKind);
            }

            if (property.emoteKind == ParameterEmoteKind.Unused) return;

            var editTargets = context.State.EditTargets && property.emoteKind == ParameterEmoteKind.Transition;
            using (new ParameterEmoteStateDrawerContext(context.EmoteWizardRoot, context.Layer, property.name, editTargets).StartContext())
            {
                TypedGUI.TypedField(position.UISliceV(4, -4), ref property.states, "States");
            }

            if (editTargets && IsExpandedTracker.GetIsExpanded(property.states))
            {
                if (GUI.Button(position.UISliceV(-1), "Generate clips from targets"))
                {
                    context.AnimationWizardBase.GenerateParameterEmoteClipsFromTargets(context, property.name);
                }
            }
        }

        public override float GetPropertyHeight(ParameterEmote property, GUIContent label)
        {
            var context = EnsureContext();

            var states = property.states;
            var emoteKind = property.emoteKind;
            var editTargets = context.State.EditTargets && emoteKind == ParameterEmoteKind.Transition;
            var statesLines = 0f;
            if (emoteKind != ParameterEmoteKind.Unused)
            {
                if (IsExpandedTracker.GetIsExpanded(states)) statesLines += (editTargets ? 2f : 1f) + states.Count * (editTargets ? 2f : 1f);
                statesLines += 1f;
            }
            return BoxHeight(LineHeight(4f + statesLines));
        }
    }
}