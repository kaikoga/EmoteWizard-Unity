using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Templates;
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

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            var environment = CreateEnv();
            var platformFeatures = environment.GetPlatformFeatures();
            var path = EmoteTemplatePath.Context(environment, this);
            
            switch (defaultSourceKind)
            {
                case DefaultSourceKind.Fx:
                    return DefaultEmoteItem.EnumerateDefaultHandSigns(path, emoteItemKind, emoteSequenceFactoryKind, platformFeatures, LayerKind.FX, unpack);
                case DefaultSourceKind.Gesture:
                    return DefaultEmoteItem.EnumerateDefaultHandSigns(path, emoteItemKind, emoteSequenceFactoryKind, platformFeatures, LayerKind.Gesture, unpack);
                case DefaultSourceKind.Action:
                    // force Non-Generic EmoteItem / EmoteSequence
                    return DefaultActionEmote.EnumerateDefaultActionEmoteItems(path, unpack);
                case DefaultSourceKind.Vrm:
                    // force Generic EmoteItem / EmoteSequence
                    return DefaultBlendShape.EnumerateDefaultBlendShapes(path);
            }
            return Enumerable.Empty<IEmoteTemplate>();
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