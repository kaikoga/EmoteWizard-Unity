using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.Extensions
{
    public static class AnimatorLayerBuilderExtensions
    {
        public static T AddStateMachineBehaviour2Access<T>(this AnimatorState state, Type type, AnimatorLayerBuilder builder, Func<StateMachineBehaviour, T> toAccess)
            => toAccess(state.AddStateMachineBehaviour2(type, builder));

        public static StateMachineBehaviour AddStateMachineBehaviour2(this AnimatorState state, Type type, AnimatorLayerBuilder builder)
        {
            // XXX AnimatorState.AddStateMachineBehaviour shim to work with states in controllers not persisted on disk yet
            StateMachineBehaviour stateMachineBehaviour;
            if (builder.IsPersistedAsset)
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