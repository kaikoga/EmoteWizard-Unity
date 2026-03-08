using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    // FIXME
    public static class DefaultParametersCVR
    {
        static readonly int[] Empty = {};
        
        static readonly (string name, ParameterItemKind kind, int[] states)[] DefaultParameterData = {
            (EmoteWizardConstants.Params.Viseme, ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
            ("MovementX", ParameterItemKind.Float, Empty),
            ("MovementY", ParameterItemKind.Float, Empty),
            ("Grounded", ParameterItemKind.Bool, Empty),
            ("Emote", ParameterItemKind.Int, Empty),
            ("CancelEmote", ParameterItemKind.Bool, Empty),
            ("GestureLeft", ParameterItemKind.Float, Empty),
            ("GestureRight", ParameterItemKind.Float, Empty),
            ("Toggle", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
            ("Sitting", ParameterItemKind.Bool, Empty),
            ("Crouching", ParameterItemKind.Bool, Empty),
            ("Prone", ParameterItemKind.Bool, Empty),
            ("Flying", ParameterItemKind.Bool, Empty),
            ("Swimming", ParameterItemKind.Bool, Empty),
        };

        public static List<ParameterInstance> Populate()
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
                    writeUsages = states.Select(state => new ParameterWriteUsage(ParameterWriteUsageKind.Int, state)).ToList(),
                    readUsages = states.Select(state => new ParameterReadUsage(ParameterItemKind.Int, state)).ToList(),
                };
            }).ToList();
        }
    }
}