using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class StaticLayerBuilder : LayerBuilderBase
    {
        public StaticLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer) : base(builder, layer) { }

        public void Build(string stateName, Motion clip)
        {
            StateMachine.defaultState = AddStateAsTransition(stateName, clip).destinationState;
        }
    }
}