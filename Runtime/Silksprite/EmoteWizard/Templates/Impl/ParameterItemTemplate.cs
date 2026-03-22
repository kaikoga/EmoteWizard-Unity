using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class ParameterItemTemplate : IEmoteTemplate
    {
        public EmoteTemplatePath Path { get; }

        public readonly ParameterItem ParameterItem;

        public ParameterItemTemplate(EmoteTemplatePath path, ParameterItem parameterItem)
        {
            Path = path;
            ParameterItem = parameterItem;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<ParameterSource>(target);
            source.parameterItem = ParameterItem;
        }
    }
}
