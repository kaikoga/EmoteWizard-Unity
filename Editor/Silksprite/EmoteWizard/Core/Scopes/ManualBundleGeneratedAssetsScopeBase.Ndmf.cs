#if EW_NDMF_SUPPORT
using nadena.dev.ndmf;
using nadena.dev.ndmf.util;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Scopes
{
    public partial class ManualBundleGeneratedAssetsScopeBase
    {
        class NdmfBackend : IBackend
        {
            readonly EmoteWizardEnvironment _environment;
            readonly GameObject _gameObject;
            readonly BuildContext _buildContext;

            public NdmfBackend(EmoteWizardEnvironment environment)
            {
                _environment = environment;
                _gameObject = new GameObject("Temporary");
                _buildContext = new BuildContext(_gameObject, "Assets/ZZZ_GeneratedAssets/__EmoteWizard");
            }

            void IBackend.OnDispose(ManualBundleGeneratedAssetsScopeBase scope)
            {
                foreach (var volatileAsset in scope.CollectVolatileAssets(_environment))
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
        }
    }
}
#endif
