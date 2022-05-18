using Silksprite.EmoteWizard.Extensions;
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
            var defaultTransition = AddStateAsTransition(stateName, clip);
            defaultTransition.AddAlwaysTrueCondition();
            LegacyStateMachine.defaultState = defaultTransition.destinationState;
        }
    }
}