using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteItemTemplateBuilder
    {
        readonly string _path;
        readonly EmoteTrigger _trigger;
        readonly EmoteSequence _sequence;
        bool _hasExpressionItem;
        string _expressionItemPath;
        Texture2D _expressionItemIcon;

        public EmoteItemTemplateBuilder(string path, EmoteTrigger emoteTrigger, EmoteSequence emoteSequence)
        {
            _path = path;
            _trigger = emoteTrigger;
            _sequence = emoteSequence;
        }

        public EmoteItemTemplateBuilder AddPriority(int priority)
        {
            _trigger.priority = priority;
            return this;
        }

        public EmoteItemTemplateBuilder AddCondition(EmoteCondition condition)
        {
            _trigger.conditions.Add(condition);
            return this;
        }

        public EmoteItemTemplateBuilder AddFixedDuration(bool isFixedDuration)
        {
            _sequence.isFixedDuration = isFixedDuration;
            return this;
        }

        public EmoteItemTemplateBuilder AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
        {
            _sequence.clip = clip;
            _sequence.entryTransitionDuration = entryTransitionDuration;
            _sequence.exitTransitionDuration = exitTransitionDuration;
            return this;
        }

        public EmoteItemTemplateBuilder AddClipExitTime(bool hasExitTime, float clipExitTime)
        {
            _sequence.hasExitTime = hasExitTime;
            _sequence.clipExitTime = clipExitTime;
            return this;
        }

        public EmoteItemTemplateBuilder AddTimeParameter(bool hasTimeParameter, string timeParameter)
        {
            _sequence.hasTimeParameter = hasTimeParameter;
            _sequence.timeParameter = timeParameter;
            return this;
        }

        public EmoteItemTemplateBuilder AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration)
        {
            _sequence.hasExitClip = hasExitClip;
            _sequence.exitClip = exitClip;
            _sequence.exitClipExitTime = exitClipExitTime;
            _sequence.postExitTransitionDuration = postExitTransitionDuration;
            return this;
        }

        public EmoteItemTemplateBuilder AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut)
        {
            _sequence.hasLayerBlend = hasLayerBlend;
            _sequence.blendIn = blendIn;
            _sequence.blendOut = blendOut;
            return this;
        }

        public EmoteItemTemplateBuilder AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides)
        {
            _sequence.hasTrackingOverrides = hasTrackingOverrides;
            _sequence.trackingOverrides.AddRange(trackingOverrides);
            return this;
        }

        public EmoteItemTemplateBuilder AddExpressionItem(bool hasExpressionItem, string expressionItemPath, Texture2D expressionItemIcon)
        {
            _hasExpressionItem = hasExpressionItem;
            _expressionItemPath = expressionItemPath;
            _expressionItemIcon = expressionItemIcon;
            return this;
        }

        public EmoteItemTemplate ToEmoteItemTemplate()
        {
            return new EmoteItemTemplate(_path, _trigger,
                new EmoteSequenceFactory(_sequence),
                _hasExpressionItem, _expressionItemPath, _expressionItemIcon);
        }
    }
}