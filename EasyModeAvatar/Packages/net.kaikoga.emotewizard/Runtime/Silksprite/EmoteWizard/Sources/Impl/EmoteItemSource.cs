using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
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

        EmoteSequence FindEmoteSequence(EmoteWizardEnvironment environment)
        {
            var source = FindEmoteSequenceSource();
            if (!source) return null;

            return environment.EmoteSequence(source);
        }

        EmoteItem ToEmoteItem(EmoteWizardEnvironment environment)
        {
            var sequence = FindEmoteSequence(environment);
            if (sequence == null) return null;

            return new EmoteItem(trigger, sequence);
        }

        public bool LooksLikeMirrorItem => trigger.LooksLikeMirrorItem || sequence.LooksLikeMirrorItem;

        public bool CanAutoExpression
        {
            get
            {
                if (trigger.conditions.Count != 1) return false;

                var soleCondition = trigger.conditions[0];
                switch (soleCondition.kind)
                {
                    case ParameterItemKind.Auto:
                    case ParameterItemKind.Int:
                        break;
                    case ParameterItemKind.Bool:
                    case ParameterItemKind.Float:
                    default:
                        return false;
                }
                if (soleCondition.mode != EmoteConditionMode.Equals) return false;

                return true;
            }
        }

        public bool IsAutoExpression => hasExpressionItem && CanAutoExpression;

        public IEnumerable<EmoteItem> ToEmoteItems(EmoteWizardEnvironment environment)
        {
            var emoteItem = ToEmoteItem(environment);
            if (emoteItem != null) yield return emoteItem;
        }

        public IEnumerable<ExpressionItem> ToExpressionItems(ExpressionContext context)
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
                itemKind = FindEmoteSequence(context.Environment).hasExitTime ? ExpressionItemKind.Button : ExpressionItemKind.Toggle
            };
        }
    }
}