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
        [SerializeField] public VRCExpressionParameters.ValueType valueType;
        [SerializeField] public List<ParameterEmoteState> states = new List<ParameterEmoteState>();

        public void CollectStates(ParameterItem parameterItem)
        {
            var oldStates = states;
            states = parameterItem.states.Select(state => new ParameterEmoteState
            {
                value = state.value,
                clip = states.FirstOrDefault(s => s.value == state.value)?.clip
            }).ToList();
        }
    }
}