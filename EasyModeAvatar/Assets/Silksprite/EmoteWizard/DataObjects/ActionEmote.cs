using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ActionEmote
    {
        [SerializeField] public string name;
        [SerializeField] public int emoteIndex;
    }
}