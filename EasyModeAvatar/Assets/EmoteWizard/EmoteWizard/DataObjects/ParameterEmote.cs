using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterEmote
    {
        [SerializeField] public bool enabled = true;
        [SerializeField] public string name;
        [SerializeField] public string parameter;
        [SerializeField] public ParameterValueKind valueKind;
        [SerializeField] public List<ParameterEmoteState> states = new List<ParameterEmoteState>();

        public VRCExpressionParameters.ValueType VrcValueType
        {
            get
            {
                switch (valueKind)
                {
                    case ParameterValueKind.Bool:
                        return VRCExpressionParameters.ValueType.Bool;
                        break;
                    case ParameterValueKind.Int:
                        return VRCExpressionParameters.ValueType.Int;
                        break;
                    case ParameterValueKind.Float:
                        return VRCExpressionParameters.ValueType.Float;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void CollectStates(ParameterItem parameterItem)
        {
            var oldStates = states;
            states = Enumerable.Empty<ParameterEmoteState>()
                .Concat(parameterItem.states.Select(state => new ParameterEmoteState
                {
                    value = state.value,
                    clip = states.FirstOrDefault(s => s.value == state.value)?.clip
                })).Concat(oldStates.Where(oldState => parameterItem.states.All(state => state.value != oldState.value)))
                .ToList();
        }
    }
}