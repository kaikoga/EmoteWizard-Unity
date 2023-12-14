using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        List<EmoteItem> _emoteItems;

        IEnumerable<EmoteItem> CollectAllEmoteItems()
        {
            return GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.ToEmoteItems());
        }

        public IEnumerable<EmoteItem> AllEmoteItems()
        {
            return _emoteItems = _emoteItems ?? CollectAllEmoteItems().ToList();
        }

        public IEnumerable<EmoteItem> EmoteItems(LayerKind layerKind)
        {
            return AllEmoteItems().Where(item => item.Sequence.layerKind == layerKind);
        }
    }
}