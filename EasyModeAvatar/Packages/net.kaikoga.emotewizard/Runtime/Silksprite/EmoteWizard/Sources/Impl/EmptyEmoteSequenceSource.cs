using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmptyEmoteSequenceSource : EmoteSequenceSourceBase
    {
        [SerializeField] public LayerKind layerKind = LayerKind.FX;
        [SerializeField] public string groupName;

        public override bool LooksLikeMirrorItem => false;

        public override EmoteSequence ToEmoteSequence(EmoteWizardEnvironment emoteWizardEnvironment) => new EmoteSequence
        {
            layerKind = layerKind,
            groupName = groupName
        };
    }
}