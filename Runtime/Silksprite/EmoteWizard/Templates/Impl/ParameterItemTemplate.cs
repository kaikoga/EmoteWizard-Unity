using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class ParameterItemTemplate : IEmoteTemplate
    {
        public EmoteTemplatePath Path { get; }

        readonly ParameterItem _parameterItem;

        public ParameterItemTemplate(EmoteTemplatePath path, ParameterItem parameterItem)
        {
            Path = path;
            _parameterItem = parameterItem;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<ParameterSource>(target);
            source.parameterItem = _parameterItem;
        }
    }
}
