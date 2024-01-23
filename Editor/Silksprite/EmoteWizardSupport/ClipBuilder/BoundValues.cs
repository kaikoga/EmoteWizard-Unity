using System.Collections.Generic;
using Silksprite.EmoteWizard.ClipBuilder;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.ClipBuilder
{
    public readonly struct BoundValues
    {
        readonly List<BoundFloatValue> _boundFloats;
        readonly List<BoundObjectValue> _boundObjects;

        public BoundValues(List<BoundFloatValue> boundFloats, List<BoundObjectValue> boundObjects)
        {
            _boundFloats = boundFloats;
            _boundObjects = boundObjects;
        }

        public void Build(AnimationClip clip)
        {
            clip.ClearCurves();
            clip.frameRate = 60f;

            foreach (var boundFloat in _boundFloats)
            {
                var binding = boundFloat.Binding;
                clip.SetCurve(binding.path, binding.type, binding.propertyName, AnimationCurve.Constant(0f, 1 / 60f, boundFloat.Value));
            }
            foreach (var boundObject in _boundObjects)
            {
                AnimationUtility.SetObjectReferenceCurve(clip, boundObject.Binding, new []
                {
                    new ObjectReferenceKeyframe { time = 0, value = boundObject.Value },
                    new ObjectReferenceKeyframe { time = 1 / 60f, value = boundObject.Value }
                });
            }
        }

        public struct BoundObjectValue
        {
            public readonly EditorCurveBinding Binding;
            public readonly Object Value;

            public BoundObjectValue(EditorCurveBinding binding, Object value)
            {
                Binding = binding;
                Value = value;
            }
        }

        public struct BoundFloatValue
        {
            public readonly EditorCurveBinding Binding;
            public readonly float Value;

            public BoundFloatValue(EditorCurveBinding binding, float value)
            {
                Binding = binding;
                Value = value;
            }

            public static BoundFloatValue FromAnimatedValue(AnimatedValue<float> floatValue)
            {
                var editorCurveBinding = EditorCurveBinding.FloatCurve(floatValue.Path, floatValue.Type, floatValue.PropertyName);
                return new BoundFloatValue(editorCurveBinding, floatValue.Value);
            }
        }
    }
}