using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class ParameterItemTemplate : IEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly ParameterItem _parameterItem;

        public ParameterItemTemplate(EmoteTemplatePath path, ParameterItem parameterItem)
        {
            _path = path;
            _parameterItem = parameterItem;
        }

        public IEnumerable<ParameterItem> ToParameterItems()
        {
            yield return _parameterItem;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<ParameterSource>(target);
            source.parameterItem = _parameterItem;
        }
    }
}
