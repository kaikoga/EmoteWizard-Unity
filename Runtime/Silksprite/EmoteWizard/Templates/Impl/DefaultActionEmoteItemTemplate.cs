using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Wizards.Defaults;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class DefaultActionEmoteItemTemplate : ICompositeEmoteTemplate
    {
        public string Path { get; }

        readonly DefaultActionIndex _defaultActionIndex;

        public DefaultActionEmoteItemTemplate(string path, DefaultActionIndex defaultActionIndex)
        {
            Path = path;
            _defaultActionIndex = defaultActionIndex;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<DefaultActionEmoteItemSource>(target);
            source.defaultActionIndex = _defaultActionIndex;
        }

        public IEnumerable<IEmoteTemplate> Unpack(IPlatformFeatures platformFeatures)
        {
            yield return DefaultActionEmote.UnpackDefaultAction(_defaultActionIndex);
        }
    }
}
