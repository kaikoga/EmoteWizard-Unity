using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Wizards.Defaults;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards
{
    [AddComponentMenu("Emote Wizard/Wizards/Default Sources Wizard", 900)]
    public class DefaultSourcesWizard : EmoteWizardBase
    {
        [SerializeField] public DefaultSourceKind defaultSourceKind;
        [SerializeField] public EmoteItemKind emoteItemKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            switch (defaultSourceKind)
            {
                case DefaultSourceKind.Fx:
                    return DefaultEmoteItem.EnumerateDefaultHandSigns(emoteItemKind, emoteSequenceFactoryKind, LayerKind.FX);
                case DefaultSourceKind.Gesture:
                    return DefaultEmoteItem.EnumerateDefaultHandSigns(emoteItemKind, emoteSequenceFactoryKind, LayerKind.Gesture);
                case DefaultSourceKind.Action:
                    // force EmoteSequence
                    return DefaultActionEmote.EnumerateDefaultActionEmoteItems();
                case DefaultSourceKind.Vrm:
                    // force Generic EmoteItem / EmoteSequence
                    return DefaultBlendShape.EnumerateDefaultBlendShapes();
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