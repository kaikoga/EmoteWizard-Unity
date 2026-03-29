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
        class VRChatFeatures : IPlatformFeatures
        {
            readonly IPlatformReferences _platformReferences;
            readonly DefaultParameterDatabase _defaultParameterDatabase;

            static DefaultParameterDatabase DefaultParameterDatabase(IPlatformReferences platformReferences)
            {
                var empty = Array.Empty<int>();

                return new DefaultParameterDatabase(new[]
                {
                    ("IsLocal", "IsLocal", ParameterItemKind.Bool, empty),
                    (platformReferences.VisemeReference, VRChat.Params.Viseme, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
                    ("Voice", "Voice", ParameterItemKind.Float, empty),
                    (platformReferences.GestureLeftReference, VRChat.Params.GestureLeft, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (platformReferences.GestureRightReference, VRChat.Params.GestureRight, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (platformReferences.GestureLeftWeightReference, VRChat.Params.GestureLeftWeight, ParameterItemKind.Float, empty),
                    (platformReferences.GestureRightWeightReference, VRChat.Params.GestureRightWeight, ParameterItemKind.Float, empty),
                    ("AngularY", "AngularY", ParameterItemKind.Float, empty),
                    ("VelocityX", "VelocityX", ParameterItemKind.Float, empty),
                    ("VelocityY", "VelocityY", ParameterItemKind.Float, empty),
                    ("VelocityZ", "VelocityZ", ParameterItemKind.Float, empty),
                    ("VelocityMagnitude", "VelocityMagnitude", ParameterItemKind.Float, empty),
                    ("Upright", "Upright", ParameterItemKind.Float, empty),
                    ("Grounded", "Grounded", ParameterItemKind.Bool, empty),
                    ("Seated", "Seated", ParameterItemKind.Bool, empty),
                    (Params.Afk, Params.Afk, ParameterItemKind.Bool, empty),
                    ("TrackingType", "TrackingType", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6}),
                    ("VRMode", "VRMode", ParameterItemKind.Int, new[]{0, 1}),
                    ("MuteSelf", "MuteSelf", ParameterItemKind.Bool, empty),
                    ("InStation", "InStation", ParameterItemKind.Bool, empty),
                    ("Earmuffs", "Earmuffs", ParameterItemKind.Bool, empty),
                    ("IsOnFriendList", "IsOnFriendList", ParameterItemKind.Bool, empty),
                    ("AvatarVersion", "AvatarVersion", ParameterItemKind.Int, new[]{0, 3}),
                    ("InStation", "InStation", ParameterItemKind.Bool, empty),
                    ("ScaleModified", "ScaleModified", ParameterItemKind.Bool, empty),
                    ("ScaleFactor", "ScaleFactor", ParameterItemKind.Float, empty),
                    ("ScaleFactorInverse", "ScaleFactorInverse", ParameterItemKind.Float, empty),
                    ("EyeHeightAsMeters", "EyeHeightAsMeters", ParameterItemKind.Float, empty),
                    ("EyeHeightAsPercent", "EyeHeightAsPercent", ParameterItemKind.Float, empty),

                    // These are mirrored into appropriate VRC default parameters
                    (Params.Gesture, Params.Gesture, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (Params.GestureOther, Params.GestureOther, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                    (Params.GestureWeight, Params.GestureWeight, ParameterItemKind.Float, empty),
                    (Params.GestureOtherWeight, Params.GestureOtherWeight, ParameterItemKind.Float, empty),
                });
            }

            public VRChatFeatures(IPlatformReferences platformReferences)
            {
                _platformReferences = platformReferences;
                _defaultParameterDatabase = DefaultParameterDatabase(platformReferences);
            }

            string IPlatformFeatures.ParameterReferenceForActionSelect => _platformReferences.ActionSelectReference;
            string IPlatformFeatures.ParameterForAlwaysTrue => VRChat.Params.Viseme;
            string IPlatformFeatures.ResolveMirrorParameter(string virtualParameter, EmoteHand hand)
            {
                return (virtualParameter, hand) switch
                {
                    (Params.Gesture, EmoteHand.Left) => VRChat.Params.GestureLeft,
                    (Params.Gesture, EmoteHand.Right) => VRChat.Params.GestureRight,
                    (Params.GestureOther, EmoteHand.Left) => VRChat.Params.GestureRight,
                    (Params.GestureOther, EmoteHand.Right) => VRChat.Params.GestureLeft,
                    (Params.GestureWeight, EmoteHand.Left) => VRChat.Params.GestureLeftWeight,
                    (Params.GestureWeight, EmoteHand.Right) => VRChat.Params.GestureRightWeight,
                    (Params.GestureOtherWeight, EmoteHand.Left) => VRChat.Params.GestureRightWeight,
                    (Params.GestureOtherWeight, EmoteHand.Right) => VRChat.Params.GestureLeftWeight,
                    _ => virtualParameter
                };
            }

            Motion IPlatformFeatures.GestureClipIdleLeft => VrcSdkAssetLocator.ProxyHandsIdle();
            Motion IPlatformFeatures.GestureClipFistLeft => VrcSdkAssetLocator.ProxyHandsFist();
            Motion IPlatformFeatures.GestureClipOpenLeft => VrcSdkAssetLocator.ProxyHandsOpen();
            Motion IPlatformFeatures.GestureClipPointLeft => VrcSdkAssetLocator.ProxyHandsPoint();
            Motion IPlatformFeatures.GestureClipPeaceLeft => VrcSdkAssetLocator.ProxyHandsPeace();
            Motion IPlatformFeatures.GestureClipRockNRollLeft => VrcSdkAssetLocator.ProxyHandsRock();
            Motion IPlatformFeatures.GestureClipGunLeft => VrcSdkAssetLocator.ProxyHandsGun();
            Motion IPlatformFeatures.GestureClipThumbsUpLeft => VrcSdkAssetLocator.ProxyHandsThumbsUp();
            Motion IPlatformFeatures.GestureClipIdleRight => VrcSdkAssetLocator.ProxyHandsIdle();
            Motion IPlatformFeatures.GestureClipFistRight => VrcSdkAssetLocator.ProxyHandsFist();
            Motion IPlatformFeatures.GestureClipOpenRight => VrcSdkAssetLocator.ProxyHandsOpen();
            Motion IPlatformFeatures.GestureClipPointRight => VrcSdkAssetLocator.ProxyHandsPoint();
            Motion IPlatformFeatures.GestureClipPeaceRight => VrcSdkAssetLocator.ProxyHandsPeace();
            Motion IPlatformFeatures.GestureClipRockNRollRight => VrcSdkAssetLocator.ProxyHandsRock();
            Motion IPlatformFeatures.GestureClipGunRight => VrcSdkAssetLocator.ProxyHandsGun();
            Motion IPlatformFeatures.GestureClipThumbsUpRight => VrcSdkAssetLocator.ProxyHandsThumbsUp();

            AvatarMask IPlatformFeatures.HandLeft => VrcSdkAssetLocator.HandLeft();
            AvatarMask IPlatformFeatures.HandRight => VrcSdkAssetLocator.HandRight();

            int IPlatformFeatures.HandSignValue(HandSign handSign)
            {
                return (int)handSign;
            }
            
            List<ParameterInstance> IPlatformFeatures.DefaultParameters() => _defaultParameterDatabase.ToInstances();

            bool IPlatformFeatures.IsDefaultParameterReference(string parameterReference) => _defaultParameterDatabase.IsDefaultParameterReference(parameterReference);

            string IPlatformFeatures.ResolveParameterReference(string parameterReference) => _defaultParameterDatabase.ResolveParameterReference(parameterReference);

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultHandSign(EmoteTemplatePath path, EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind, HandSign handSign)
            {
                yield return DefaultEmoteItem.Default(this, layerKind, handSign)
                    .ToEmoteItemTemplate(path, emoteItemKind, emoteSequenceFactoryKind, layerKind);
            }

            public virtual IEnumerable<DefaultActionIndex> DefaultActionIndexes() =>
                Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                    .Where(index => DefaultActionEmoteVRChat.Default(index) is { })
                    .Concat(new[] { DefaultActionIndex.Afk });

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultAction(IPlatformFeatures platformFeatures, EmoteTemplatePath path, DefaultActionIndex index)
            {
                if (DefaultActionEmoteVRChat.Default(index) is { } defaultActionEmote)
                {
                    yield return defaultActionEmote.ToEmoteItemTemplate(platformFeatures, path);
                }
            }
        }
    }
}
