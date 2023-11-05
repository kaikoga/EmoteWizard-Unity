using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Base
{
    public interface IEmoteWizardContext
    {
        T GetWizard<T>() where T : EmoteWizardBase;

        void DisconnectAllOutputAssets();

        string GeneratedAssetPath(string relativePath);

        IEnumerable<EmoteItem> CollectAllEmoteItems();
    }
}