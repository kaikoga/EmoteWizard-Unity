using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using static Silksprite.EmoteWizard.EmoteWizardConstants;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public static partial class PlatformFeatures
    {
        class ChilloutVRFeatures : IPlatformFeatures
        {
            public string ParameterForAlwaysTrue => Params.VisemeIdx;
            string IPlatformFeatures.GestureLeft => "GestureLeftIdx";
            string IPlatformFeatures.GestureLeftWeight => "GestureLeft";
            string IPlatformFeatures.GestureRight => "GestureRightIdx";
            string IPlatformFeatures.GestureRightWeight => "GestureRight";

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
                ("GestureLeft", "GestureLeftIdx", ParameterItemKind.Int, new[]{-1, 0, 1, 2, 3, 4, 5, 6}),
                ("GestureRight", "GestureRightIdx", ParameterItemKind.Int, new[]{-1, 0, 1, 2, 3, 4, 5, 6}),
                ("Toggle", "Toggle", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
                ("Sitting", "Sitting", ParameterItemKind.Bool, Empty),
                ("Crouching", "Crouching", ParameterItemKind.Bool, Empty),
                ("Prone", "Prone", ParameterItemKind.Bool, Empty),
                ("Flying", "Flying", ParameterItemKind.Bool, Empty),
                ("Swimming", "Swimming", ParameterItemKind.Bool, Empty),
                (Params.AFK, Params.AFK, ParameterItemKind.Bool, Empty),
            };

            public List<ParameterInstance> DefaultParameters()
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

            public bool IsDefaultParameterReference(string parameterReference)
            {
                return DefaultParameterData.Any(data => parameterReference == data.reference);
            }
        }
    }
}
