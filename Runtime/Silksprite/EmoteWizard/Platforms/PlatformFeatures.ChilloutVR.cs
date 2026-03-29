using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Defaults;
using Silksprite.EmoteWizard.Platforms.Utils;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Wizards;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizard.EmoteWizardConstants;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        class ChilloutVRFeatures : IPlatformFeatures
        {
            string IPlatformFeatures.ParameterForAlwaysTrue => Params.VisemeIdx;
            string IPlatformFeatures.GestureLeft => "GestureLeftIdx";
            string IPlatformFeatures.GestureLeftWeight => "GestureLeft";
            string IPlatformFeatures.GestureRight => "GestureRightIdx";
            string IPlatformFeatures.GestureRightWeight => "GestureRight";

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

            bool IPlatformFeatures.IsHandSignParameterReference(string parameterReference) =>
                parameterReference switch
                {
                    "GestureLeft" => true,
                    "GestureLeftIdx" => true,
                    "GestureRight" => true,
                    "GestureRightIdx" => true,
                    Params.Gesture => true,
                    Params.GestureOther => true,
                    Params.GestureWeight => true,
                    Params.GestureOtherWeight => true,
                    _ => false
                };

            string IPlatformFeatures.ResolveParameterReference(string parameterReference)
            {
                return DefaultParameterData.FirstOrDefault(tuple => tuple.reference == parameterReference).name ?? parameterReference;
            }

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

            static readonly int[] Empty = {};
        
            static readonly (string reference, string name, ParameterItemKind kind, int[] states)[] DefaultParameterData = {
                (Params.Viseme, Params.VisemeIdx, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
                ("MovementX", "MovementX", ParameterItemKind.Float, Empty),
                ("MovementY", "MovementY", ParameterItemKind.Float, Empty),
                ("Grounded", "Grounded", ParameterItemKind.Bool, Empty),
                ("VRCEmote", "Emote", ParameterItemKind.Int, Empty),
                ("CancelEmote", "CancelEmote", ParameterItemKind.Bool, Empty),
                ("GestureLeftWeight", "GestureLeft", ParameterItemKind.Float, Empty),
                ("GestureRightWeight", "GestureRight", ParameterItemKind.Float, Empty),
                ("GestureLeft", "GestureLeftIdx", ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("GestureRight", "GestureRightIdx", ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("Toggle", "Toggle", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("Sitting", "Sitting", ParameterItemKind.Bool, Empty),
                ("Crouching", "Crouching", ParameterItemKind.Bool, Empty),
                ("Prone", "Prone", ParameterItemKind.Bool, Empty),
                ("Flying", "Flying", ParameterItemKind.Bool, Empty),
                ("Swimming", "Swimming", ParameterItemKind.Bool, Empty),
                (Params.Afk, Params.Afk, ParameterItemKind.Bool, Empty),

                // These are mirrored into appropriate CVR default parameters
                (Params.Gesture, Params.Gesture, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (Params.GestureOther, Params.GestureOther, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (Params.GestureWeight, Params.GestureWeight, ParameterItemKind.Float, Empty),
                (Params.GestureOtherWeight, Params.GestureOtherWeight, ParameterItemKind.Float, Empty),
            };

            List<ParameterInstance> IPlatformFeatures.DefaultParameters()
            {
                return DefaultParameterData.Select(tuple =>
                {
                    var (reference, name, kind, states) = tuple;
                    var writeUsageKind = kind switch
                    {
                        ParameterItemKind.Auto => ParameterWriteUsageKind.Int,
                        ParameterItemKind.Bool => ParameterWriteUsageKind.Int,
                        ParameterItemKind.Int => ParameterWriteUsageKind.Int,
                        ParameterItemKind.Float => ParameterWriteUsageKind.Int,
                        ParameterItemKind.HandSign => ParameterWriteUsageKind.HandSign,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    return new ParameterInstance(
                        name: name,
                        itemKind: kind,
                        saved: false,
                        defaultValue: ParameterValue.Default,
                        synced: true,
                        referenceUsages: new List<string> { reference },
                        writeUsages: states.Select(state => new ParameterWriteUsage(writeUsageKind, state, ParameterWriteSourceKind.NoUI)),
                        readUsages: states.Select(state => new ParameterReadUsage(ParameterValue.Create(kind, state))));
                }).ToList();
            }

            bool IPlatformFeatures.IsDefaultParameterReference(string parameterReference)
            {
                return DefaultParameterData.Any(data => parameterReference == data.reference);
            }

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultHandSign(EmoteTemplatePath path, EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind, HandSign handSign)
            {
                yield return DefaultEmoteItem.Default(this, layerKind, handSign)
                    .ToEmoteItemTemplate(path, emoteItemKind, emoteSequenceFactoryKind, layerKind);
            }

            IEnumerable<DefaultActionIndex> IPlatformFeatures.DefaultActionIndexes() =>
                Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                    .Where(index => DefaultActionEmoteChilloutVR.Default(index) is { })
                    .Concat(new[] { DefaultActionIndex.Afk });

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultAction(EmoteTemplatePath path, DefaultActionIndex index)
            {
                if (DefaultActionEmoteChilloutVR.Default(index) is { } defaultActionEmote)
                {
                    yield return defaultActionEmote.ToEmoteItemTemplate(path);
                }
            }
        }
    }
}
