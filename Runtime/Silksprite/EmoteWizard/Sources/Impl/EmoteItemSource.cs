using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Emote Item Source", 0)]
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource
    {
        [SerializeField] public EmoteTrigger trigger;

        [Header("Sequence")]
        [SerializeField] public EmoteSequenceSourceBase sequence;

        [Header("Expression Item")]
        [SerializeField] public bool hasExpressionItem;
        [SerializeField] public string expressionItemPath;
        [SerializeField] public Texture2D expressionItemIcon;

        public EmoteSequenceSourceBase FindEmoteSequenceSource()
        {
            if (sequence) return sequence;

            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .FirstOrDefault();
        }

        IEmoteFactory FindEmoteFactory()
        {
            var source = FindEmoteSequenceSource();
            if (!source) return null;

            return source.ToEmoteFactory();
        }

        EmoteItemTemplate ToTemplate()
        {
            // TODO cache me?
            return new EmoteItemTemplate(trigger, FindEmoteFactory()?.ToTemplate(), hasExpressionItem, expressionItemPath, expressionItemIcon);
        }

        public bool LooksLikeMirrorItem => ToTemplate().LooksLikeMirrorItem;

        public bool CanAutoExpression => ToTemplate().CanAutoExpression;

        public bool IsAutoExpression => ToTemplate().IsAutoExpression;

        public IEnumerable<EmoteItem> ToEmoteItems() => ToTemplate().ToEmoteItems();

        public IEnumerable<ExpressionItem> ToExpressionItems() => ToTemplate().ToExpressionItems();
    }
}