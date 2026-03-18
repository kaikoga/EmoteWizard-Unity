using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Extensions
{
    public static class AnimatorStateExtension
    {
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