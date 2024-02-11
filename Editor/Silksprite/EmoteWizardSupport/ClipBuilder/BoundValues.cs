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
                clip.SetCurve(binding.path, binding.type, binding.propertyName, AnimationCurve.Linear(0f, boundFloat.ValueOff, 1 / 60f, boundFloat.ValueOn));
            }
            foreach (var boundObject in _boundObjects)
            {
                AnimationUtility.SetObjectReferenceCurve(clip, boundObject.Binding, new []
                {
                    new ObjectReferenceKeyframe { time = 0, value = boundObject.ValueOff },
                    new ObjectReferenceKeyframe { time = 1 / 60f, value = boundObject.ValueOn }
                });
            }
        }

        public struct BoundObjectValue
        {
            public readonly EditorCurveBinding Binding;
            public readonly Object ValueOff;
            public readonly Object ValueOn;

            public BoundObjectValue(EditorCurveBinding binding, Object valueOff, Object valueOn)
            {
                Binding = binding;
                ValueOff = valueOff;
                ValueOn = valueOn;
            }
        }

        public struct BoundFloatValue
        {
            public readonly EditorCurveBinding Binding;
            public readonly float ValueOff;
            public readonly float ValueOn;

            public BoundFloatValue(EditorCurveBinding binding, float valueOff, float valueOn)
            {
                Binding = binding;
                ValueOff = valueOff;
                ValueOn = valueOn;
            }

            public static BoundFloatValue FromAnimatedValue(AnimatedValue<float> floatValue, Transform avatarRootTransform, bool useSceneOffValue)
            {
                var editorCurveBinding = EditorCurveBinding.FloatCurve(floatValue.Path, floatValue.Type, floatValue.PropertyName);
                if (useSceneOffValue)
                {
                    if (AnimationUtility.GetFloatValue(avatarRootTransform.gameObject, editorCurveBinding, out var sceneValue))
                    {
                        return new BoundFloatValue(editorCurveBinding, sceneValue, floatValue.ValueOn);
                    }
                }
                return new BoundFloatValue(editorCurveBinding, floatValue.ValueOff, floatValue.ValueOn);
            }
        }
    }
}