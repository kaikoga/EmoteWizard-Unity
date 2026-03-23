using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Parameter Source", 2000)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/parameter_source")]
    public class ParameterSource : EmoteWizardDataSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public ParameterItem parameterItem = new ParameterItem();

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new ParameterItemTemplate(SelfPath, parameterItem);
        }
    }
}