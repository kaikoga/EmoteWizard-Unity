using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Builders;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Templates.Impl.Builders;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteItemTemplate : ICompositeEmoteTemplate, IEmoteItemTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly EmoteTrigger _trigger;
        readonly IEmoteSequenceFactoryTemplate? _sequenceFactory;

        readonly bool _hasExpressionItem;
        readonly string _expressionItemPath;
        readonly Texture2D? _expressionItemIcon;

        public EmoteItemTemplate(EmoteTemplatePath path,
            EmoteTrigger trigger,
            IEmoteSequenceFactoryTemplate? sequenceFactory,
            bool hasExpressionItem,
            string expressionItemPath,
            Texture2D? expressionItemIcon)
        {
            _path = path;
            _trigger = trigger;
            _sequenceFactory = sequenceFactory;
            _hasExpressionItem = hasExpressionItem;
            _expressionItemPath = expressionItemPath;
            _expressionItemIcon = expressionItemIcon;
        }

        public bool LooksLikeMirrorItem => _trigger.LooksLikeMirrorItem || (_sequenceFactory != null && _sequenceFactory.LooksLikeMirrorItem);

        public bool CanAutoExpression(IPlatformFeatures platformFeatures)
        {
            if (_trigger.conditions.Count != 1) return false;

            var soleCondition = _trigger.conditions[0];
            if (platformFeatures.IsDefaultParameterReference(soleCondition.parameter)) return false;
            switch (soleCondition.kind)
            {
                case ParameterItemKind.Auto:
                case ParameterItemKind.Int:
                    break;
                case ParameterItemKind.Bool:
                case ParameterItemKind.Float:
                case ParameterItemKind.HandSign:
                default:
                    return false;
            }
            if (soleCondition.mode != EmoteConditionMode.Equals) return false;

            return true;
        }

        public bool IsAutoExpression(IPlatformFeatures platformFeatures) => _hasExpressionItem && CanAutoExpression(platformFeatures);

        IEnumerable<EmoteItem> IEmoteItemTemplate.ToEmoteItems(IPlatformFeatures platformFeatures)
        {
            if (_sequenceFactory != null)
            {
                yield return new EmoteItem(_trigger.ToInstance(platformFeatures), _sequenceFactory);
            }
        }

        IEnumerable<ExpressionItem> ToExpressionItems(IPlatformFeatures platformFeatures)
        {
            if (!IsAutoExpression(platformFeatures)) yield break;
            if (ItemPathUtil.IsInvalidPathFormat(_expressionItemPath)) yield break;

            var soleCondition = _trigger.conditions[0];
            yield return new ExpressionItem
            {
                enabled = true,
                icon = _expressionItemIcon,
                path = _expressionItemPath,
                parameter = soleCondition.parameter,
                value = soleCondition.threshold,
                itemKind = _sequenceFactory?.LooksLikeToggle == true ? ExpressionItemKind.Toggle : ExpressionItemKind.Button
            };
        }

        IEnumerable<IEmoteTemplate> ICompositeEmoteTemplate.Unpack(IPlatformFeatures platformFeatures)
        {
            return ToExpressionItems(platformFeatures)
                .Select(expressionItem => new ExpressionItemTemplate(_path, expressionItem));
        }

        public static EmoteItemTemplateBuilder Builder(LayerKind layerKind,
            EmoteTemplatePath path, string groupName,
            GenericEmoteTrigger genericTrigger,
            EmoteItemKind itemKind, EmoteSequenceFactoryKind sequenceKind)
        {
            EmoteTriggerBuilder? trigger;
            switch (itemKind)
            {
                case EmoteItemKind.EmoteItem:
                    trigger = EmoteTrigger.Builder(path.FileName);
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
            return new EmoteItemTemplateBuilder(path, trigger, genericTrigger, sequence);
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<EmoteItemSource>(target);
            source.trigger = _trigger;
            _sequenceFactory?.PopulateSequenceSource(undoable, source);
            source.hasExpressionItem = _hasExpressionItem;
            source.expressionItemPath = _expressionItemPath;
            source.expressionItemIcon = _expressionItemIcon;
        }
    }
}
