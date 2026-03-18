using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Extensions
{
    public static class AnimatorStateExtension
    {
        public static T AddStateMachineBehaviour2<T>(this AnimatorState state, bool isPersistedAsset)
            where T : StateMachineBehaviour
        {
            return (T)state.AddStateMachineBehaviour2(typeof(T), isPersistedAsset);
        }

        [UsedImplicitly]
        public static StateMachineBehaviour AddStateMachineBehaviour2(this AnimatorState state, Type type, bool isPersistedAsset)
        {
            // XXX AnimatorState.AddStateMachineBehaviour shim to work with states in controllers not persisted on disk yet
            StateMachineBehaviour stateMachineBehaviour;
            if (isPersistedAsset)
            {
                stateMachineBehaviour = state.AddStateMachineBehaviour(type);
            }
            else
            {
                // NOTE: volatile assets should be collected by ndmf BuildContext
                var t = (StateMachineBehaviour)ScriptableObject.CreateInstance(type);
                var behaviours = state.behaviours;
                ArrayUtility.Add(ref behaviours, t);
                state.behaviours = behaviours;
                return t;
            }
            return stateMachineBehaviour;
        }
    }
}