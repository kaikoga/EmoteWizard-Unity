using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParameterEmoteExtension
    {
        public static IEnumerable<AnimationClip> AllClips(this ParameterEmote parameterEmote)
        {
            return parameterEmote.states.Select(state => state.clip);
        }
    }
}