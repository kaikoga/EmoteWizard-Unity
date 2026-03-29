#if EW_VRCSDK3_AVATARS

using System.Collections.Generic;
using System.Linq;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;

namespace Silksprite.EmoteWizard.Ndmf.Introspection
{
    [ParameterProviderFor(typeof(EmoteWizardRoot))]
    public class EmoteWizardParameterProvider : IParameterProvider
    {
        readonly EmoteWizardRoot _root;

        public EmoteWizardParameterProvider(EmoteWizardRoot root)
        {
            _root = root;
        }

        public IEnumerable<ProvidedParameter> GetSuppliedParameters(BuildContext? context = null)
        {
            return _root.ToEnv()
                .GetContext<ParametersContext>().Snapshot().ParameterItems
                .Select(parameterItem => new ProvidedParameter(parameterItem.Name, ParameterNamespace.Animator, _root, EmoteWizardPlugin.Instance, parameterItem.GetParameterType())
                {
                    WantSynced = parameterItem.Synced
                });
        }
    }
}

#endif