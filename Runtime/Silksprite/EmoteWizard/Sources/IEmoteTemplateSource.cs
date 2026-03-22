using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Templates;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IEmoteTemplateSource
    {
        IEmoteTemplate ToEmoteTemplate(EmoteWizardEnvironment environment);
    }
}