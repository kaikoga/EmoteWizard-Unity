using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizard.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmote))]
    public class ParameterEmoteDrawer : PropertyDrawerWithContext<ParameterEmoteDrawerContext>
    {
        public static bool EditTargets = true; // FIXME

        public static ParameterEmoteDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, AnimationWizardBase animationWizardBase, string layer, bool editTargets) => PropertyDrawerWithContext<ParameterEmoteDrawerContext>.StartContext(new ParameterEmoteDrawerContext(emoteWizardRoot, animationWizardBase, layer, editTargets));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var name = property.FindPropertyRelative("name");
                var emoteKind = property.FindPropertyRelative("emoteKind");
                using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                {
                    EditorGUI.PropertyField(position.UISliceV(0), property.FindPropertyRelative("enabled"));
                    EditorGUI.PropertyField(position.UISliceV(1), name);
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.8f, 2), property.FindPropertyRelative("parameter"));
                    using (new HideLabelsScope())
                    {
                        EditorGUI.PropertyField(position.UISlice(0.8f, 0.2f, 2), property.FindPropertyRelative("valueKind"));
                    }

                    EditorGUI.PropertyField(position.UISliceV(3), emoteKind);
                }

                var emoteKindValue = (ParameterEmoteKind) emoteKind.intValue;
                if (emoteKindValue == ParameterEmoteKind.Unused) return;

                var editTargets = context.EditTargets && emoteKindValue == ParameterEmoteKind.Transition;
                using (ParameterEmoteStateDrawer.StartContext(context.EmoteWizardRoot, context.Layer, name.stringValue, editTargets))
                {
                    EditorGUI.PropertyField(position.UISliceV(4, -4), property.FindPropertyRelative("states"), true);
                }

                if (editTargets)
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
                if (editTargets) statesLines += 1f;
                if (states.isExpanded) statesLines += 1f + states.arraySize * (context.EditTargets && editTargets ? 2f : 1f);
                statesLines += 1f;
            }
            return BoxHeight(LineHeight(4f + statesLines));
        }
    }
}