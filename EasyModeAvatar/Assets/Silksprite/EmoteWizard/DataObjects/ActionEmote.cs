using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ActionEmote
    {
        [SerializeField] public string name;
        [SerializeField] public int emoteIndex;

        internal static string NameForDefaultEmote(int value)
        {
            switch (value)
            {
                case 1: return "Wave";
                case 2: return "Clap";
                case 3: return "Point";
                case 4: return "Cheer";
                case 5: return "Dance";
                case 6: return "Backflip";
                case 7: return "SadKick";
                case 8: return "Die";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}