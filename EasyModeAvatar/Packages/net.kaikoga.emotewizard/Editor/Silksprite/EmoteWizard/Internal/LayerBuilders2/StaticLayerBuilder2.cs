using Silksprite.EmoteWizard.Internal.LayerBuilders2.Base;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders2
{
    public class StaticLayerBuilder2 : LayerBuilderBase2
    {
        readonly string _stateName;
        readonly Motion _clip;

        public StaticLayerBuilder2(AnimatorLayerBuilder builder, AnimatorControllerLayer layer, string stateName, Motion clip) : base(builder, layer)
        {
            _stateName = stateName;
            _clip = clip;
        }

        protected override void Process()
        {
            AddStateWithoutTransition(_stateName, _clip);
        }
    }
}