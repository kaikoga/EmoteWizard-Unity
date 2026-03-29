using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Defaults;
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
            string IPlatformFeatures.ParameterForAlwaysTrue => VRChat.Params.Viseme;
            string IPlatformFeatures.GestureLeft => VRChat.Params.GestureLeft;
            string IPlatformFeatures.GestureLeftWeight => VRChat.Params.GestureLeftWeight;
            string IPlatformFeatures.GestureRight => VRChat.Params.GestureRight;
            string IPlatformFeatures.GestureRightWeight => VRChat.Params.GestureRightWeight;

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

            bool IPlatformFeatures.IsHandSignParameterReference(string parameterReference) =>
                parameterReference switch
                {
                    VRChat.Params.GestureLeft => true,
                    VRChat.Params.GestureLeftWeight => true,
                    VRChat.Params.GestureRight => true,
                    VRChat.Params.GestureRightWeight => true,
                    Params.Gesture => true,
                    Params.GestureOther => true,
                    Params.GestureWeight => true,
                    Params.GestureOtherWeight => true,
                    _ => false
                };

            int IPlatformFeatures.HandSignValue(HandSign handSign)
            {
                return (int)handSign;
            }
            
            static readonly int[] Empty = {};

            static readonly DefaultParameterDatabase DefaultParameterDatabase = new DefaultParameterDatabase(new[]
            {
                ("IsLocal", "IsLocal", ParameterItemKind.Bool, Empty),
                (VRChat.Params.Viseme, VRChat.Params.Viseme, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
                ("Voice", "Voice", ParameterItemKind.Float, Empty),
                (VRChat.Params.GestureLeft, VRChat.Params.GestureLeft, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (VRChat.Params.GestureRight, VRChat.Params.GestureRight, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (VRChat.Params.GestureLeftWeight, VRChat.Params.GestureLeftWeight, ParameterItemKind.Float, Empty),
                (VRChat.Params.GestureRightWeight, VRChat.Params.GestureRightWeight, ParameterItemKind.Float, Empty),
                ("AngularY", "AngularY", ParameterItemKind.Float, Empty),
                ("VelocityX", "VelocityX", ParameterItemKind.Float, Empty),
                ("VelocityY", "VelocityY", ParameterItemKind.Float, Empty),
                ("VelocityZ", "VelocityZ", ParameterItemKind.Float, Empty),
                ("VelocityMagnitude", "VelocityMagnitude", ParameterItemKind.Float, Empty),
                ("Upright", "Upright", ParameterItemKind.Float, Empty),
                ("Grounded", "Grounded", ParameterItemKind.Bool, Empty),
                ("Seated", "Seated", ParameterItemKind.Bool, Empty),
                (Params.Afk, Params.Afk, ParameterItemKind.Bool, Empty),
                ("TrackingType", "TrackingType", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6}),
                ("VRMode", "VRMode", ParameterItemKind.Int, new[]{0, 1}),
                ("MuteSelf", "MuteSelf", ParameterItemKind.Bool, Empty),
                ("InStation", "InStation", ParameterItemKind.Bool, Empty),
                ("Earmuffs", "Earmuffs", ParameterItemKind.Bool, Empty),
                ("IsOnFriendList", "IsOnFriendList", ParameterItemKind.Bool, Empty),
                ("AvatarVersion", "AvatarVersion", ParameterItemKind.Int, new[]{0, 3}),
                ("InStation", "InStation", ParameterItemKind.Bool, Empty),
                ("ScaleModified", "ScaleModified", ParameterItemKind.Bool, Empty),
                ("ScaleFactor", "ScaleFactor", ParameterItemKind.Float, Empty),
                ("ScaleFactorInverse", "ScaleFactorInverse", ParameterItemKind.Float, Empty),
                ("EyeHeightAsMeters", "EyeHeightAsMeters", ParameterItemKind.Float, Empty),
                ("EyeHeightAsPercent", "EyeHeightAsPercent", ParameterItemKind.Float, Empty),

                // These are mirrored into appropriate VRC default parameters
                (Params.Gesture, Params.Gesture, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (Params.GestureOther, Params.GestureOther, ParameterItemKind.HandSign, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (Params.GestureWeight, Params.GestureWeight, ParameterItemKind.Float, Empty),
                (Params.GestureOtherWeight, Params.GestureOtherWeight, ParameterItemKind.Float, Empty),
            });


            List<ParameterInstance> IPlatformFeatures.DefaultParameters() => DefaultParameterDatabase.ToInstances();

            bool IPlatformFeatures.IsDefaultParameterReference(string parameterReference) => DefaultParameterDatabase.IsDefaultParameterReference(parameterReference);

            string IPlatformFeatures.ResolveParameterReference(string parameterReference) => DefaultParameterDatabase.ResolveParameterReference(parameterReference);

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultHandSign(EmoteTemplatePath path, EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind, HandSign handSign)
            {
                yield return DefaultEmoteItem.Default(this, layerKind, handSign)
                    .ToEmoteItemTemplate(path, emoteItemKind, emoteSequenceFactoryKind, layerKind);
            }

            public virtual IEnumerable<DefaultActionIndex> DefaultActionIndexes() =>
                Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                    .Where(index => DefaultActionEmoteVRChat.Default(index) is { })
                    .Concat(new[] { DefaultActionIndex.Afk });

            IEnumerable<IEmoteTemplate> IPlatformFeatures.UnpackDefaultAction(EmoteTemplatePath path, DefaultActionIndex index)
            {
                if (DefaultActionEmoteVRChat.Default(index) is { } defaultActionEmote)
                {
                    yield return defaultActionEmote.ToEmoteItemTemplate(path);
                }
            }
        }
    }
}
