using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Internal.Extensions
{
    public static class AnimatorLayerBuilderExtensions
    {
        public static T AddStateMachineBehaviour2<T>(this AnimatorState state, bool isPersistedAsset)
            where T : StateMachineBehaviour
        {
            // XXX AnimatorState.AddStateMachineBehaviour shim to work with states in controllers not persisted on disk yet
            T stateMachineBehaviour;
            if (isPersistedAsset)
            {
                stateMachineBehaviour = state.AddStateMachineBehaviour<T>();
            }
            else
            {
                // NOTE: volatile assets should be collected by ndmf BuildContext
                var t = ScriptableObject.CreateInstance<T>();
                var behaviours = state.behaviours;
                ArrayUtility.Add(ref behaviours, t);
                state.behaviours = behaviours;
                return t;
            }
            return stateMachineBehaviour;
        }
    }
}