using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class ExpressionItemTemplate : IEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly ExpressionItem _expressionItem;

        public ExpressionItemTemplate(EmoteTemplatePath path, ExpressionItem expressionItem)
        {
            _path = path;
            _expressionItem = expressionItem;
        }

        public IEnumerable<ExpressionItem> ToExpressionItems()
        {
            yield return _expressionItem;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<ExpressionItemSource>(target);
            source.expressionItem = _expressionItem;
        }
    }
}
