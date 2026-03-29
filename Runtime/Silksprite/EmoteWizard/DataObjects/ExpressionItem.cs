using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardTools;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ExpressionItem
    {
        [SerializeField] public bool enabled = true;
        [SerializeField] public Texture2D? icon;
        [ItemPath]
        [SerializeField] public string path = "";
        [ParameterName(true, true)]
        [SerializeField] public string parameter = "";
        [SerializeField] public float value;
        [SerializeField] public ExpressionItemKind itemKind;
#if EW_VRCSDK3_AVATARS
        [SerializeField] public VRCExpressionsMenu? subMenu;
#else
        [SerializeField] public ScriptableObject? subMenu;
#endif
        [ParameterName(false, true)]
        [SerializeField] public string[] subParameters = { };
        [SerializeField] public string[] labels = { };
        [SerializeField] public Texture2D[] labelIcons = { };

        public bool IsValid
        {
            get
            {
                if (ItemPathUtil.IsInvalidPathFormat(path)) return false;
                if (ParameterNameUtil.IsInvalidParameterFormat(parameter, true)) return false;
                if (subParameters.Take(itemKind).Any(sub => ParameterNameUtil.IsInvalidParameterFormat(sub, false))) return false; 
                return true;
            }
        }

        public string Name => GetFileName(path);
        public string? Folder => GetDirectoryName(path);

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

        public IEnumerable<string> FoldersIncludesRoot()
        {
            var p = path;
            while (!string.IsNullOrEmpty(p))
            {
                p = GetDirectoryName(p) ?? "";
                yield return p;
            }
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
    }
}