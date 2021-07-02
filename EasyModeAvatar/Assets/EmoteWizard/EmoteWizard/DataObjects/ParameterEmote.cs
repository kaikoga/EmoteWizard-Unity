using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterEmote
    {
        [SerializeField] public string name;
        [SerializeField] public string parameter;
        [SerializeField] public List<ParameterEmoteState> states;
    }
}