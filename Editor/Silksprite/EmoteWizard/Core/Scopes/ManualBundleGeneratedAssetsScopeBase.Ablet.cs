#if EW_ABLET_SUPPORT

using System.Linq;
using Ablet.Building;
using Ablet.Building.Ephemeral;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;

namespace Silksprite.EmoteWizard.Scopes
{
    public partial class ManualBundleGeneratedAssetsScopeBase
    {
        class AbletBackend : IBackend
        {
            readonly EmoteWizardEnvironment _environment;

            public AbletBackend(EmoteWizardEnvironment environment)
            {
                _environment = environment;
            }

            void IBackend.OnDispose(ManualBundleGeneratedAssetsScopeBase scope)
            {
                var generatedAssets = scope.CollectVolatileAssets(_environment)
                    .SelectMany(AssetPersister.IterateAssetObjectReferences)
                    .Distinct()
                    .Where(obj => !EditorUtility.IsPersistent(obj));

                var assetPersister = new AssetPersisterState(_environment.AvatarRoot.gameObject, true);
                assetPersister.PersistAssets(generatedAssets);
            }
        }
    }
}
#endif
