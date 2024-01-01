using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteItemTemplate : IEmoteTemplate
    {
        public readonly EmoteTrigger Trigger;
        public readonly IEmoteFactoryTemplate Factory;

        public readonly bool HasExpressionItem;
        public readonly string ExpressionItemPath;
        public readonly Texture2D ExpressionItemIcon;

        public EmoteItemTemplate(EmoteTrigger trigger, IEmoteFactoryTemplate factory, bool hasExpressionItem, string expressionItemPath, Texture2D expressionItemIcon)
        {
            Trigger = trigger;
            Factory = factory;
            HasExpressionItem = hasExpressionItem;
            ExpressionItemPath = expressionItemPath;
            ExpressionItemIcon = expressionItemIcon;
        }

        public bool LooksLikeMirrorItem => Trigger.LooksLikeMirrorItem || (Factory != null && Factory.LooksLikeMirrorItem);

        public bool CanAutoExpression
        {
            get
            {
                if (Trigger.conditions.Count != 1) return false;

                var soleCondition = Trigger.conditions[0];
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

        EmoteItem ToEmoteItem() => Factory == null ? null : new EmoteItem(Trigger, Factory);

        public IEnumerable<EmoteItem> ToEmoteItems()
        {
            var emoteItem = ToEmoteItem();
            if (emoteItem != null) yield return emoteItem;
        }

        public IEnumerable<ExpressionItem> ToExpressionItems()
        {
            if (!IsAutoExpression) yield break;

            var soleCondition = Trigger.conditions[0];
            yield return new ExpressionItem
            {
                enabled = true,
                icon = ExpressionItemIcon,
                path = ExpressionItemPath,
                parameter = soleCondition.parameter,
                value = soleCondition.threshold,
                itemKind = Factory.LooksLikeToggle ? ExpressionItemKind.Button : ExpressionItemKind.Toggle
            };
        }

        public static EmoteItemTemplateBuilder Builder(LayerKind layerKind, string name, string groupName)
        {
            return new EmoteItemTemplateBuilder(
                new EmoteTrigger
                {
                    name = name,
                },
                new EmoteSequence
                {
                    layerKind = layerKind,
                    groupName = groupName
                });
        }
        
        public class EmoteItemTemplateBuilder
        {
            readonly EmoteTrigger _trigger;
            readonly EmoteSequence _sequence;
            bool _hasExpressionItem;
            string _expressionItemPath;
            Texture2D _expressionItemIcon;

            public EmoteItemTemplateBuilder(EmoteTrigger emoteTrigger, EmoteSequence emoteSequence)
            {
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

            public EmoteItemTemplate ToEmoteItemTemplate() => new EmoteItemTemplate(_trigger, (IEmoteFactoryTemplate)((IEmoteFactory)new StaticEmoteFactory(_sequence)), _hasExpressionItem, _expressionItemPath, _expressionItemIcon);
        }
    }
}
