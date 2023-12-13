using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class EditorLayerBuilder : LayerBuilderBase
    {
        readonly IEnumerable<Motion> _motions;

        public EditorLayerBuilder(AnimatorLayerBuilder builder, AnimatorControllerLayer layer, IEnumerable<Motion> motions) : base(builder, layer)
        {
            _motions = motions.ToArray();
        }

        protected override void Process()
        {
            foreach (var motion in _motions)
            {
                AddStateWithoutTransition(motion.name, motion);
                NextStateRow();
            }
        }
    }
}