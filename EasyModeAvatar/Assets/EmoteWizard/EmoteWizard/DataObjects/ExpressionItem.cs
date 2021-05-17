using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ExpressionItem
    {
        [SerializeField] public ExpressionItemKind kind;
        [SerializeField] public Texture2D icon;
        [SerializeField] public string path;
        [SerializeField] public string parameter;
        [SerializeField] public int value;
        [SerializeField] public string subMenu;
        [SerializeField] public VRCExpressionsMenu.Control.ControlType controlType;

        public string Name => Path.GetFileName(path);
        public string Folder => Path.GetDirectoryName(path);

        public IEnumerable<string> Folders()
        {
            var p = path;
            while (!string.IsNullOrEmpty(p))
            {
                p = Path.GetDirectoryName(p);
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

        public static ExpressionItem PopulateDefault(Texture2D icon, int value)
        {
            return new ExpressionItem
            {
                kind = ExpressionItemKind.Default,
                icon = icon,
                path = NameForDefaultEmote(value),
                parameter = "VRC_emote",
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
                controlType = VRCExpressionsMenu.Control.ControlType.SubMenu,
                subMenu = path
            };
        }
        
        public VRCExpressionsMenu.Control ToControl(Func<string, VRCExpressionsMenu> subMenuResolver)
        {
            switch (kind)
            {
                case ExpressionItemKind.Default:
                    return new VRCExpressionsMenu.Control
                    {
                        icon = icon,
                        labels = new VRCExpressionsMenu.Control.Label[] { },
                        name = Name,
                        parameter = new VRCExpressionsMenu.Control.Parameter { name = parameter },
                        style = VRCExpressionsMenu.Control.Style.Style1,
                        subMenu = null,
                        subParameters = new VRCExpressionsMenu.Control.Parameter[] { },
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
                        subMenu = subMenuResolver(subMenu),
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