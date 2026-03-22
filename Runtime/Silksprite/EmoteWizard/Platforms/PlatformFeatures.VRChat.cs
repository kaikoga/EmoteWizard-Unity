using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;
using static Silksprite.EmoteWizard.EmoteWizardConstants;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        class VRChatFeatures : IPlatformFeatures
        {
            string IPlatformFeatures.ParameterForAlwaysTrue => Params.Viseme;
            string IPlatformFeatures.GestureLeft => "GestureLeft";
            string IPlatformFeatures.GestureLeftWeight => "GestureLeftWeight";
            string IPlatformFeatures.GestureRight => "GestureRight";
            string IPlatformFeatures.GestureRightWeight => "GestureRightWeight";

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
                    "GestureLeft" => true,
                    "GestureLeftWeight" => true,
                    "GestureRight" => true,
                    "GestureRightWeight" => true,
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
                return (int)handSign;
            }
            
            static readonly int[] Empty = {};

            static readonly (string reference, string name, ParameterItemKind kind, int[] states)[] DefaultParameterData = {
                ("IsLocal", "IsLocal", ParameterItemKind.Bool, Empty),
                (Params.Viseme, Params.Viseme, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
                ("Voice", "Voice", ParameterItemKind.Float, Empty),
                ("GestureLeft", "GestureLeft", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("GestureRight", "GestureRight", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("GestureLeftWeight", "GestureLeftWeight", ParameterItemKind.Float, Empty),
                ("GestureRightWeight", "GestureRightWeight", ParameterItemKind.Float, Empty),
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
                (Params.Gesture, Params.Gesture, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (Params.GestureOther, Params.GestureOther, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (Params.GestureWeight, Params.GestureWeight, ParameterItemKind.Float, Empty),
                (Params.GestureOtherWeight, Params.GestureOtherWeight, ParameterItemKind.Float, Empty),
            };


            List<ParameterInstance> IPlatformFeatures.DefaultParameters()
            {
                return DefaultParameterData.Select(tuple =>
                {
                    var (reference, name, kind, states) = tuple;
                    return new ParameterInstance
                    {
                        defaultValue = 0,
                        name = name,
                        saved = false,
                        itemKind = kind,
                        referenceUsages = new List<string> { reference },
                        writeUsages = states.Select(state => new ParameterWriteUsage(ParameterWriteUsageKind.Int, state, ParameterWriteSourceKind.NoUI)).ToList(),
                        readUsages = states.Select(state => new ParameterReadUsage(ParameterItemKind.Int, state)).ToList(),
                    };
                }).ToList();
            }

            bool IPlatformFeatures.IsDefaultParameterReference(string parameterReference)
            {
                return DefaultParameterData.Any(data => parameterReference == data.name);
            }
        }
    }
}
