using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Extensions;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards.Defaults;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards
{
    [AddComponentMenu("Emote Wizard/Wizards/Default Sources Wizard", 900)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/wizards/default_sources_wizard")]
    public class DefaultSourcesWizard : EmoteWizardBase
    {
        [SerializeField] public DefaultSourceKind defaultSourceKind;
        [SerializeField] public EmoteItemKind emoteItemKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        [SerializeField] public bool unpack = true;

        IEnumerable<IEmoteTemplate> EnumerateDefaultHandSignSources(EmoteTemplatePath path, LayerKind layerKind)
        {
            return Enum.GetValues(typeof(HandSign)).OfType<HandSign>()
                .Select(handSign => new DefaultEmoteItemTemplate(path.Join($"{handSign}"), emoteItemKind, emoteSequenceFactoryKind, layerKind, handSign));
        }

        static IEnumerable<DefaultActionEmoteItemTemplate> EnumerateDefaultActionSources(IPlatformFeatures platformFeatures, EmoteTemplatePath path)
        {
            return platformFeatures.DefaultActionIndexes()
                .Select(index => new DefaultActionEmoteItemTemplate(path.Join(index.Name()), index));
        }

        static IEnumerable<IEmoteTemplate> EnumerateDefaultBlendShapeSources(EmoteTemplatePath path)
        {
            return DefaultBlendShapes.EnumerateDefaultBlendShapes(path);
        }

        protected override IEnumerable<IEmoteTemplate> SourceTemplates(EmoteWizardEnvironment environment)
        {
            var platformFeatures = environment.GetPlatformFeatures();
            var path = EmoteTemplatePath.Context(environment, this);

            var defaultSources = defaultSourceKind switch
            {
                DefaultSourceKind.Fx =>
                    EnumerateDefaultHandSignSources(path, LayerKind.FX),
                DefaultSourceKind.Gesture =>
                    EnumerateDefaultHandSignSources(path, LayerKind.Gesture),
                DefaultSourceKind.Action =>
                    // force Non-Generic EmoteItem / EmoteSequence
                    EnumerateDefaultActionSources(platformFeatures, path),
                DefaultSourceKind.Vrm =>
                    // force Generic EmoteItem / EmoteSequence
                    EnumerateDefaultBlendShapeSources(path),
                _ => Enumerable.Empty<IEmoteTemplate>()
            };
            return unpack
                ? defaultSources.SelectMany(MaybeUnpack)
                : defaultSources;

            IEnumerable<IEmoteTemplate> MaybeUnpack(IEmoteTemplate template)
            {
                if (template is ICompositeEmoteTemplate composite)
                {
                    foreach (var item in composite.Unpack(platformFeatures)) yield return item; 
                }
                else
                {
                    yield return template;
                }
            }
        }
    }

    public enum DefaultSourceKind
    {
        [InspectorName("FX")]
        Fx,
        Gesture,
        Action,
        [InspectorName("VRM 0+1")]
        Vrm
    }
}