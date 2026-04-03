#if EW_ABLET_SUPPORT

using System;
using System.Linq;
using Ablet.Building;
using Ablet.Building.Ephemeral;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;

namespace Silksprite.EmoteWizard.Scopes
{
    public partial class ManualBundleGeneratedAssetsScopeBase
    {
        class AbletBackend : IDisposable
        {
            readonly ManualBundleGeneratedAssetsScopeBase _scope;
            readonly EmoteWizardEnvironment _environment;

            public AbletBackend(ManualBundleGeneratedAssetsScopeBase scope, EmoteWizardEnvironment environment)
            {
                _scope = scope;
                _environment = environment;
            }

            void IDisposable.Dispose()
            {
                var generatedAssets = _scope.CollectVolatileAssets(_environment)
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
