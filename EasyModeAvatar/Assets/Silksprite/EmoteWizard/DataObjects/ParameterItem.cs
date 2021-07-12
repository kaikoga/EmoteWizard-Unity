using System;
using System.Collections.Generic;
using System.Linq;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public bool enabled;
        public string name;
        public ParameterItemKind itemKind;
        public bool saved = true;
        public float defaultValue;
        public List<ParameterUsage> usages;

        public ParameterValueKind ValueKind
        {
            get
            {
                switch (itemKind)
                {
                    case ParameterItemKind.Auto:
                        if (usages.Any(usage => usage.usageKind == ParameterUsageKind.Float)) return ParameterValueKind.Float;
                        return usages.Count(usage => usage.usageKind != ParameterUsageKind.Default) > 1 ? ParameterValueKind.Int : ParameterValueKind.Bool;
                    case ParameterItemKind.Bool:
                        return ParameterValueKind.Bool;
                    case ParameterItemKind.Int:
                        return ParameterValueKind.Int;
                    case ParameterItemKind.Float:
                        return ParameterValueKind.Float;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public VRCExpressionParameters.ValueType VrcValueType
        {
            get
            {
                switch (ValueKind)
                {
                    case ParameterValueKind.Bool:
                        return VRCExpressionParameters.ValueType.Bool;
                    case ParameterValueKind.Int:
                        return VRCExpressionParameters.ValueType.Int;
                    case ParameterValueKind.Float:
                        return VRCExpressionParameters.ValueType.Float;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        static int[] _empty = {};
        static (string, ParameterItemKind Int, int[])[] _defaultParameters = {
            ("IsLocal", ParameterItemKind.Bool, _empty),
            ("Viseme", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}),
            ("GestureLeft", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
            ("GestureRight", ParameterItemKind.Float, _empty),
            ("GestureLeftWeight", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 5, 6, 7}),
            ("GestureRightWeight", ParameterItemKind.Float, _empty),
            ("AngularY", ParameterItemKind.Float, _empty),
            ("VelocityX", ParameterItemKind.Float, _empty),
            ("VelocityY", ParameterItemKind.Float, _empty),
            ("VelocityX", ParameterItemKind.Float, _empty),
            ("Upright", ParameterItemKind.Float, _empty),
            ("Grounded", ParameterItemKind.Bool, _empty),
            ("Seated", ParameterItemKind.Bool, _empty),
            ("AFK", ParameterItemKind.Bool, _empty),
            ("TrackingType", ParameterItemKind.Int, new[]{0, 1, 2, 3, 4, 6}),
            ("VRMode", ParameterItemKind.Int, _empty),
            ("MuteSelf", ParameterItemKind.Bool, _empty),
            ("InStation", ParameterItemKind.Bool, _empty)
        };

        public static List<ParameterItem> PopulateDefaultParameters(List<ParameterItem> oldItems)
        {
            return _defaultParameters.Select(tuple =>
            {
                var (name, kind, states) = tuple;
                return new ParameterItem
                {
                    enabled = oldItems.FirstOrDefault(item => item.name == name)?.enabled ?? false,
                    defaultValue = 0,
                    name = name,
                    saved = false,
                    itemKind = kind,
                    usages = states.Select(state => new ParameterUsage(ParameterUsageKind.Int, state)).ToList()
                };
            }).ToList();
        }

        public VRCExpressionParameters.Parameter ToParameter()
        {
            return new VRCExpressionParameters.Parameter
            {
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                valueType = VrcValueType
            };
        }
    }
}