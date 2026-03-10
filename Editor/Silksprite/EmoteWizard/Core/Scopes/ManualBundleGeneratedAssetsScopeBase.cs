using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.Building.Ephemeral;
using Silksprite.EmoteWizard.Contexts;
using Object = UnityEngine.Object;

#if EW_NDMF_SUPPORT || EW_ABLET_SUPPORT
using UnityEditor;
using UnityEngine;
#endif

#if EW_NDMF_SUPPORT
using nadena.dev.ndmf;
using nadena.dev.ndmf.util;
#elif EW_ABLET_SUPPORT
using Ablet.Building;
#endif

namespace Silksprite.EmoteWizard.Scopes
{
    public abstract class ManualBundleGeneratedAssetsScopeBase : IDisposable
    {
        readonly EmoteWizardEnvironment _environment;
#if EW_NDMF_SUPPORT
        readonly GameObject _gameObject;
        readonly BuildContext _buildContext;
#endif
        protected ManualBundleGeneratedAssetsScopeBase(EmoteWizardEnvironment environment, bool manualBuild)
        {
            _environment = environment;
            if (manualBuild && !environment.PersistGeneratedAssets)
            {
#if EW_NDMF_SUPPORT
                _gameObject = new GameObject("Temporary");
                _buildContext = new BuildContext(_gameObject, "Assets/ZZZ_GeneratedAssets/__EmoteWizard");
#elif EW_ABLET_SUPPORT
#else
                throw new InvalidOperationException("");
#endif
            }
        }

        void IDisposable.Dispose()
        {
#if EW_NDMF_SUPPORT
            if (_buildContext == null) return;

            foreach (var volatileAsset in CollectVolatileAssets(_environment))
            {
                // we are sure these are not prefabs
                foreach (var asset in volatileAsset.ReferencedAssets(traverseSaved: false, includeScene: true))
                {
                    AssetDatabase.AddObjectToAsset(asset, _buildContext.AssetContainer);
                    asset.hideFlags = HideFlags.None; // match Modular Avatar behavior 
                }
            }

            AssetDatabase.SaveAssets();
            Object.DestroyImmediate(_gameObject);
#elif EW_ABLET_SUPPORT
            var generatedAssets = CollectVolatileAssets(_environment)
                .SelectMany(AssetPersister.IterateAssetObjectReferences)
                .Distinct()
                .Where(obj => !EditorUtility.IsPersistent(obj));
            
            var assetPersister = new AssetPersisterState(_environment.AvatarRoot.gameObject, true);
            assetPersister.PersistAssets(generatedAssets);
#endif
        }

        protected abstract IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment);
    }
}