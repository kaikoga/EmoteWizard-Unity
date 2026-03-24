using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Object = UnityEngine.Object;

#if EW_NDMF_SUPPORT && EW_ABLET_SUPPORT
using Ablet.API;
#endif

namespace Silksprite.EmoteWizard.Scopes
{
    public abstract partial class ManualBundleGeneratedAssetsScopeBase : IDisposable
    {
        interface IBackend
        {
            void OnDispose(ManualBundleGeneratedAssetsScopeBase scope);
        }

        readonly IBackend? _backend;

        protected ManualBundleGeneratedAssetsScopeBase(EmoteWizardEnvironment environment, bool manualBuild)
        {
            if (manualBuild && !environment.PersistGeneratedAssets)
            {
#if EW_NDMF_SUPPORT && EW_ABLET_SUPPORT
                _backend = AbletSymbols.PreferAblet ? (IBackend)new AbletBackend(environment) : new NdmfBackend(environment);
#elif EW_NDMF_SUPPORT
                _backend = new NdmfBackend(environment);
#elif EW_ABLET_SUPPORT
                _backend = new AbletBackend(environment);
#else
                throw new InvalidOperationException("");
#endif
            }
            _backend = null;
        }

        void IDisposable.Dispose()
        {
            _backend?.OnDispose(this);
        }

        protected abstract IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment);
    }
}