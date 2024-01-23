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

        public override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            switch (layerKind)
            {
                case LayerKind.None:
                    break;
                case LayerKind.FX:
                    return DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.FX);
                case LayerKind.Gesture:
                    return DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.Gesture);
                case LayerKind.Action:
                    return DefaultActionEmote.EnumerateDefaultActionEmoteItems();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Enumerable.Empty<IEmoteTemplate>();
        }
    }
}