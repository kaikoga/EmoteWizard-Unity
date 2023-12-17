using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ClipBuilders
{
    public static class ResetValuesExtractor
    {
        public static BoundValues ExtractFromAvatarRoot(CurveBindings bindings, GameObject avatarRoot)
        {
            void WarnBindingNotFound(EditorCurveBinding binding)
            {
                Debug.LogWarning($@"{avatarRoot.name}: ResetClip may be insufficient because animated property is not found in avatar.
Object Path: {binding.path}
Property: {binding.type} {binding.propertyName}
This property is not included in ResetClip.", avatarRoot);
            }

            var boundFloats = new List<BoundValues.BoundFloatValue>();
            foreach (var binding in bindings.Floats)
            {
                if (AnimationUtility.GetFloatValue(avatarRoot, binding, out var value))
                {
                    boundFloats.Add(new BoundValues.BoundFloatValue(binding, value));
                }
                else
                {
                    WarnBindingNotFound(binding);
                }
            }

            var boundObjects = new List<BoundValues.BoundObjectValue>();
            foreach (var objectBinding in bindings.Objects)
            {
                if (AnimationUtility.GetObjectReferenceValue(avatarRoot, objectBinding, out var value))
                {
                    boundObjects.Add(new BoundValues.BoundObjectValue(objectBinding, value));
                }
                else
                {
                    WarnBindingNotFound(objectBinding);
                }
            }

            return new BoundValues(boundFloats, boundObjects);
        }
    }
}