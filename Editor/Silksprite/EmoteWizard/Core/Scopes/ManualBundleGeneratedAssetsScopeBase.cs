using System;
using System.Collections.Generic;
using nadena.dev.ndmf;
using nadena.dev.ndmf.util;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Scopes
{
    public abstract class ManualBundleGeneratedAssetsScopeBase : IDisposable
    {
        readonly EmoteWizardEnvironment _environment;
        readonly GameObject _gameObject;
        readonly BuildContext _buildContext;

        protected ManualBundleGeneratedAssetsScopeBase(EmoteWizardEnvironment environment, bool manualBuild)
        {
            _environment = environment;
            if (manualBuild && !environment.PersistGeneratedAssets)
            {
                _gameObject = new GameObject("Temporary");
                _buildContext = new BuildContext(_gameObject, "Assets/ZZZ_GeneratedAssets/__EmoteWizard");
            }
        }

        void IDisposable.Dispose()
        {
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
        }

        protected abstract IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment);
    }
}