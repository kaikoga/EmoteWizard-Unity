using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders
{
    class ConditionBuilder
    {
        readonly List<ConditionElement> _conditions = new List<ConditionElement>();

        public void AddCondition(AnimatorControllerParameterType parameterType, AnimatorConditionMode mode, string parameter, float threshold)
        {
            _conditions.Add(new ConditionElement
            { 
                ParameterType = parameterType,
                Mode = mode,
                Parameter = parameter,
                Threshold = threshold
            });
        }

        public AnimatorCondition[] ToArray()
        {
            return _conditions.Select(condition => condition.ToAnimatorCondition()).ToArray();
        }

        public IEnumerable<ConditionBuilder> Inverse()
        {
            foreach (var condition in _conditions)
            {
                var newCondition = new ConditionBuilder();
                switch (condition.Mode)
                {
                    case AnimatorConditionMode.If:
                        newCondition.AddCondition(condition.ParameterType, AnimatorConditionMode.IfNot, condition.Parameter, condition.Threshold);
                        break;
                    case AnimatorConditionMode.IfNot:
                        newCondition.AddCondition(condition.ParameterType, AnimatorConditionMode.If, condition.Parameter, condition.Threshold);
                        break;
                    case AnimatorConditionMode.Greater:
                        newCondition.AddCondition(condition.ParameterType, AnimatorConditionMode.Less, condition.Parameter, condition.ParameterType == AnimatorControllerParameterType.Int ? condition.Threshold + 1 : condition.Threshold);
                        break;
                    case AnimatorConditionMode.Less:
                        newCondition.AddCondition(condition.ParameterType, AnimatorConditionMode.Greater, condition.Parameter, condition.ParameterType == AnimatorControllerParameterType.Int ? condition.Threshold - 1 : condition.Threshold);
                        break;
                    case AnimatorConditionMode.Equals:
                        newCondition.AddCondition(condition.ParameterType, AnimatorConditionMode.NotEqual, condition.Parameter, condition.Threshold);
                        break;
                    case AnimatorConditionMode.NotEqual:
                        newCondition.AddCondition(condition.ParameterType, AnimatorConditionMode.Equals, condition.Parameter, condition.Threshold);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                yield return newCondition;
            }
        }

        struct ConditionElement
        {
            public AnimatorControllerParameterType ParameterType; 
            public AnimatorConditionMode Mode;
            public string Parameter;
            public float Threshold;

            public AnimatorCondition ToAnimatorCondition()
            {
                return new AnimatorCondition
                {
                    mode = Mode,
                    parameter = Parameter,
                    threshold = Threshold
                };
            }
        }
    }
}