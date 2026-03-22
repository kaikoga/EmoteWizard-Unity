using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizard.Wizards.Defaults;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class DefaultEmoteItemTemplate : ICompositeEmoteTemplate
    {
        public EmoteTemplatePath Path { get; }

        readonly EmoteItemKind _emoteItemKind;
        readonly EmoteSequenceFactoryKind _emoteSequenceFactoryKind;
        readonly LayerKind _layerKind;
        readonly HandSign _handSign;

        public DefaultEmoteItemTemplate(EmoteTemplatePath path,
            EmoteItemKind emoteItemKind,
            EmoteSequenceFactoryKind emoteSequenceFactoryKind,
            LayerKind layerKind,
            HandSign handSign
        )
        {
            Path = path;
            _emoteItemKind = emoteItemKind;
            _emoteSequenceFactoryKind = emoteSequenceFactoryKind;
            _layerKind = layerKind;
            _handSign = handSign;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<DefaultEmoteItemSource>(target);
            source.emoteItemKind = _emoteItemKind;
            source.emoteSequenceFactoryKind = _emoteSequenceFactoryKind;
            source.layerKind = _layerKind;
            source.handSign = _handSign;
        }

        public IEnumerable<IEmoteTemplate> Unpack(IPlatformFeatures platformFeatures)
        {
            yield return DefaultEmoteItem.UnpackDefaultHandSign(_emoteItemKind, _emoteSequenceFactoryKind, platformFeatures, _layerKind, _handSign);
        }
    }
}
