using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public static partial class PlatformFeatures
    {
        class VRChatFeatures : IPlatformFeatures
        {
            static readonly int[] Empty = {};

            static readonly (string name, ParameterItemKind kind, int[] states)[] DefaultParameterData = {
                ("IsLocal", ParameterItemKind.Bool, Empty),
                (EmoteWizardConstants.Params.Viseme, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
                ("Voice", ParameterItemKind.Float, Empty),
                ("GestureLeft", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("GestureRight", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("GestureLeftWeight", ParameterItemKind.Float, Empty),
                ("GestureRightWeight", ParameterItemKind.Float, Empty),
                ("AngularY", ParameterItemKind.Float, Empty),
                ("VelocityX", ParameterItemKind.Float, Empty),
                ("VelocityY", ParameterItemKind.Float, Empty),
                ("VelocityZ", ParameterItemKind.Float, Empty),
                ("VelocityMagnitude", ParameterItemKind.Float, Empty),
                ("Upright", ParameterItemKind.Float, Empty),
                ("Grounded", ParameterItemKind.Bool, Empty),
                ("Seated", ParameterItemKind.Bool, Empty),
                (EmoteWizardConstants.Params.AFK, ParameterItemKind.Bool, Empty),
                ("TrackingType", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6}),
                ("VRMode", ParameterItemKind.Int, new[]{0, 1}),
                ("MuteSelf", ParameterItemKind.Bool, Empty),
                ("InStation", ParameterItemKind.Bool, Empty),
                ("Earmuffs", ParameterItemKind.Bool, Empty),
                ("IsOnFriendList", ParameterItemKind.Bool, Empty),
                ("AvatarVersion", ParameterItemKind.Int, new[]{0, 3}),
                ("InStation", ParameterItemKind.Bool, Empty),
                ("ScaleModified", ParameterItemKind.Bool, Empty),
                ("ScaleFactor", ParameterItemKind.Float, Empty),
                ("ScaleFactorInverse", ParameterItemKind.Float, Empty),
                ("EyeHeightAsMeters", ParameterItemKind.Float, Empty),
                ("EyeHeightAsPercent", ParameterItemKind.Float, Empty),

                // These are mirrored into appropriate VRC default parameters
                (EmoteWizardConstants.Params.Gesture, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (EmoteWizardConstants.Params.GestureOther, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                (EmoteWizardConstants.Params.GestureWeight, ParameterItemKind.Float, Empty),
                (EmoteWizardConstants.Params.GestureOtherWeight, ParameterItemKind.Float, Empty),
            };

            public string ParameterForAlwaysTrue => EmoteWizardConstants.Params.Viseme;

            public List<ParameterInstance> Populate()
            {
                return DefaultParameterData.Select(tuple =>
                {
                    var (name, kind, states) = tuple;
                    return new ParameterInstance
                    {
                        defaultValue = 0,
                        name = name,
                        saved = false,
                        itemKind = kind,
                        writeUsages = states.Select(state => new ParameterWriteUsage(ParameterWriteUsageKind.Int, state, ParameterWriteSourceKind.NoUI)).ToList(),
                        readUsages = states.Select(state => new ParameterReadUsage(ParameterItemKind.Int, state)).ToList(),
                    };
                }).ToList();
            }

            public bool IsDefaultParameter(string parameter)
            {
                return DefaultParameterData.Any(data => parameter == data.name);
            }
        }
    }
}
