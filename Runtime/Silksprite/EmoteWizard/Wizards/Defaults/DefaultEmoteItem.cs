using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards.Defaults
{
    public class DefaultEmoteItem
    {
        HandSign _handSign;
        Motion _clip;

        static DefaultEmoteItem Default(LayerKind layerKind, HandSign handSign) =>
            new DefaultEmoteItem
            {
                _handSign = handSign,
                _clip = (layerKind, handSign) switch
                {
                    (LayerKind.Gesture, HandSign.Idle) => VrcSdkAssetLocator.ProxyHandsIdle(),
                    (LayerKind.Gesture, HandSign.Fist) => VrcSdkAssetLocator.ProxyHandsFist(),
                    (LayerKind.Gesture, HandSign.Open) => VrcSdkAssetLocator.ProxyHandsOpen(),
                    (LayerKind.Gesture, HandSign.Point) => VrcSdkAssetLocator.ProxyHandsPoint(),
                    (LayerKind.Gesture, HandSign.Peace) => VrcSdkAssetLocator.ProxyHandsPeace(),
                    (LayerKind.Gesture, HandSign.RockNRoll) => VrcSdkAssetLocator.ProxyHandsRock(),
                    (LayerKind.Gesture, HandSign.Gun) => VrcSdkAssetLocator.ProxyHandsGun(),
                    (LayerKind.Gesture, HandSign.ThumbsUp) => VrcSdkAssetLocator.ProxyHandsThumbsUp(),
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

        public static IEmoteTemplate DefaultHandSign(EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind, HandSign handSign)
        {
            return Default(layerKind, handSign)
                .ToEmoteItemTemplate(emoteItemKind, emoteSequenceFactoryKind, layerKind);
        }
        
        public static IEnumerable<IEmoteTemplate> EnumerateDefaultHandSigns(EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind)
        {
            return Enum.GetValues(typeof(HandSign)).OfType<HandSign>()
                .Select(handSign => DefaultHandSign(emoteItemKind, emoteSequenceFactoryKind, layerKind, handSign));
        }
    }
}