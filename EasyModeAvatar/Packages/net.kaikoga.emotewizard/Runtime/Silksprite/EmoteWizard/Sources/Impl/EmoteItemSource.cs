using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource
    {
        [SerializeField] public EmoteTrigger trigger;

        [Header("Expression Item")]
        [SerializeField] public bool hasExpressionItem;
        [SerializeField] public string expressionItemPath;
        [SerializeField] public Texture2D expressionItemIcon;

        EmoteSequence FindEmoteSequence()
        {
            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .Select(source => source.EmoteSequence)
                .FirstOrDefault();
        }

        EmoteItem ToEmoteItem()
        {
            var sequence = FindEmoteSequence();
            if (sequence == null) return null;

            return new EmoteItem(trigger, sequence);
        }

        public bool IsMirrorItem
        {
            get
            {
                var item = ToEmoteItem();
                if (item == null) return false;

                return item.IsMirrorItem;
            }
        }

        public bool CanAutoExpression
        {
            get
            {
                if (trigger.conditions.Count != 1) return false;

                var soleCondition = trigger.conditions[0];
                if (soleCondition.kind != ParameterItemKind.Int) return false;
                if (soleCondition.mode != EmoteConditionMode.Equals) return false;

                return true;
            }
        }

        public bool IsAutoExpression => hasExpressionItem && CanAutoExpression;

        public IEnumerable<EmoteItem> EmoteItems
        {
            get
            {
                var emoteItem = ToEmoteItem();
                if (emoteItem != null) yield return emoteItem;
            }
        }

        public IEnumerable<ExpressionItem> ExpressionItems
        {
            get
            {
                if (!IsAutoExpression) yield break;

                var soleCondition = trigger.conditions[0];
                yield return new ExpressionItem
                {
                    enabled = true,
                    icon = expressionItemIcon,
                    path = expressionItemPath,
                    parameter = soleCondition.parameter,
                    value = soleCondition.threshold,
                    itemKind = FindEmoteSequence().hasExitTime ? ExpressionItemKind.Button : ExpressionItemKind.Toggle
                };
            }
        }
    }
}