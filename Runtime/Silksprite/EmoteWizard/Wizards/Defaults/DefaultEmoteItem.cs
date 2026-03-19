using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards.Defaults
{
    public class DefaultEmoteItem
    {
        HandSign _handSign;
        Motion _clip;

        static DefaultEmoteItem Default(IPlatformFeatures platformFeatures, LayerKind layerKind, HandSign handSign) =>
            new DefaultEmoteItem
            {
                _handSign = handSign,
                _clip = (layerKind, handSign) switch
                {
                    (LayerKind.Gesture, HandSign.Idle) => platformFeatures.GestureClipIdleLeft,
                    (LayerKind.Gesture, HandSign.Fist) => platformFeatures.GestureClipFistLeft,
                    (LayerKind.Gesture, HandSign.Open) => platformFeatures.GestureClipOpenLeft,
                    (LayerKind.Gesture, HandSign.Point) => platformFeatures.GestureClipPointLeft,
                    (LayerKind.Gesture, HandSign.Peace) => platformFeatures.GestureClipPeaceLeft,
                    (LayerKind.Gesture, HandSign.RockNRoll) => platformFeatures.GestureClipRockNRollLeft,
                    (LayerKind.Gesture, HandSign.Gun) => platformFeatures.GestureClipGunLeft,
                    (LayerKind.Gesture, HandSign.ThumbsUp) => platformFeatures.GestureClipThumbsUpLeft,
                    _ => null
                }
            };
        IEmoteTemplate ToEmoteItemTemplate(EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind)
        {
            return EmoteItemTemplate.Builder(layerKind, $"{_handSign}", EmoteWizardConstants.Defaults.Groups.HandSign, GenericEmoteTrigger.FromHandSign(_handSign), emoteItemKind, emoteSequenceFactoryKind)
                .AddCondition(new EmoteCondition
                {
                    kind = ParameterItemKind.Int,
                    parameter = EmoteWizardConstants.Params.Gesture,
                    mode = EmoteConditionMode.Equals,
                    threshold = (int)_handSign
                })
                .AddTimeParameter(_handSign == HandSign.Fist, EmoteWizardConstants.Params.GestureWeight)
                .AddFixedDuration(true)
                .AddClip(_clip, 0f, 0.1f)
                .ToEmoteItemTemplate();
        }

        public static IEmoteTemplate DefaultHandSign(EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, IPlatformFeatures platformFeatures, LayerKind layerKind, HandSign handSign)
        {
            return Default(platformFeatures, layerKind, handSign)
                .ToEmoteItemTemplate(emoteItemKind, emoteSequenceFactoryKind, layerKind);
        }
        
        public static IEnumerable<IEmoteTemplate> EnumerateDefaultHandSigns(EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, IPlatformFeatures platformFeatures, LayerKind layerKind)
        {
            return Enum.GetValues(typeof(HandSign)).OfType<HandSign>()
                .Select(handSign => DefaultHandSign(emoteItemKind, emoteSequenceFactoryKind, platformFeatures, layerKind, handSign));
        }
    }
}