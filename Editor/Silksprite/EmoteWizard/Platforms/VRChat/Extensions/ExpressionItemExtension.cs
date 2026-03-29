using System;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Extensions;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Extensions
{
    public static class ExpressionItemExtension
    {
        public static VRCExpressionsMenu.Control ToControl(this ExpressionItem expressionItem, EmoteWizardEnvironment environment, Func<string, VRCExpressionsMenu?> subMenuResolver)
        {
            VRCExpressionsMenu.Control.Parameter[] ToSubParameters()
            {
                switch (expressionItem.itemKind)
                {
                    case ExpressionItemKind.Button:
                    case ExpressionItemKind.Toggle:
                    case ExpressionItemKind.SubMenu:
                        return new VRCExpressionsMenu.Control.Parameter[] { };
                    case ExpressionItemKind.TwoAxisPuppet:
                    case ExpressionItemKind.FourAxisPuppet:
                    case ExpressionItemKind.RadialPuppet:
                        return expressionItem.subParameters.Take(expressionItem.itemKind).Select(subParameter => new VRCExpressionsMenu.Control.Parameter
                        {
                            name = subParameter
                        }).ToArray();
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            VRCExpressionsMenu? ToSubMenu()
            {
                if (expressionItem.itemKind != ExpressionItemKind.SubMenu) return null;
                return subMenuResolver(expressionItem.path) ?? expressionItem.subMenu;
            }

            VRCExpressionsMenu.Control.Label[] ToLabels()
            {
                switch (expressionItem.itemKind)
                {
                    case ExpressionItemKind.Button:
                    case ExpressionItemKind.Toggle:
                    case ExpressionItemKind.SubMenu:
                    case ExpressionItemKind.RadialPuppet:
                        return new VRCExpressionsMenu.Control.Label[] { };
                    case ExpressionItemKind.TwoAxisPuppet:
                    case ExpressionItemKind.FourAxisPuppet:
                        return expressionItem.labels.Zip(expressionItem.labelIcons, (label, labelIcon) => new VRCExpressionsMenu.Control.Label
                        {
                            icon = labelIcon,
                            name = label
                        }).Take(expressionItem.itemKind).ToArray();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            VRCExpressionsMenu.Control.ControlType ToType()
            {
                switch (expressionItem.itemKind)
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

            var hasParameter = expressionItem.TryGetParameter(out var parameter);

            var value =
                hasParameter
                && environment.GetContext<ParametersContext>().Snapshot()
                    .TryResolveParameterWithTypeAndWarning(parameter!, ParameterItemKind.Auto, out _, out var parameterType)
                    ? ParameterValue.Create(parameterType, expressionItem.value).AsFloat(environment.GetPlatformFeatures())
                    : expressionItem.value;

            return new VRCExpressionsMenu.Control
            {
                icon = expressionItem.icon,
                labels = ToLabels(),
                name = expressionItem.Name,
                parameter = new VRCExpressionsMenu.Control.Parameter { name = hasParameter ? parameter : "" },
                style = VRCExpressionsMenu.Control.Style.Style1,
                subMenu = ToSubMenu(),
                subParameters = ToSubParameters(),
                type = ToType(),
                value = value
            };
        }
    }
}