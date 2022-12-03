using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteItemTemplate
    {
        public readonly EmoteTrigger Trigger;
        public readonly EmoteSequence Sequence;
        
        public EmoteItemTemplate() : this(new EmoteTrigger(), new EmoteSequence()) { }

        public EmoteItemTemplate(EmoteTrigger trigger, EmoteSequence sequence)
        {
            this.Trigger = trigger;
            this.Sequence = sequence;
        }

        public static EmoteItemTemplateBuilder Builder(LayerKind layerKind, string name, string groupName)
        {
            return new EmoteItemTemplateBuilder(new EmoteItemTemplate
            {
                Trigger =
                {
                    layerKind = layerKind,
                    name = name,
                    groupName = groupName
                }
            });
        }

        public class EmoteItemTemplateBuilder
        {
            readonly EmoteItemTemplate _emoteItemTemplate;

            public EmoteItemTemplateBuilder(EmoteItemTemplate emoteItemTemplate) => _emoteItemTemplate = emoteItemTemplate;

            public EmoteItemTemplateBuilder AddPriority(int priority)
            {
                _emoteItemTemplate.Trigger.priority = priority;
                return this;
            }

            public EmoteItemTemplateBuilder AddCondition(EmoteCondition condition)
            {
                _emoteItemTemplate.Trigger.conditions.Add(condition);
                return this;
            }

            public EmoteItemTemplateBuilder AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
            {
                _emoteItemTemplate.Sequence.clip = clip;
                _emoteItemTemplate.Sequence.entryTransitionDuration = entryTransitionDuration;
                _emoteItemTemplate.Sequence.exitTransitionDuration = exitTransitionDuration;
                return this;
            }

            public EmoteItemTemplateBuilder AddClipExitTime(bool hasExitTime, float clipExitTime)
            {
                _emoteItemTemplate.Sequence.hasExitTime = hasExitTime;
                _emoteItemTemplate.Sequence.clipExitTime = clipExitTime;
                return this;
            }

            public EmoteItemTemplateBuilder AddTimeParameter(bool hasTimeParameter, string timeParameter)
            {
                _emoteItemTemplate.Sequence.hasTimeParameter = hasTimeParameter;
                _emoteItemTemplate.Sequence.timeParameter = timeParameter;
                return this;
            }

            public EmoteItemTemplateBuilder AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration)
            {
                _emoteItemTemplate.Sequence.hasExitClip = hasExitClip;
                _emoteItemTemplate.Sequence.exitClip = exitClip;
                _emoteItemTemplate.Sequence.exitClipExitTime = exitClipExitTime;
                _emoteItemTemplate.Sequence.postExitTransitionDuration = postExitTransitionDuration;
                return this;
            }

            public EmoteItemTemplateBuilder AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut)
            {
                _emoteItemTemplate.Sequence.hasLayerBlend = hasLayerBlend;
                _emoteItemTemplate.Sequence.blendIn = blendIn;
                _emoteItemTemplate.Sequence.blendOut = blendOut;
                return this;
            }

            public EmoteItemTemplateBuilder AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides)
            {
                _emoteItemTemplate.Sequence.hasTrackingOverrides = hasTrackingOverrides;
                _emoteItemTemplate.Sequence.trackingOverrides.AddRange(trackingOverrides);
                return this;
            }

            public EmoteItemTemplate ToEmoteItemTemplate() => _emoteItemTemplate;
        }
    }
}
