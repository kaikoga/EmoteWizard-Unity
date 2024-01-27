using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteItemTemplateGenericBuilder
    {
        readonly string _path;
        readonly EmoteTrigger _trigger;
        readonly GenericEmoteSequence _sequence;
        bool _hasExpressionItem;
        string _expressionItemPath;
        Texture2D _expressionItemIcon;

        public EmoteItemTemplateGenericBuilder(string path, EmoteTrigger emoteTrigger, GenericEmoteSequence genericEmoteSequence)
        {
            _path = path;
            _trigger = emoteTrigger;
            _sequence = genericEmoteSequence;
        }

        public EmoteItemTemplateGenericBuilder AddPriority(int priority)
        {
            _trigger.priority = priority;
            return this;
        }

        public EmoteItemTemplateGenericBuilder AddCondition(EmoteCondition condition)
        {
            _trigger.conditions.Add(condition);
            return this;
        }

        public EmoteItemTemplateGenericBuilder AddFixedDuration(bool isFixedDuration)
        {
            _sequence.isFixedDuration = isFixedDuration;
            return this;
        }

        public EmoteItemTemplateGenericBuilder AddTransitionDuration(float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
        {
            _sequence.entryTransitionDuration = entryTransitionDuration;
            _sequence.exitTransitionDuration = exitTransitionDuration;
            return this;
        }

        public EmoteItemTemplateGenericBuilder AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut)
        {
            _sequence.hasLayerBlend = hasLayerBlend;
            _sequence.blendIn = blendIn;
            _sequence.blendOut = blendOut;
            return this;
        }

        public EmoteItemTemplateGenericBuilder AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides)
        {
            _sequence.hasTrackingOverrides = hasTrackingOverrides;
            _sequence.trackingOverrides.AddRange(trackingOverrides);
            return this;
        }

        public EmoteItemTemplateGenericBuilder AddExpressionItem(bool hasExpressionItem, string expressionItemPath, Texture2D expressionItemIcon)
        {
            _hasExpressionItem = hasExpressionItem;
            _expressionItemPath = expressionItemPath;
            _expressionItemIcon = expressionItemIcon;
            return this;
        }

        public EmoteItemTemplate ToEmoteItemTemplate()
        {
            return new EmoteItemTemplate(_path, _trigger,
                new GenericEmoteSequenceFactory(_sequence, _path),
                _hasExpressionItem, _expressionItemPath, _expressionItemIcon);
        }
    }
}