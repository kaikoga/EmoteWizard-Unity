using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class DefaultEmoteItem
    {
        HandSign _handSign;
        Motion _clip;

        static IEnumerable<DefaultEmoteItem> Defaults(LayerKind layerKind)
        {
            if (layerKind != LayerKind.Gesture)
            {
                return Enum.GetValues(typeof(HandSign)).OfType<HandSign>()
                    .Select(handSign => new DefaultEmoteItem
                    {
                        _handSign = handSign,
                        _clip = null
                    });
            }

            return new List<DefaultEmoteItem>
            {
                new DefaultEmoteItem
                {
                    _handSign = HandSign.Idle,
                    _clip = VrcSdkAssetLocator.ProxyHandsIdle()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.Fist,
                    _clip = VrcSdkAssetLocator.ProxyHandsFist()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.Open,
                    _clip = VrcSdkAssetLocator.ProxyHandsOpen()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.Point,
                    _clip = VrcSdkAssetLocator.ProxyHandsPoint()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.Peace,
                    _clip = VrcSdkAssetLocator.ProxyHandsPeace()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.RockNRoll,
                    _clip = VrcSdkAssetLocator.ProxyHandsRock()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.Gun,
                    _clip = VrcSdkAssetLocator.ProxyHandsGun()
                },
                new DefaultEmoteItem
                {
                    _handSign = HandSign.ThumbsUp,
                    _clip = VrcSdkAssetLocator.ProxyHandsThumbsUp()
                }
            };
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultHandSigns(EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind)
        {
            return Defaults(layerKind).Select(def => EmoteItemTemplate.Builder(layerKind, $"{def._handSign}", EmoteWizardConstants.Defaults.Groups.HandSign, GenericEmoteTrigger.FromHandSign(def._handSign), emoteItemKind, emoteSequenceFactoryKind)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Params.Gesture, mode = EmoteConditionMode.Equals, threshold = (int)def._handSign })
                .AddTimeParameter(def._handSign == HandSign.Fist, EmoteWizardConstants.Params.GestureWeight)
                .AddFixedDuration(true)
                .AddClip(def._clip, 0f, 0.1f)
                .ToEmoteItemTemplate());
        }
    }
}