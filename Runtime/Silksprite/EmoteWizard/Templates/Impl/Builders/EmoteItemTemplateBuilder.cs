using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Builders;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl.Builders
{
    public class EmoteItemTemplateBuilder
    {
        readonly string _path;
        [CanBeNull] readonly EmoteTriggerBuilder _trigger;
        readonly GenericEmoteTrigger _genericTrigger;
        readonly IEmoteSequenceBuilder _sequence;
        bool _hasExpressionItem;
        string _expressionItemPath;
        Texture2D _expressionItemIcon;

        public EmoteItemTemplateBuilder(string path, EmoteTriggerBuilder trigger, GenericEmoteTrigger genericTrigger, IEmoteSequenceBuilder sequence)
        {
            _path = path;
            _trigger = trigger;
            _genericTrigger = genericTrigger;
            _sequence = sequence;
            _sequence.AddPath(_path);
        }

        public EmoteItemTemplateBuilder AddPriority(int priority)
        {
            _trigger?.AddPriority(priority);
            return this;
        }

        public EmoteItemTemplateBuilder AddCondition(EmoteCondition condition)
        {
            _trigger?.AddCondition(condition);
            return this;
        }

        public EmoteItemTemplateBuilder AddFixedDuration(bool isFixedDuration)
        {
            _sequence.AddFixedDuration(isFixedDuration);
            return this;
        }

        public EmoteItemTemplateBuilder AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
        {
            _sequence.AddClip(clip, entryTransitionDuration, exitTransitionDuration);
            return this;
        }

        public EmoteItemTemplateBuilder AddClipExitTime(bool hasExitTime, float clipExitTime)
        {
            _sequence.AddClipExitTime(hasExitTime, clipExitTime);
            return this;
        }

        public EmoteItemTemplateBuilder AddTimeParameter(bool hasTimeParameter, string timeParameter)
        {
            _sequence.AddTimeParameter(hasTimeParameter, timeParameter);
            return this;
        }

        public EmoteItemTemplateBuilder AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration)
        {
            _sequence.AddExitClip(hasExitClip, exitClip, exitClipExitTime, postExitTransitionDuration);
            return this;
        }

        public EmoteItemTemplateBuilder AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut)
        {
            _sequence.AddLayerBlend(hasLayerBlend, blendIn, blendOut);
            return this;
        }

        public EmoteItemTemplateBuilder AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides)
        {
            _sequence.AddTrackingOverrides(hasTrackingOverrides, trackingOverrides);
            return this;
        }

        public EmoteItemTemplateBuilder AddExpressionItem(bool hasExpressionItem, string expressionItemPath, Texture2D expressionItemIcon)
        {
            _hasExpressionItem = hasExpressionItem;
            _expressionItemPath = expressionItemPath;
            _expressionItemIcon = expressionItemIcon;
            return this;
        }

        public IEmoteTemplate ToEmoteItemTemplate()
        {
            if (_trigger != null)
            {
                return new EmoteItemTemplate(_path, _trigger.ToEmoteTrigger(),
                    _sequence.ToEmoteSequenceFactory(),
                    _hasExpressionItem, _expressionItemPath, _expressionItemIcon);
            }
            else
            {
                return new GenericEmoteItemTemplate(_path, _genericTrigger, _sequence.ToEmoteSequenceFactory());
            }
        }
    }
}