using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Defaults;
using Silksprite.EmoteWizard.Platforms.References;
using Silksprite.EmoteWizard.Platforms.Utils;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;
using static Silksprite.EmoteWizard.EmoteWizardConstants;
using static Silksprite.EmoteWizard.PlatformConstants;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        class ChilloutVRFeatures : IPlatformFeatures
        {
            readonly IPlatformReferences _platformReferences;
            readonly DefaultParameterDatabase _defaultParameterDatabase;

            static DefaultParameterDatabase DefaultParameterDatabase(IPlatformReferences platformReferences)
            {
                var empty = Array.Empty<int>();

                return new DefaultParameterDatabase(new[]
                {
                    (platformReferences.VisemeReference, ChilloutVR.Params.VisemeIdx, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
                    ("MovementX", "MovementX", ParameterItemKind.Float, empty),
                    ("MovementY", "MovementY", ParameterItemKind.Float, empty),
                    ("Grounded", "Grounded", ParameterItemKind.Bool, empty),
                    (platformReferences.ActionSelectReference, ChilloutVR.Params.Emote, ParameterItemKind.Int, empty),
                    ("CancelEmote", "CancelEmote", ParameterItemKind.Bool, empty),
                    (platformReferences.GestureLeftWeightReference, ChilloutVR.Params.GestureLeft, ParameterItemKind.Float, empty),
                    (platformReferences.GestureRightWeightReference, ChilloutVR.Params.GestureRight, ParameterItemKind.Float, empty),
                    (platformReferences.GestureLeftReference, ChilloutVR.Params.GestureLeftIdx, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (platformReferences.GestureRightReference, ChilloutVR.Params.GestureRightIdx, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    ("Toggle", "Toggle", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    ("Sitting", "Sitting", ParameterItemKind.Bool, empty),
                    ("Crouching", "Crouching", ParameterItemKind.Bool, empty),
                    ("Prone", "Prone", ParameterItemKind.Bool, empty),
                    ("Flying", "Flying", ParameterItemKind.Bool, empty),
                    ("Swimming", "Swimming", ParameterItemKind.Bool, empty),
                    (Params.Afk, Params.Afk, ParameterItemKind.Bool, empty),

                    // These are mirrored into appropriate CVR default parameters
                    (Params.Gesture, Params.Gesture, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (Params.GestureOther, Params.GestureOther, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (Params.GestureWeight, Params.GestureWeight, ParameterItemKind.Float, empty),
                    (Params.GestureOtherWeight, Params.GestureOtherWeight, ParameterItemKind.Float, empty),
                });
            }

            public ChilloutVRFeatures(IPlatformReferences platformReferences)
            {
                _platformReferences = platformReferences;
                _defaultParameterDatabase = DefaultParameterDatabase(platformReferences);
            }

            string IPlatformFeatures.ParameterReferenceForActionSelect => _platformReferences.ActionSelectReference;
            string IPlatformFeatures.ParameterForAlwaysTrue => ChilloutVR.Params.VisemeIdx;
            string IPlatformFeatures.ResolveMirrorParameter(string virtualParameter, EmoteHand hand)
            {
                return (virtualParameter, hand) switch
                {
                    (Params.Gesture, EmoteHand.Left) => ChilloutVR.Params.GestureLeftIdx,
                    (Params.Gesture, EmoteHand.Right) => ChilloutVR.Params.GestureRightIdx,
                    (Params.GestureOther, EmoteHand.Left) => ChilloutVR.Params.GestureRightIdx,
                    (Params.GestureOther, EmoteHand.Right) => ChilloutVR.Params.GestureLeftIdx,
                    (Params.GestureWeight, EmoteHand.Left) => ChilloutVR.Params.GestureLeft,
                    (Params.GestureWeight, EmoteHand.Right) => ChilloutVR.Params.GestureRight,
                    (Params.GestureOtherWeight, EmoteHand.Left) => ChilloutVR.Params.GestureRight,
                    (Params.GestureOtherWeight, EmoteHand.Right) => ChilloutVR.Params.GestureLeft,
                    _ => virtualParameter
                };
            }

            Motion IPlatformFeatures.GestureClipIdleLeft => CvrCckAssetLocator.HandLeftRelaxed();
            Motion IPlatformFeatures.GestureClipFistLeft => CvrCckAssetLocator.HandLeftFist();
            Motion IPlatformFeatures.GestureClipOpenLeft => CvrCckAssetLocator.HandLeftOpen();
            Motion IPlatformFeatures.GestureClipPointLeft => CvrCckAssetLocator.HandLeftPoint();
            Motion IPlatformFeatures.GestureClipPeaceLeft => CvrCckAssetLocator.HandLeftPeace();
            Motion IPlatformFeatures.GestureClipRockNRollLeft => CvrCckAssetLocator.HandLeftRocknroll();
            Motion IPlatformFeatures.GestureClipGunLeft => CvrCckAssetLocator.HandLeftGun();
            Motion IPlatformFeatures.GestureClipThumbsUpLeft => CvrCckAssetLocator.HandLeftThumbsUp();
            Motion IPlatformFeatures.GestureClipIdleRight => CvrCckAssetLocator.HandRightRelaxed();
            Motion IPlatformFeatures.GestureClipFistRight => CvrCckAssetLocator.HandRightFist();
            Motion IPlatformFeatures.GestureClipOpenRight => CvrCckAssetLocator.HandRightOpen();
            Motion IPlatformFeatures.GestureClipPointRight => CvrCckAssetLocator.HandRightPoint();
            Motion IPlatformFeatures.GestureClipPeaceRight => CvrCckAssetLocator.HandRightPeace();
            Motion IPlatformFeatures.GestureClipRockNRollRight => CvrCckAssetLocator.HandRightRocknroll();
            Motion IPlatformFeatures.GestureClipGunRight => CvrCckAssetLocator.HandRightGun();
            Motion IPlatformFeatures.GestureClipThumbsUpRight => CvrCckAssetLocator.HandRightThumbsUp();

            AvatarMask IPlatformFeatures.HandLeft => CvrCckAssetLocator.GesturesLeft();
            AvatarMask IPlatformFeatures.HandRight => CvrCckAssetLocator.GesturesRight();

            int IPlatformFeatures.HandSignValue(HandSign handSign)
            {
                return handSign switch
                {
                    HandSign.Idle => 0,
                    HandSign.Fist => 1,
                    HandSign.Open => -1,
                    HandSign.Point => 4,
                    HandSign.Peace => 5,
                    HandSign.RockNRoll => 6,
                    HandSign.Gun => 3,
                    HandSign.ThumbsUp => 2,
                    _ => throw new ArgumentOutOfRangeException(nameof(handSign), handSign, null)
                };
            }

            List<ParameterInstance> IPlatformFeatures.DefaultParameters() => _defaultParameterDatabase.ToInstances();

            bool IPlatformFeatures.IsDefaultParameterReference(string parameterReference) => _defaultParameterDatabase.IsDefaultParameterReference(parameterReference);

            string IPlatformFeatures.ResolveParameterReference(string parameterReference) => _defaultParameterDatabase.ResolveParameterReference(parameterReference);

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultHandSign(EmoteTemplatePath path, EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind, HandSign handSign)
            {
                yield return DefaultEmoteItem.Default(this, layerKind, handSign)
                    .ToEmoteItemTemplate(path, emoteItemKind, emoteSequenceFactoryKind, layerKind);
            }

            IEnumerable<DefaultActionIndex> IPlatformFeatures.DefaultActionIndexes() =>
                Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                    .Where(index => DefaultActionEmoteChilloutVR.Default(index) is { })
                    .Concat(new[] { DefaultActionIndex.Afk });

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultAction(IPlatformFeatures platformFeatures, EmoteTemplatePath path, DefaultActionIndex index)
            {
                if (DefaultActionEmoteChilloutVR.Default(index) is { } defaultActionEmote)
                {
                    yield return defaultActionEmote.ToEmoteItemTemplate(platformFeatures, path);
                }
            }
        }
    }
}
