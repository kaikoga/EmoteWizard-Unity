using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        Dictionary<EmoteSequenceSourceBase, EmoteSequence> _emoteSequences = new Dictionary<EmoteSequenceSourceBase, EmoteSequence>();

        List<EmoteItem> _emoteItems;

        IEnumerable<EmoteItem> CollectAllEmoteItems()
        {
            return GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.ToEmoteItems(this));
        }

        public IEnumerable<EmoteItem> AllEmoteItems()
        {
            return _emoteItems = _emoteItems ?? CollectAllEmoteItems().ToList();
        }

        public IEnumerable<EmoteItem> EmoteItems(LayerKind layerKind)
        {
            return AllEmoteItems().Where(item => item.Sequence.layerKind == layerKind);
        }

        public EmoteSequence EmoteSequence(EmoteSequenceSourceBase source)
        {
            if (!_emoteSequences.TryGetValue(source, out var sequence))
            {
                sequence = source.ToEmoteSequence();
                _emoteSequences.Add(source, sequence);
            }
            return sequence;
        }
    }
}