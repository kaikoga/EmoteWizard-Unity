using System;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(AnimationMixin))]
    public class AnimationMixinDrawer : HybridDrawerWithContext<AnimationMixin, AnimationMixinDrawerContext>
    {
        public static AnimationMixinDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, string relativePath) => StartContext(new AnimationMixinDrawerContext(emoteWizardRoot, relativePath));

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
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.3f, 0), name);
                    EditorGUI.PropertyField(position.UISlice(0.3f, 0.3f, 0), kind);
                }
                
                switch ((AnimationMixinKind) kind.intValue)
                {
                    case AnimationMixinKind.AnimationClip:
                        using (new HideLabelsScope())
                        {
                            CustomEditorGUI.PropertyFieldWithGenerate(
                                position.UISlice(0.6f, 0.4f, 0),
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
                        EditorGUI.PropertyField(position.UISliceV(1), normalizedTimeEnabled, new GUIContent("Normalized Time"));
                        if (normalizedTimeEnabled.boolValue)
                        {
                            EditorGUI.PropertyField(position.UISliceV(2), property.FindPropertyRelative("normalizedTime"), new GUIContent("Parameter Name"));
                        }
                        break;
                    case AnimationMixinKind.BlendTree:
                        using (new HideLabelsScope())
                        {
                            CustomEditorGUI.PropertyFieldWithGenerate(
                                position.UISlice(0.6f, 0.4f, 0),
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
            switch ((AnimationMixinKind) property.FindPropertyRelative("kind").intValue)
            {
                case AnimationMixinKind.AnimationClip:
                    innerHeight = LineHeight(2f);
                    if (property.FindPropertyRelative("normalizedTimeEnabled").boolValue)
                    {
                        innerHeight = LineHeight(3f);
                    }
                    break;
            }

            return BoxHeight(innerHeight);
        }
        
        public override bool FixedPropertyHeight => false;

        public override string PagerItemName(AnimationMixin property, int index) => property.name;

        public override void OnGUI(Rect position, ref AnimationMixin property, GUIContent label)
        {
            var context = EnsureContext();
            var name = property.name;

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new HideLabelsScope())
            {
                TypedGUI.TextField(position.UISlice(0.0f, 0.3f, 0), "name", ref property.name);
                TypedGUI.EnumPopup(position.UISlice(0.3f, 0.3f, 0), "Kind", ref property.kind);
            }

            switch (property.kind)
            {
                case AnimationMixinKind.AnimationClip:
                    using (new HideLabelsScope())
                    {
                        CustomTypedGUI.AssetFieldWithGenerate(
                            position.UISlice(0.6f, 0.4f, 0),
                            "Animation Clip",
                            ref property.animationClip,
                            () =>
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    Debug.LogError("Mixin Name is required.");
                                    return null;
                                }

                                var relativePath =
                                    $"{context.RelativePath}@@@Generated@@@Mixin_{name}.anim";
                                return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                            });
                    }

                    TypedGUI.Toggle(position.UISliceV(1), new GUIContent("Normalized Time"), ref property.normalizedTimeEnabled);
                    if (property.normalizedTimeEnabled)
                    {
                        TypedGUI.TextField(position.UISliceV(2), new GUIContent("Parameter Name"), ref property.normalizedTime);
                    }

                    break;
                case AnimationMixinKind.BlendTree:
                    using (new HideLabelsScope())
                    {
                        CustomTypedGUI.AssetFieldWithGenerate(
                            position.UISlice(0.6f, 0.4f, 0),
                            "Blend Tree",
                            ref property.blendTree,
                            () =>
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    Debug.LogError("Mixin Name is required.");
                                    return null;
                                }

                                var relativePath =
                                    $"{context.RelativePath}@@@Generated@@@Mixin_{name}.asset";
                                return context.EmoteWizardRoot.EnsureAsset<BlendTree>(relativePath);
                            });
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override float GetPropertyHeight(AnimationMixin property, GUIContent label)
        {
            var innerHeight = LineHeight(1f);
            switch (property.kind)
            {
                case AnimationMixinKind.AnimationClip:
                    innerHeight = LineHeight(2f);
                    if (property.normalizedTimeEnabled)
                    {
                        innerHeight = LineHeight(3f);
                    }
                    break;
            }

            return BoxHeight(innerHeight);
        }
    }
}