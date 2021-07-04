using EmoteWizard.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmote))]
    public class ParameterEmoteDrawer : PropertyDrawerWithContext<ParameterEmoteDrawer.Context>
    {
        public static Context StartContext(EmoteWizardRoot emoteWizardRoot, string layer) => PropertyDrawerWithContext<Context>.StartContext(new Context(emoteWizardRoot, layer));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                {
                    EditorGUI.PropertyField(position.SliceV(0), property.FindPropertyRelative("name"));
                    EditorGUI.PropertyField(position.Slice(0.0f, 0.8f, 1), property.FindPropertyRelative("parameter"));
                    using (new HideLabelsScope())
                    {
                        EditorGUI.PropertyField(position.Slice(0.8f, 0.2f, 1), property.FindPropertyRelative("valueType"));
                    }
                }

                using (ParameterEmoteStateDrawer.StartContext(context.EmoteWizardRoot, context.Layer, property.FindPropertyRelative("name").stringValue))
                {
                    EditorGUI.PropertyField(position.SliceV(2, -2), property.FindPropertyRelative("states"), true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var states = property.FindPropertyRelative("states");
            var statesLines = states.isExpanded ? states.arraySize + 2f : 1f;
            return BoxHeight(LineHeight(2f + statesLines));
        }

        public class Context : ContextBase
        {
            public readonly string Layer;

            public Context() : base(null) { }
            public Context(EmoteWizardRoot emoteWizardRoot, string layer) : base(emoteWizardRoot) => Layer = layer;
        }
    }
}