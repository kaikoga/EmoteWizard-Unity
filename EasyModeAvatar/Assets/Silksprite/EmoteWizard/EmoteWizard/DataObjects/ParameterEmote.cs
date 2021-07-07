using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterEmote
    {
        [SerializeField] public bool enabled = true;
        [SerializeField] public string name;
        [SerializeField] public string parameter;
        [SerializeField] public ParameterEmoteKind emoteKind = ParameterEmoteKind.Transition;
        [SerializeField] public ParameterValueKind valueKind;
        [SerializeField] public List<ParameterEmoteState> states = new List<ParameterEmoteState>();

        public void CollectStates(ParameterItem parameterItem)
        {
            var oldStates = states;
            states = Enumerable.Empty<ParameterEmoteState>()
                .Concat(parameterItem.usages.Select(state => new ParameterEmoteState
                {
                    value = state.value,
                    clip = states.FirstOrDefault(s => s.value == state.value)?.clip
                })).Concat(oldStates.Where(oldState => parameterItem.usages.All(state => state.value != oldState.value)))
                .ToList();
        }
    }
}