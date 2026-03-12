using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;

namespace Silksprite.EmoteWizard.Contexts.Ephemeral
{
    [UsedImplicitly]
    public class EmoteItemContext : ContextBase
    {
        public EmoteItemContext(EmoteWizardEnvironment env) : base(env) { }
        
        List<EmoteItem> _mirroredEmoteItems;
        List<EmoteItem> _forceMirroredEmoteItems;

        IEnumerable<EmoteItem> CollectAllMirroredEmoteItems()
        {
            return Environment.GetComponentsInChildren<IEmoteItemSource>(true)
                .SelectMany(source => source.ToEmoteItems(Environment))
                .SelectMany(item => item.IsMirrorItem ? item.Mirror() : item.NoMirror());
        }

        IEnumerable<EmoteItem> CollectAllForceMirroredEmoteItems()
        {
            // note: try to reuse mirroredEmoteItems as possible
            return AllMirroredEmoteItems().GroupBy(item => item.GroupNameNoMirror)
                .SelectMany(group => group.All(item => item.Hand is EmoteHand.Neither)
                    ? group
                    : group.SelectMany(item => item.Mirror()));
        }

        // Mirrors Mirror Parameters per EmoteItem
        public IEnumerable<EmoteItem> AllMirroredEmoteItems()
        {
            return _mirroredEmoteItems = _mirroredEmoteItems ?? CollectAllMirroredEmoteItems().ToList();
        }

        // Mirrors Mirror Parameters per Group
        public IEnumerable<EmoteItem> AllForceMirroredEmoteItems()
        {
            return _forceMirroredEmoteItems = _forceMirroredEmoteItems ?? CollectAllForceMirroredEmoteItems().ToList();
        }

        public IEnumerable<EmoteItem> ForceMirroredEmoteItems(LayerKind layerKind)
        {
            return AllForceMirroredEmoteItems().Where(item => item.LayerKind == layerKind);
        }
    }
}