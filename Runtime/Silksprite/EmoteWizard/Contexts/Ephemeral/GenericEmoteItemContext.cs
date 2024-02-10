using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;

namespace Silksprite.EmoteWizard.Contexts.Ephemeral
{
    [UsedImplicitly]
    public class GenericEmoteItemContext : ContextBase
    {
        public GenericEmoteItemContext(EmoteWizardEnvironment env) : base(env) { }
        
        List<GenericEmoteItem> _genericEmoteItems;

        IEnumerable<GenericEmoteItem> CollectAllGenericEmoteItems()
        {
            return Environment.GetComponentsInChildren<IGenericEmoteItemSource>(true).SelectMany(source => source.ToGenericEmoteItems());
        }
        
        public IEnumerable<GenericEmoteItem> AllGenericEmoteItems()
        {
            return _genericEmoteItems = _genericEmoteItems ?? CollectAllGenericEmoteItems().ToList();
        }
    }
}