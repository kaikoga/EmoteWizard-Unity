using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class ExpressionItemTemplate : IEmoteTemplate
    {
        public string Path { get; }
        readonly ExpressionItem _expressionItem;

        public ExpressionItemTemplate(string path, ExpressionItem expressionItem)
        {
            Path = path;
            _expressionItem = expressionItem;
        }

        public IEnumerable<EmoteItem> ToEmoteItems() => Array.Empty<EmoteItem>();

        public IEnumerable<ExpressionItem> ToExpressionItems()
        {
            yield return _expressionItem;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<ExpressionItemSource>(target);
            source.expressionItem = _expressionItem;
        }
    }
}
