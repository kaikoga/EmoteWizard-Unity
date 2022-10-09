using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public static class DefaultParameters
    {
        static readonly int[] Empty = {};

        static readonly (string, ParameterItemKind Int, int[])[] DefaultParameterData = {
            ("IsLocal", ParameterItemKind.Bool, Empty),
            ("Viseme", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
            ("GestureLeft", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
            ("GestureRight", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
            ("GestureLeftWeight", ParameterItemKind.Float, Empty),
            ("GestureRightWeight", ParameterItemKind.Float, Empty),
            ("AngularY", ParameterItemKind.Float, Empty),
            ("VelocityX", ParameterItemKind.Float, Empty),
            ("VelocityY", ParameterItemKind.Float, Empty),
            ("VelocityX", ParameterItemKind.Float, Empty),
            ("Upright", ParameterItemKind.Float, Empty),
            ("Grounded", ParameterItemKind.Bool, Empty),
            ("Seated", ParameterItemKind.Bool, Empty),
            ("AFK", ParameterItemKind.Bool, Empty),
            ("TrackingType", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 6}),
            ("VRMode", ParameterItemKind.Int, Empty),
            ("MuteSelf", ParameterItemKind.Bool, Empty),
            ("InStation", ParameterItemKind.Bool, Empty)
        };

        public static List<ParameterInstance> Populate()
        {
            return DefaultParameterData.Select(tuple =>
            {
                var (name, kind, states) = tuple;
                return new ParameterInstance
                {
                    DefaultValue = 0,
                    Name = name,
                    Saved = false,
                    ItemKind = kind,
                    WriteUsages = states.Select(state => new ParameterWriteUsage(ParameterWriteUsageKind.Int, state)).ToList()
                };
            }).ToList();
        }
    }
}