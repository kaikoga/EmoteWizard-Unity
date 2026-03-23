using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Defaults
{
    class DefaultEmoteItemDef
    {
        HandSign _handSign;
        Motion? _clipLeft;
        Motion? _clipRight;

        public IEmoteTemplate ToEmoteItemTemplate(EmoteTemplatePath path, EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind)
        {
            var builder = EmoteItemTemplate.Builder(layerKind, path, EmoteWizardConstants.Groups.HandSign, GenericEmoteTrigger.FromHandSign(_handSign), emoteItemKind, emoteSequenceFactoryKind)
                .AddCondition(new EmoteCondition
                {
                    kind = ParameterItemKind.Int,
                    parameter = EmoteWizardConstants.Params.Gesture,
                    mode = EmoteConditionMode.Equals,
                    threshold = (int)_handSign
                })
                .AddTimeParameter(_handSign == HandSign.Fist, EmoteWizardConstants.Params.GestureWeight)
                .AddFixedDuration(true);
            if (layerKind == LayerKind.Gesture)
            {
                if (_clipLeft == _clipRight)
                {
                    builder.AddClip(_clipLeft, 0f, 0.1f);
                }
                else
                {
                    builder.AddMirroredClip(_clipLeft, _clipRight, 0f, 0.1f);
                }
            }
            return builder.ToEmoteItemTemplate();
        }

        public static DefaultEmoteItemDef Default(IPlatformFeatures platformFeatures, LayerKind layerKind, HandSign handSign) =>
            new DefaultEmoteItemDef
            {
                _handSign = handSign,
                _clipLeft = (layerKind, handSign) switch
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
                },
                _clipRight = (layerKind, handSign) switch
                {
                    (LayerKind.Gesture, HandSign.Idle) => platformFeatures.GestureClipIdleRight,
                    (LayerKind.Gesture, HandSign.Fist) => platformFeatures.GestureClipFistRight,
                    (LayerKind.Gesture, HandSign.Open) => platformFeatures.GestureClipOpenRight,
                    (LayerKind.Gesture, HandSign.Point) => platformFeatures.GestureClipPointRight,
                    (LayerKind.Gesture, HandSign.Peace) => platformFeatures.GestureClipPeaceRight,
                    (LayerKind.Gesture, HandSign.RockNRoll) => platformFeatures.GestureClipRockNRollRight,
                    (LayerKind.Gesture, HandSign.Gun) => platformFeatures.GestureClipGunRight,
                    (LayerKind.Gesture, HandSign.ThumbsUp) => platformFeatures.GestureClipThumbsUpRight,
                    _ => null
                }
            };
    }
}
