using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ConditionBuilders
{
    public class ConditionBuilder
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

        public static ConditionBuilder IfCondition(string paramName, bool value) => new ConditionBuilder().If(paramName, value);
        public static ConditionBuilder NotEqualCondition(string paramName, int value) => new ConditionBuilder().NotEqual(paramName, value);
        public static ConditionBuilder EqualsCondition(string paramName, int value) => new ConditionBuilder().Equals(paramName, value);
        public static ConditionBuilder LessCondition(string paramName, float value) => new ConditionBuilder().Less(paramName, value);
        public static ConditionBuilder ApproximateCondition(string paramName, float value) => new ConditionBuilder().Approximate(paramName, value);
        public static ConditionBuilder InclusiveRangeCondition(string paramName, int? min, int? max) => new ConditionBuilder().InclusiveRange(paramName, min, max);
        public static ConditionBuilder InclusiveRangeCondition(string paramName, float? min, float? max) => new ConditionBuilder().InclusiveRange(paramName, min, max);
        public static ConditionBuilder ExclusiveRangeCondition(string paramName, int? min, int? max) => new ConditionBuilder().ExclusiveRange(paramName, min, max);
        public static ConditionBuilder ExclusiveRangeCondition(string paramName, float? min, float? max) => new ConditionBuilder().ExclusiveRange(paramName, min, max);

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