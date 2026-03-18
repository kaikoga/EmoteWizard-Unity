using System;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Extensions
{
    public static class AnimatorStateExtension
    {
        public static T AddStateMachineBehaviour2Access<T>(this AnimatorState state, Type type, Func<StateMachineBehaviour, T> toAccess)
            => toAccess(state.AddStateMachineBehaviour2(type));
    }
}