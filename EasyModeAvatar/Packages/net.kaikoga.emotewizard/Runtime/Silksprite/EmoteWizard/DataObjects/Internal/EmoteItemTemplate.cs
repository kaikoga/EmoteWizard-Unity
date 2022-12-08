using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteItemTemplate
    {
        public readonly EmoteTrigger Trigger;
        public readonly EmoteSequence Sequence;

        public EmoteItemTemplate(EmoteTrigger trigger, EmoteSequence sequence)
        {
            Trigger = trigger;
            Sequence = sequence;
        }

        public static EmoteItemTemplateBuilder Builder(LayerKind layerKind, string name, string groupName)
        {
            return new EmoteItemTemplateBuilder(new EmoteItemTemplate(
                new EmoteTrigger
                {
                    name = name,
                },
                new EmoteSequence
                {
                    layerKind = layerKind,
                    groupName = groupName
                }));
        }

        public bool IsMirrorItem
        {
            get
            {
                bool IsMirrorParameter(string parameter)
                {
                    switch (parameter)
                    {
                        case EmoteWizardConstants.Params.Gesture:
                        case EmoteWizardConstants.Params.GestureOther:
                        case EmoteWizardConstants.Params.GestureWeight:
                        case EmoteWizardConstants.Params.GestureOtherWeight:
                            return true;
                        default:
                            return false;
                    }
                }
                foreach (var condition in Trigger.conditions)
                {
                    if (IsMirrorParameter(condition.parameter)) return true;
                }
                if (IsMirrorParameter(Sequence.timeParameter)) return true;

                return false;
            }
        }

        public EmoteItem ToEmoteItem() => new EmoteItem(Trigger, Sequence, EmoteHand.Neither);

        public EmoteItem Mirror(EmoteHand handValue)
        {
            string ResolveMirrorParameter(string parameter)
            {
                switch (parameter)
                {
                    case EmoteWizardConstants.Params.Gesture:
                        return handValue == EmoteHand.Left ? "GestureLeft" : "GestureRight";
                    case EmoteWizardConstants.Params.GestureOther:
                        return handValue == EmoteHand.Left ? "GestureRight" : "GestureLeft";
                    case EmoteWizardConstants.Params.GestureWeight:
                        return handValue == EmoteHand.Left ? "GestureLeftWeight" : "GestureRightWeight";
                    case EmoteWizardConstants.Params.GestureOtherWeight:
                        return handValue == EmoteHand.Left ? "GestureRightWeight" : "GestureLeftWeight";
                    default:
                        return parameter;
                }
            }

            var item = new EmoteItem(SerializableUtils.Clone(Trigger),
                SerializableUtils.Clone(Sequence),
                handValue);

            foreach (var condition in item.trigger.conditions)
            {
                condition.parameter = ResolveMirrorParameter(condition.parameter);
            }
            item.sequence.timeParameter = ResolveMirrorParameter(item.sequence.timeParameter);

            return item;
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

            public EmoteItemTemplateBuilder AddFixedDuration(bool isFixedDuration)
            {
                _emoteItemTemplate.Sequence.isFixedDuration = isFixedDuration;
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
