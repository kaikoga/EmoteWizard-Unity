using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Templates;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [AddComponentMenu("Emote Wizard/Wizards/Default Items Wizard", 900)]
    public class DefaultSourcesWizard : EmoteWizardBase
    {
        [SerializeField] public LayerKind layerKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            switch (layerKind)
            {
                case LayerKind.None:
                    break;
                case LayerKind.FX:
                case LayerKind.Gesture:
                    switch (emoteSequenceFactoryKind)
                    {
                        case EmoteSequenceFactoryKind.EmoteSequence:
                            return DefaultEmoteItems.EnumerateDefaultHandSigns(layerKind);
                        case EmoteSequenceFactoryKind.GenericEmoteSequence:
                            return DefaultEmoteItems.EnumerateGenericDefaultHandSigns(layerKind);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case LayerKind.Action:
                    // force EmoteSequence
                    return DefaultActionEmote.EnumerateDefaultActionEmoteItems();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Enumerable.Empty<IEmoteTemplate>();
        }
    }
}