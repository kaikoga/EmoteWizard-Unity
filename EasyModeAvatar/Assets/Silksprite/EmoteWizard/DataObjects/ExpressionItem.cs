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
        [SerializeField] public ExpressionItemKind kind;
        [SerializeField] public Texture2D icon;
        [SerializeField] public string path;
        [SerializeField] public string parameter;
        [SerializeField] public float value;
        [SerializeField] public VRCExpressionsMenu.Control.ControlType controlType;
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
                switch (controlType)
                {
                    case VRCExpressionsMenu.Control.ControlType.Button:
                    case VRCExpressionsMenu.Control.ControlType.Toggle:
                    case VRCExpressionsMenu.Control.ControlType.SubMenu:
                        return false;
                    case VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet:
                    case VRCExpressionsMenu.Control.ControlType.FourAxisPuppet:
                    case VRCExpressionsMenu.Control.ControlType.RadialPuppet:
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

        static VRCExpressionsMenu.Control.ControlType ControlTypeForDefaultEmote(int value)
        {
            switch (value)
            {
                case 2:
                case 4:
                case 5:
                case 8:
                    return VRCExpressionsMenu.Control.ControlType.Toggle;
                default:
                    return VRCExpressionsMenu.Control.ControlType.Button;
            }
        }

        public static ExpressionItem PopulateDefault(Texture2D icon, string prefix, int value)
        {
            return new ExpressionItem
            {
                kind = ExpressionItemKind.Default,
                icon = icon,
                path = $"{prefix}{NameForDefaultEmote(value)}",
                parameter = "VRCEmote",
                value = value,
                controlType = ControlTypeForDefaultEmote(value),
            };
        }
        
        public static ExpressionItem PopulateFolder(Texture2D icon, string path)
        {
            return new ExpressionItem
            {
                kind = ExpressionItemKind.Folder,
                icon = icon,
                path = path,
                controlType = VRCExpressionsMenu.Control.ControlType.SubMenu
            };
        }
        
        public VRCExpressionsMenu.Control ToControl(Func<string, VRCExpressionsMenu> subMenuResolver)
        {
            VRCExpressionsMenu.Control.Parameter[] ToSubParameters()
            {
                switch (controlType)
                {
                    case VRCExpressionsMenu.Control.ControlType.Button:
                    case VRCExpressionsMenu.Control.ControlType.Toggle:
                    case VRCExpressionsMenu.Control.ControlType.SubMenu:
                        return new VRCExpressionsMenu.Control.Parameter[] { };
                    case VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet:
                    case VRCExpressionsMenu.Control.ControlType.FourAxisPuppet:
                    case VRCExpressionsMenu.Control.ControlType.RadialPuppet:
                        return subParameters.Select(subParameter => new VRCExpressionsMenu.Control.Parameter
                        {
                            name = subParameter
                        }).ToArray();
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            VRCExpressionsMenu.Control.Label[] ToLabels()
            {
                switch (controlType)
                {
                    case VRCExpressionsMenu.Control.ControlType.Button:
                    case VRCExpressionsMenu.Control.ControlType.Toggle:
                    case VRCExpressionsMenu.Control.ControlType.SubMenu:
                    case VRCExpressionsMenu.Control.ControlType.RadialPuppet:
                        return new VRCExpressionsMenu.Control.Label[] { };
                    case VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet:
                    case VRCExpressionsMenu.Control.ControlType.FourAxisPuppet:
                        return labels.Zip(labelIcons, (label, labelIcon) => new VRCExpressionsMenu.Control.Label
                        {
                            icon = labelIcon,
                            name = label
                        }).ToArray();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            switch (kind)
            {
                case ExpressionItemKind.Default:
                    return new VRCExpressionsMenu.Control
                    {
                        icon = icon,
                        labels = ToLabels(), 
                        name = Name,
                        parameter = new VRCExpressionsMenu.Control.Parameter { name = parameter },
                        style = VRCExpressionsMenu.Control.Style.Style1,
                        subMenu = controlType == VRCExpressionsMenu.Control.ControlType.SubMenu ? subMenu : null,
                        subParameters = ToSubParameters(),
                        type = controlType,
                        value = value
                    };
                case ExpressionItemKind.Folder:
                    return new VRCExpressionsMenu.Control
                    {
                        icon = icon,
                        labels = new VRCExpressionsMenu.Control.Label[] { },
                        name = Name,
                        parameter = new VRCExpressionsMenu.Control.Parameter { name = parameter },
                        style = VRCExpressionsMenu.Control.Style.Style1,
                        subMenu = subMenuResolver(path),
                        subParameters = new VRCExpressionsMenu.Control.Parameter[] { },
                        type = controlType,
                        value = value
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}