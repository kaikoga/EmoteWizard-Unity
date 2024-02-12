using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Builders;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Templates.Impl.Builders;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteItemTemplate : IEmoteTemplate
    {
        public string Path { get; }
        public readonly EmoteTrigger Trigger;
        public readonly IEmoteSequenceFactoryTemplate SequenceFactory;

        public readonly bool HasExpressionItem;
        public readonly string ExpressionItemPath;
        public readonly Texture2D ExpressionItemIcon;

        public EmoteItemTemplate(string path,
            EmoteTrigger trigger,
            IEmoteSequenceFactoryTemplate sequenceFactory,
            bool hasExpressionItem,
            string expressionItemPath,
            Texture2D expressionItemIcon)
        {
            Path = path;
            Trigger = trigger;
            SequenceFactory = sequenceFactory;
            HasExpressionItem = hasExpressionItem;
            ExpressionItemPath = expressionItemPath;
            ExpressionItemIcon = expressionItemIcon;
        }

        public bool LooksLikeMirrorItem => Trigger.LooksLikeMirrorItem || (SequenceFactory != null && SequenceFactory.LooksLikeMirrorItem);

        public bool CanAutoExpression
        {
            get
            {
                if (Trigger.conditions.Count != 1) return false;

                var soleCondition = Trigger.conditions[0];
                if (DefaultParameters.IsDefaultParameter(soleCondition.parameter)) return false;
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

        public bool IsAutoExpression => HasExpressionItem && CanAutoExpression;

        EmoteItem ToEmoteItem() => SequenceFactory == null ? null : new EmoteItem(Trigger, SequenceFactory);

        public IEnumerable<EmoteItem> ToEmoteItems()
        {
            var emoteItem = ToEmoteItem();
            if (emoteItem != null) yield return emoteItem;
        }

        public IEnumerable<ExpressionItem> ToExpressionItems()
        {
            if (!IsAutoExpression) yield break;
            if (ItemPathAttribute.IsInvalidPathInput(ExpressionItemPath)) yield break;

            var soleCondition = Trigger.conditions[0];
            yield return new ExpressionItem
            {
                enabled = true,
                icon = ExpressionItemIcon,
                path = ExpressionItemPath,
                parameter = soleCondition.parameter,
                value = soleCondition.threshold,
                itemKind = SequenceFactory?.LooksLikeToggle == true ? ExpressionItemKind.Toggle : ExpressionItemKind.Button
            };
        }

        public static EmoteItemTemplateBuilder Builder(LayerKind layerKind, string name, string groupName)
            => Builder(layerKind, name, groupName, default, EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence);

        public static EmoteItemTemplateBuilder Builder(LayerKind layerKind,
            string name, string groupName,
            GenericEmoteTrigger genericTrigger,
            EmoteItemKind itemKind, EmoteSequenceFactoryKind sequenceKind)
        {
            EmoteTriggerBuilder trigger;
            switch (itemKind)
            {
                case EmoteItemKind.EmoteItem:
                    trigger = EmoteTrigger.Builder(name);
                    break;
                case EmoteItemKind.GenericEmoteItem:
                    trigger = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null);
            }
            IEmoteSequenceBuilder sequence;
            switch (sequenceKind)
            {
                case EmoteSequenceFactoryKind.EmoteSequence:
                    sequence = EmoteSequence.Builder(layerKind, groupName);
                    break;
                case EmoteSequenceFactoryKind.GenericEmoteSequence:
                    sequence = GenericEmoteSequence.Builder(layerKind, groupName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sequenceKind), sequenceKind, null);
            }
            return new EmoteItemTemplateBuilder(name, trigger, genericTrigger, sequence);
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<EmoteItemSource>(target);
            source.trigger = Trigger;
            SequenceFactory?.PopulateSequenceSource(undoable, source);
            source.hasExpressionItem = HasExpressionItem;
            source.expressionItemPath = ExpressionItemPath;
            source.expressionItemIcon = ExpressionItemIcon;
        }
    }
}
