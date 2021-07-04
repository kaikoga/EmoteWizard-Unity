using EmoteWizard.Base;
using EmoteWizard.DataObjects.DrawerContexts;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmoteState))]
    public class ParameterEmoteStateDrawer : PropertyDrawerWithContext<ParameterEmoteStateDrawerContext>
    {
        public static ParameterEmoteStateDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, string layer, string name, bool editTargets) => PropertyDrawerWithContext<ParameterEmoteStateDrawerContext>.StartContext(new ParameterEmoteStateDrawerContext(emoteWizardRoot, layer, name, editTargets));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.IndentLevelScope())
            {
                using (new HideLabelsScope())
                {
                    var value = property.FindPropertyRelative("value");
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.2f, 0), value);
                    EmoteWizardGUI.PropertyFieldWithGenerate(position.UISlice(0.2f, 0.75f, 0),
                        property.FindPropertyRelative("clip"),
                        () =>
                        {
                            if (string.IsNullOrEmpty(context.Name))
                            {
                                Debug.LogError("Emote Name is required.");
                                return null;
                            }

                            var relativePath =
                                $"{context.Layer}/@@@Generated@@@{context.Layer}_{context.Name}_{value.floatValue}.anim";
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                if (context.EditTargets)
                {
                    EmoteWizardGUI.HorizontalListPropertyField(position.UISliceV(1), property.FindPropertyRelative("targets"));
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);
            var result = LineHeight(1f);
            if (!context.EditTargets) return result;
            result += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targets"), true);
            result += EditorGUIUtility.standardVerticalSpacing;
            return result;
        }
    }
}