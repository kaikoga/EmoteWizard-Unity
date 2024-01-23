using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;

namespace Silksprite.EmoteWizard.Contexts.Ephemeral
{
    [UsedImplicitly]
    public class EmoteItemContext : ContextBase
    {
        public EmoteItemContext(EmoteWizardEnvironment env) : base(env) { }
        
        List<EmoteItem> _emoteItems;
        List<EmoteItem> _mirroredEmoteItems;
        List<EmoteItem> _forceMirroredEmoteItems;

        IEnumerable<EmoteItem> CollectAllEmoteItems()
        {
            return Environment.GetComponentsInChildren<IEmoteItemSource>(true).SelectMany(source => source.ToEmoteItems());
        }

        IEnumerable<EmoteItem> CollectAllMirroredEmoteItems()
        {
            return AllEmoteItems().SelectMany(item => item.Mirror(false));
        }

        IEnumerable<EmoteItem> CollectAllForceMirroredEmoteItems()
        {
            return AllEmoteItems().GroupBy(item => item.GroupName)
                .SelectMany(group =>
                {
                    return group.Any(item => item.IsMirrorItem) ? group.SelectMany(item => item.Mirror(true)) : group;
                });
        }

        public IEnumerable<EmoteItem> AllEmoteItems()
        {
            return _emoteItems = _emoteItems ?? CollectAllEmoteItems().ToList();
        }

        public IEnumerable<EmoteItem> AllMirroredEmoteItems()
        {
            return _mirroredEmoteItems = _mirroredEmoteItems ?? CollectAllMirroredEmoteItems().ToList();
        }

        IEnumerable<EmoteItem> AllForceMirroredEmoteItems()
        {
            return _forceMirroredEmoteItems = _forceMirroredEmoteItems ?? CollectAllForceMirroredEmoteItems().ToList();;
        }

        public IEnumerable<EmoteItem> ForceMirroredEmoteItems(LayerKind layerKind)
        {
            return AllForceMirroredEmoteItems().Where(item => item.LayerKind == layerKind);
        }
    }
}