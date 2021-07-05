using EmoteWizard.Base;
using EmoteWizard.DataObjects.DrawerContexts;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
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
                using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                {
                    EditorGUI.PropertyField(position.UISliceV(0), name);
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.8f, 1), property.FindPropertyRelative("parameter"));
                    using (new HideLabelsScope())
                    {
                        EditorGUI.PropertyField(position.UISlice(0.8f, 0.2f, 1), property.FindPropertyRelative("valueKind"));
                    }
                }

                using (ParameterEmoteStateDrawer.StartContext(context.EmoteWizardRoot, context.Layer, name.stringValue, context.EditTargets))
                {
                    EditorGUI.PropertyField(position.UISliceV(2, -2), property.FindPropertyRelative("states"), true);
                }

                if (context.EditTargets)
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
            var statesLines = 1f;
            if (context.EditTargets) statesLines += 1f;
            if (states.isExpanded) statesLines += 1f + states.arraySize * (context.EditTargets ? 2f : 1f);
            return BoxHeight(LineHeight(2f + statesLines));
        }
    }
}