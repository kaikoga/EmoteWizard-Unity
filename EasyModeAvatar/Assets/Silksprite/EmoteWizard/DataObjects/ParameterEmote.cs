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
        [SerializeField] public ParameterEmoteKind emoteKind = ParameterEmoteKind.Unused;
        [SerializeField] public ParameterValueKind valueKind;
        [SerializeField] public List<ParameterEmoteState> states = new List<ParameterEmoteState>();

        public void CollectStates(ParameterItem parameterItem)
        {
            var oldStates = states;
            states = Enumerable.Empty<ParameterEmoteState>()
                .Concat(parameterItem.usages.Select(state =>
                {
                    var oldState = states.FirstOrDefault(s => s.value == state.value);
                    return new ParameterEmoteState
                    {
                        enabled = oldState?.enabled ?? true,
                        value = state.value,
                        clip = oldState?.clip
                    };
                })).Concat(oldStates.Where(oldState => parameterItem.usages.All(state => state.value != oldState.value)))
                .ToList();
        }
    }
}