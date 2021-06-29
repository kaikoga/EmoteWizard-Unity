using System;
using EmoteWizard.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using EmoteWizard.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(AnimationMixin))]
    public class AnimationMixinDrawer : PropertyDrawerWithContext<AnimationMixinDrawer.Context>
    {
        public static Context StartContext(EmoteWizardRoot emoteWizardRoot, string relativePath) => PropertyDrawerWithContext<Context>.StartContext(new Context(emoteWizardRoot, relativePath));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var kind = property.FindPropertyRelative("kind");
                var name = property.FindPropertyRelative("name");
                using (new HideLabelsScope())
                {
                    EditorGUI.PropertyField(position.Slice(0.0f, 0.3f, 0), name);
                    EditorGUI.PropertyField(position.Slice(0.3f, 0.3f, 0), kind);
                }
                
                switch ((AnimationMixin.AnimationMixinKind) kind.intValue)
                {
                    case AnimationMixin.AnimationMixinKind.AnimationClip:
                        using (new HideLabelsScope())
                        {
                            EmoteWizardGUI.PropertyFieldWithGenerate(
                                position.Slice(0.6f, 0.4f, 0),
                                property.FindPropertyRelative("animationClip"),
                                () =>
                                {
                                    if (string.IsNullOrEmpty(name.stringValue))
                                    {
                                        Debug.LogError("Mixin Name is required.");
                                        return null;
                                    }

                                    var relativePath =
                                        $"{context.RelativePath}@@@Generated@@@Mixin_{name.stringValue}.anim";
                                    return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                                });
                        }

                        var normalizedTimeEnabled = property.FindPropertyRelative("normalizedTimeEnabled");
                        EditorGUI.PropertyField(position.SliceV(1), normalizedTimeEnabled, new GUIContent("Normalized Time"));
                        if (normalizedTimeEnabled.boolValue)
                        {
                            EditorGUI.PropertyField(position.SliceV(2), property.FindPropertyRelative("normalizedTime"), new GUIContent("Parameter Name"));
                        }
                        break;
                    case AnimationMixin.AnimationMixinKind.BlendTree:
                        using (new HideLabelsScope())
                        {
                            EmoteWizardGUI.PropertyFieldWithGenerate(
                                position.Slice(0.6f, 0.4f, 0),
                                property.FindPropertyRelative("blendTree"),
                                () =>
                                {
                                    if (string.IsNullOrEmpty(name.stringValue))
                                    {
                                        Debug.LogError("Mixin Name is required.");
                                        return null;
                                    }

                                    var relativePath =
                                        $"{context.RelativePath}@@@Generated@@@Mixin_{name.stringValue}.asset";
                                    return context.EmoteWizardRoot.EnsureAsset<BlendTree>(relativePath);
                                });
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var innerHeight = LineHeight(1f);
            switch ((AnimationMixin.AnimationMixinKind) property.FindPropertyRelative("kind").intValue)
            {
                case AnimationMixin.AnimationMixinKind.AnimationClip:
                    innerHeight = LineHeight(2f);
                    if (property.FindPropertyRelative("normalizedTimeEnabled").boolValue)
                    {
                        innerHeight = LineHeight(3f);
                    }
                    break;
            }

            return BoxHeight(innerHeight);
        }
        
        public class Context : ContextBase
        {
            public readonly string RelativePath;

            public Context() : base(null) { }
            public Context(EmoteWizardRoot emoteWizardRoot, string relativePath) : base(emoteWizardRoot) => RelativePath = relativePath;
        }
    }
}