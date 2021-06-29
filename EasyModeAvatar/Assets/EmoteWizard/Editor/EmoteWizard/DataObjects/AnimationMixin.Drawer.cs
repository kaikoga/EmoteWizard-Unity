using System;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(AnimationMixin))]
    public class AnimationMixinDrawer : PropertyDrawer
    {
        public static void DrawHeader(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, BoxHeight(LineHeight(1f)));
            EmoteWizardGUI.ColoredBox(position, Color.yellow);
            position = position.InsideBox();
            position.xMin += useReorderUI ? 20f : 6f;
            position.xMax -= 6f;
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.0f, 0.3f, 0), "Name");
                GUI.Label(position.Slice(0.3f, 0.3f, 0), "Kind");
                GUI.Label(position.Slice(0.6f, 0.4f, 0), "Asset");
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new HideLabelsScope())
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var kind = property.FindPropertyRelative("kind");
                EditorGUI.PropertyField(position.Slice(0.0f, 0.3f, 0), property.FindPropertyRelative("name"));
                EditorGUI.PropertyField(position.Slice(0.3f, 0.3f, 0), kind);

                switch ((AnimationMixin.AnimationMixinKind) kind.intValue)
                {
                    case AnimationMixin.AnimationMixinKind.AnimationClip:
                        EditorGUI.PropertyField(position.Slice(0.6f, 0.4f, 0), property.FindPropertyRelative("animationClip"));
                        break;
                    case AnimationMixin.AnimationMixinKind.BlendTree:
                        EditorGUI.PropertyField(position.Slice(0.6f, 0.4f, 0), property.FindPropertyRelative("blendTree"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return BoxHeight(LineHeight(1f));
        }
    }
}