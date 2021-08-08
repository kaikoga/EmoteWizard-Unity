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
    public class AnimationMixinDrawer : TypedDrawerWithContext<AnimationMixin, AnimationMixinDrawerContext>
    {
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
                TypedGUI.Toggle(position.UISlice(0.0f, 0.1f, 0), " ", ref property.enabled);
            }
            using (new EditorGUI.DisabledScope(!property.enabled))
            {
                TypedGUI.TextField(position.UISlice(0.1f, 0.9f, 0), "Name", ref property.name);
                TypedGUI.EnumPopup(position.UISliceV(1), "Kind", ref property.kind);

                switch (property.kind)
                {
                    case AnimationMixinKind.AnimationClip:
                        CustomTypedGUI.AssetFieldWithGenerate(
                            position.UISliceV(2),
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
                        break;
                    case AnimationMixinKind.BlendTree:
                        CustomTypedGUI.AssetFieldWithGenerate(
                            position.UISliceV(2),
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
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                position = position.UISliceV(3, -3);
                if (context.State.EditConditions)
                {
                    using (context.EmoteConditionDrawerContext().StartContext())
                    {
                        var height = TypedGUI.GetPropertyHeight(property.conditions, new GUIContent("Conditions"));
                        TypedGUI.TypedField(position.SliceV(0, height), ref property.conditions, new GUIContent("Conditions"));
                        position = position.Inset(0, height + EditorGUIUtility.standardVerticalSpacing, 0, 0);
                    }
                }
                using (context.EmoteControlDrawerContext().StartContext())
                {
                    TypedGUI.TypedField(position, ref property.control, new GUIContent("Control"));
                }
            }
        }
        
        public override float GetPropertyHeight(AnimationMixin property, GUIContent label)
        {
            var context = EnsureContext();
            var innerHeight = LineHeight(3f);
            switch (property.kind)
            {
                case AnimationMixinKind.AnimationClip:
                    if (context.State.EditConditions)
                    {
                        innerHeight += TypedGUI.GetPropertyHeight(property.conditions, new GUIContent("Conditions")) + EditorGUIUtility.standardVerticalSpacing;
                    }
                    using (context.EmoteControlDrawerContext().StartContext())
                    {
                        innerHeight += TypedGUI.GetPropertyHeight(property.control, new GUIContent("Control")) + EditorGUIUtility.standardVerticalSpacing; 
                    }
                    break;
            }

            return BoxHeight(innerHeight);
        }
    }
}