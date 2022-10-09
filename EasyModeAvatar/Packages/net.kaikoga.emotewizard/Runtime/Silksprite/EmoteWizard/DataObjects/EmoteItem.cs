using System;
using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteItem
    {
        [SerializeField] public EmoteTrigger trigger;
        [SerializeField] public EmoteSequence sequence;
        
        public EmoteItem() : this(new EmoteTrigger(), new EmoteSequence()) { }

        public EmoteItem(EmoteTrigger trigger, EmoteSequence sequence)
        {
            this.trigger = trigger;
            this.sequence = sequence;
        }

        public static EmoteItemBuilder Builder(LayerKind layerKind, string name, string groupName)
        {
            return new EmoteItemBuilder(new EmoteItem
            {
                trigger =
                {
                    layerKind = layerKind,
                    name = name,
                    groupName = groupName
                }
            });
        }

        public bool IsMirrorItem
        {
            get
            {
                bool IsMirrorParameter(string parameter)
                {
                    switch (parameter)
                    {
                        case "Gesture":
                        case "GestureOther":
                        case "GestureWeight":
                        case "GestureOtherWeight":
                            return true;
                        default:
                            return false;
                    }
                }
                foreach (var condition in trigger.conditions)
                {
                    if (IsMirrorParameter(condition.parameter)) return true;
                }
                if (IsMirrorParameter(sequence.timeParameter)) return true;

                return false;
            }
        }

        public EmoteItem Mirror(string side)
        {
            string ResolveMirrorParameter(string parameter)
            {
                switch (parameter)
                {
                    case "Gesture":
                        return side == "Left" ? "GestureLeft" : "GestureRight";
                    case "GestureOther":
                        return side == "Left" ? "GestureRight" : "GestureLeft";
                    case "GestureWeight":
                        return side == "Left" ? "GestureLeftWeight" : "GestureRightWeight";
                    case "GestureOtherWeight":
                        return side == "Left" ? "GestureRightWeight" : "GestureLeftWeight";
                    default:
                        return parameter;
                }
            }
            var item = SerializableUtils.Clone(this);
            item.trigger.groupName = $"{item.trigger.groupName} ({side})";
            foreach (var condition in item.trigger.conditions)
            {
                condition.parameter = ResolveMirrorParameter(condition.parameter);
            }
            item.sequence.timeParameter = ResolveMirrorParameter(item.sequence.timeParameter);
            return item;
        }

        public class EmoteItemBuilder
        {
            readonly EmoteItem _emoteItem;

            public EmoteItemBuilder(EmoteItem emoteItem) => _emoteItem = emoteItem;

            public EmoteItemBuilder AddPriority(int priority)
            {
                _emoteItem.trigger.priority = priority;
                return this;
            }

            public EmoteItemBuilder AddCondition(EmoteCondition condition)
            {
                _emoteItem.trigger.conditions.Add(condition);
                return this;
            }

            public EmoteItemBuilder AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
            {
                _emoteItem.sequence.clip = clip;
                _emoteItem.sequence.entryTransitionDuration = entryTransitionDuration;
                _emoteItem.sequence.exitTransitionDuration = exitTransitionDuration;
                return this;
            }

            public EmoteItemBuilder AddClipExitTime(bool hasExitTime, float clipExitTime)
            {
                _emoteItem.sequence.hasExitTime = hasExitTime;
                _emoteItem.sequence.clipExitTime = clipExitTime;
                return this;
            }

            public EmoteItemBuilder AddTimeParameter(bool hasTimeParameter, string timeParameter)
            {
                _emoteItem.sequence.hasTimeParameter = hasTimeParameter;
                _emoteItem.sequence.timeParameter = timeParameter;
                return this;
            }

            public EmoteItemBuilder AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration)
            {
                _emoteItem.sequence.hasExitClip = hasExitClip;
                _emoteItem.sequence.exitClip = exitClip;
                _emoteItem.sequence.exitClipExitTime = exitClipExitTime;
                _emoteItem.sequence.postExitTransitionDuration = postExitTransitionDuration;
                return this;
            }

            public EmoteItemBuilder AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut)
            {
                _emoteItem.sequence.hasLayerBlend = hasLayerBlend;
                _emoteItem.sequence.blendIn = blendIn;
                _emoteItem.sequence.blendOut = blendOut;
                return this;
            }

            public EmoteItemBuilder AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides)
            {
                _emoteItem.sequence.hasTrackingOverrides = hasTrackingOverrides;
                _emoteItem.sequence.trackingOverrides.AddRange(trackingOverrides);
                return this;
            }

            public EmoteItem ToEmoteItem() => _emoteItem;
        }
    }
}
