using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class DefaultEmoteItemTemplate : ICompositeEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

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
            _path = path;
            _emoteItemKind = emoteItemKind;
            _emoteSequenceFactoryKind = emoteSequenceFactoryKind;
            _layerKind = layerKind;
            _handSign = handSign;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<DefaultEmoteItemSource>(target);
            source.emoteItemKind = _emoteItemKind;
            source.emoteSequenceFactoryKind = _emoteSequenceFactoryKind;
            source.layerKind = _layerKind;
            source.handSign = _handSign;
        }

        IEnumerable<IEmoteTemplate> ICompositeEmoteTemplate.Unpack(IPlatformFeatures platformFeatures)
        {
            return platformFeatures.UnpackDefaultHandSign(_path, _emoteItemKind, _emoteSequenceFactoryKind, _layerKind, _handSign);
        }
    }
}
