using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
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
    }
}