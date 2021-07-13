using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardTools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ExpressionItem
    {
        [SerializeField] public Texture2D icon;
        [SerializeField] public string path;
        [SerializeField] public string parameter;
        [SerializeField] public float value;
        [SerializeField] public ExpressionItemKind itemKind;
        [SerializeField] public VRCExpressionsMenu subMenu;
        [SerializeField] public string[] subParameters;
        [SerializeField] public string[] labels;
        [SerializeField] public Texture2D[] labelIcons;

        public string Name => GetFileName(path);
        public string Folder => GetDirectoryName(path);

        public bool IsPuppet
        {
            get
            {
                switch (itemKind)
                {
                    case ExpressionItemKind.Button:
                    case ExpressionItemKind.Toggle:
                    case ExpressionItemKind.SubMenu:
                        return false;
                    case ExpressionItemKind.TwoAxisPuppet:
                    case ExpressionItemKind.FourAxisPuppet:
                    case ExpressionItemKind.RadialPuppet:
                        return true;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public IEnumerable<string> Folders()
        {
            var p = path;
            while (!string.IsNullOrEmpty(p))
            {
                p = GetDirectoryName(p);
                yield return p;
            }
        }

        static string NameForDefaultEmote(int value)
        {
            switch (value)
            {
                case 1: return "Wave";
                case 2: return "Clap";
                case 3: return "Point";
                case 4: return "Cheer";
                case 5: return "Dance";
                case 6: return "Backflip";
                case 7: return "SadKick";
                case 8: return "Die";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        static ExpressionItemKind ItemKindForDefaultEmote(int value)
        {
            switch (value)
            {
                case 2:
                case 4:
                case 5:
                case 8:
                    return ExpressionItemKind.Toggle;
                default:
                    return ExpressionItemKind.Button;
            }
        }

        public static ExpressionItem PopulateDefault(Texture2D icon, string prefix, int value)
        {
            return new ExpressionItem
            {
                icon = icon,
                path = $"{prefix}{NameForDefaultEmote(value)}",
                parameter = "VRCEmote",
                value = value,
                itemKind = ItemKindForDefaultEmote(value),
            };
        }
        
        public static ExpressionItem PopulateFolder(Texture2D icon, string path)
        {
            return new ExpressionItem
            {
                icon = icon,
                path = path,
                itemKind = ExpressionItemKind.SubMenu
            };
        }
        
        public VRCExpressionsMenu.Control ToControl(Func<string, VRCExpressionsMenu> subMenuResolver)
        {
            VRCExpressionsMenu.Control.Parameter[] ToSubParameters()
            {
                switch (itemKind)
                {
                    case ExpressionItemKind.Button:
                    case ExpressionItemKind.Toggle:
                    case ExpressionItemKind.SubMenu:
                        return new VRCExpressionsMenu.Control.Parameter[] { };
                    case ExpressionItemKind.TwoAxisPuppet:
                    case ExpressionItemKind.FourAxisPuppet:
                    case ExpressionItemKind.RadialPuppet:
                        return subParameters.Select(subParameter => new VRCExpressionsMenu.Control.Parameter
                        {
                            name = subParameter
                        }).ToArray();
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            VRCExpressionsMenu ToSubMenu()
            {
                if (itemKind != ExpressionItemKind.SubMenu) return null;
                return subMenuResolver(path) ?? subMenu;
            }

            VRCExpressionsMenu.Control.Label[] ToLabels()
            {
                switch (itemKind)
                {
                    case ExpressionItemKind.Button:
                    case ExpressionItemKind.Toggle:
                    case ExpressionItemKind.SubMenu:
                    case ExpressionItemKind.RadialPuppet:
                        return new VRCExpressionsMenu.Control.Label[] { };
                    case ExpressionItemKind.TwoAxisPuppet:
                    case ExpressionItemKind.FourAxisPuppet:
                        return labels.Zip(labelIcons, (label, labelIcon) => new VRCExpressionsMenu.Control.Label
                        {
                            icon = labelIcon,
                            name = label
                        }).ToArray();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            VRCExpressionsMenu.Control.ControlType ToType()
            {
                switch (itemKind)
                {
                    case ExpressionItemKind.Button:
                        return VRCExpressionsMenu.Control.ControlType.Button;
                    case ExpressionItemKind.Toggle:
                        return VRCExpressionsMenu.Control.ControlType.Toggle;
                    case ExpressionItemKind.SubMenu:
                        return VRCExpressionsMenu.Control.ControlType.SubMenu;
                    case ExpressionItemKind.TwoAxisPuppet:
                        return VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet;
                    case ExpressionItemKind.FourAxisPuppet:
                        return VRCExpressionsMenu.Control.ControlType.FourAxisPuppet;
                    case ExpressionItemKind.RadialPuppet:
                        return VRCExpressionsMenu.Control.ControlType.RadialPuppet;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new VRCExpressionsMenu.Control
            {
                icon = icon,
                labels = ToLabels(),
                name = Name,
                parameter = new VRCExpressionsMenu.Control.Parameter { name = parameter },
                style = VRCExpressionsMenu.Control.Style.Style1,
                subMenu = ToSubMenu(),
                subParameters = ToSubParameters(),
                type = ToType(),
                value = value
            };
        }
    }
}