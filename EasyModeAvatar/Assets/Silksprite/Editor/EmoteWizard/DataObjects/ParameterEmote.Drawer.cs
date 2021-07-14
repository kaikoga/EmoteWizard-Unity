using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmote))]
    public class ParameterEmoteDrawer : PropertyDrawerWithContext<ParameterEmote, ParameterEmoteDrawerContext>
    {
        public static bool EditTargets = true; // FIXME

        public static ParameterEmoteDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, AnimationWizardBase animationWizardBase, ParametersWizard parametersWizard, string layer, bool editTargets) => PropertyDrawerWithContext<ParameterEmote, ParameterEmoteDrawerContext>.StartContext(new ParameterEmoteDrawerContext(emoteWizardRoot, animationWizardBase, parametersWizard, layer, editTargets));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var name = property.FindPropertyRelative("name");
                var emoteKind = property.FindPropertyRelative("emoteKind");
                var parameter = property.FindPropertyRelative("parameter");
                using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                {
                    EditorGUI.PropertyField(position.UISliceV(0), property.FindPropertyRelative("enabled"));
                    EditorGUI.PropertyField(position.UISliceV(1), name);
                    using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(parameter.stringValue)))
                    {
                        EditorGUI.PropertyField(position.UISlice(0.0f, 0.8f, 2), parameter);
                    }
                    using (new HideLabelsScope())
                    {
                        EditorGUI.PropertyField(position.UISlice(0.8f, 0.2f, 2), property.FindPropertyRelative("valueKind"));
                    }

                    EditorGUI.PropertyField(position.UISliceV(3), emoteKind);
                }

                var emoteKindValue = (ParameterEmoteKind) emoteKind.intValue;
                if (emoteKindValue == ParameterEmoteKind.Unused) return;

                var editTargets = context.EditTargets && emoteKindValue == ParameterEmoteKind.Transition;
                var states = property.FindPropertyRelative("states");
                using (ParameterEmoteStateDrawer.StartContext(context.EmoteWizardRoot, context.Layer, name.stringValue, editTargets))
                {
                    EditorGUI.PropertyField(position.UISliceV(4, -4), states, true);
                }
                if (editTargets && states.isExpanded)
                {
                    if (GUI.Button(position.UISliceV(-1), "Generate clips from targets"))
                    {
                        context.AnimationWizardBase.GenerateParameterEmoteClipsFromTargets(context, name.stringValue);
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            var states = property.FindPropertyRelative("states");
            var emoteKind = (ParameterEmoteKind) property.FindPropertyRelative("emoteKind").intValue;
            var editTargets = context.EditTargets && emoteKind == ParameterEmoteKind.Transition;
            var statesLines = 0f;
            if (emoteKind != ParameterEmoteKind.Unused)
            {
                if (states.isExpanded) statesLines += (editTargets ? 2f : 1f) + states.arraySize * (editTargets ? 2f : 1f);
                statesLines += 1f;
            }
            return BoxHeight(LineHeight(4f + statesLines));
        }
    }
}