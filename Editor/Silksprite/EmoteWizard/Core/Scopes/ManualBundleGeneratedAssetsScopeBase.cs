using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Scopes
{
    public abstract partial class ManualBundleGeneratedAssetsScopeBase : IDisposable
    {
        readonly IDisposable? _backend;

        protected ManualBundleGeneratedAssetsScopeBase(EmoteWizardEnvironment environment, bool manualBuild)
        {
            if (manualBuild && !environment.PersistGeneratedAssets)
            {
#if EW_NDMF_SUPPORT && EW_ABLET_SUPPORT
                _backend = Ablet.API.AbletSymbols.PreferAblet
                    ? (IDisposable)new AbletBackend(this, environment)
                    : new NdmfBackend(this, environment);
#elif EW_NDMF_SUPPORT
                _backend = new NdmfBackend(this, environment);
#elif EW_ABLET_SUPPORT
                _backend = new AbletBackend(this, environment);
#else
                throw new InvalidOperationException("");
#endif
            }
            _backend = null;
        }

        void IDisposable.Dispose()
        {
            _backend?.Dispose();
        }

        protected abstract IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment);
    }
}