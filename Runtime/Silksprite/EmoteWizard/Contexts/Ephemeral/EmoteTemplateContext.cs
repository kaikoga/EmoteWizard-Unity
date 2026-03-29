using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Templates;

namespace Silksprite.EmoteWizard.Contexts.Ephemeral
{
    [UsedImplicitly]
    public class EmoteTemplateContext : ContextBase
    {
        List<IEmoteTemplate>? _emoteTemplates;
        List<IEmoteTemplate>? _unpackedTemplates;

        public EmoteTemplateContext(EmoteWizardEnvironment env) : base(env) { }
        
        IEnumerable<IEmoteTemplate> CollectAllEmoteTemplates()
        {
            return Environment.GetComponentsInChildren<IEmoteTemplateSource>(true)
                .Select(source => source.ToEmoteTemplate());
        }

        IEnumerable<IEmoteTemplate> UnpackCompletely()
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            IEnumerable<IEmoteTemplate> Selector(IEmoteTemplate emoteTemplate)
            {
                switch (emoteTemplate)
                {
                    case ICompositeEmoteTemplate composite:
                        yield return composite;
                        foreach (var item in composite.Unpack(platformFeatures).SelectMany(Selector))
                        {
                            yield return item;
                        }
                        break;
                    default:
                        yield return emoteTemplate;
                        break;
                }
            }
            return AllEmoteTemplates().SelectMany(Selector);
        }

        IEnumerable<IEmoteTemplate> AllEmoteTemplates()
        {
            return _emoteTemplates ??= CollectAllEmoteTemplates().ToList();
        }

        IEnumerable<IEmoteTemplate> AllUnpackedEmoteTemplates()
        {
            return _unpackedTemplates ??= UnpackCompletely().ToList();
        }

        public IEnumerable<T> UnpackedTemplates<T>() where T : IEmoteTemplate
        {
            return AllUnpackedEmoteTemplates().OfType<T>();
        }
    }
}