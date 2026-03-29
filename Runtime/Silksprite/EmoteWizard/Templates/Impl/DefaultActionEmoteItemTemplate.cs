using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class DefaultActionEmoteItemTemplate : ICompositeEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly DefaultActionIndex _defaultActionIndex;

        public DefaultActionEmoteItemTemplate(EmoteTemplatePath path, DefaultActionIndex defaultActionIndex)
        {
            _path = path;
            _defaultActionIndex = defaultActionIndex;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<DefaultActionEmoteItemSource>(target);
            source.defaultActionIndex = _defaultActionIndex;
        }

        IEnumerable<IEmoteTemplate> ICompositeEmoteTemplate.Unpack(IPlatformFeatures platformFeatures)
        {
            return platformFeatures.UnpackDefaultAction(_path, _defaultActionIndex);
        }
    }
}
