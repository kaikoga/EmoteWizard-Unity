using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal.Builders;
using Silksprite.Loch;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParametersSnapshot
    {
        readonly List<string> _validReferenceUsages;

        readonly List<ParameterInstance> _parameterItems;
        readonly List<ParameterInstance> _implicitParameterItems;
        readonly List<ParameterInstance> _defaultParameterItems;

        public IEnumerable<string> ValidReferenceUsages => _validReferenceUsages;
        public IEnumerable<ParameterInstance> ParameterItems => _parameterItems;
        public IEnumerable<ParameterInstance> ImplicitParameterItems => _implicitParameterItems;
        public IEnumerable<ParameterInstance> DefaultParameterItems => _defaultParameterItems;
        public IEnumerable<ParameterInstance> AllParameters => _parameterItems.Concat(_implicitParameterItems).Concat(_defaultParameterItems);

        public ParametersSnapshot(List<ParameterInstance> parameterItems, List<ParameterInstance> implicitParameterItems, List<ParameterInstance> defaultParameterItems)
        {
            _parameterItems = parameterItems;
            _implicitParameterItems = implicitParameterItems;
            _defaultParameterItems = defaultParameterItems;
            _validReferenceUsages = AllParameters.SelectMany(item => item.ReferenceUsages).ToList();
        }

        public bool TryResolveParameterWithType(string parameterName, ParameterItemKind itemKind,
            [MaybeNullWhen(false)] out ParameterInstance parameterInstance,
            out ParameterValueKind valueKind,
            Action<LocalizedContent> onError)
        {
            parameterInstance = AllParameters.FirstOrDefault(item => item.Name == parameterName);
            if (parameterInstance == null)
            {
                valueKind = default;
                onError(Loc("Warn::Parameter::NotFound.").Format(new Substitution
                {
                    ["parameterName"] = parameterName 
                }));
                return false;
            }

            valueKind = parameterInstance.ValueKind;
            switch (valueKind, itemKind)
            {
                case (ParameterValueKind.Bool, ParameterItemKind.Auto):
                case (ParameterValueKind.Bool, ParameterItemKind.Bool):
                case (ParameterValueKind.Int, ParameterItemKind.Auto):
                case (ParameterValueKind.Int, ParameterItemKind.Int):
                case (ParameterValueKind.Float, ParameterItemKind.Auto):
                case (ParameterValueKind.Float, ParameterItemKind.Float):
                case (ParameterValueKind.HandSign, ParameterItemKind.HandSign):
                    break;
                case (ParameterValueKind.HandSign, ParameterItemKind.Auto):
                case (ParameterValueKind.HandSign, ParameterItemKind.Int):
                case (ParameterValueKind.Int, ParameterItemKind.HandSign):
                case (ParameterValueKind.Float, ParameterItemKind.HandSign):
                    onError(Loc("Warn::Parameter::UnexpectedHandSignConversion.").Format(new Substitution
                    {
                        ["parameterName"] = parameterName,
                        ["itemKind"] = $"{itemKind}",
                        ["resolvedItemKind"] = $"{valueKind}"
                    }));
                    break;
                default:
                    onError(Loc("Warn::Parameter::TypeMismatch.").Format(new Substitution
                    {
                        ["parameterName"] = parameterName,
                        ["itemKind"] = $"{itemKind}",
                        ["resolvedItemKind"] = $"{valueKind}"
                    }));
                    break;
            }
            return true;
        }

        public bool IsInvalidParameterReference(string parameterReference)
        {
            return !string.IsNullOrEmpty(parameterReference) && !_validReferenceUsages.Contains(parameterReference);
        }

        public static ParametersSnapshotBuilder Builder() => new ParametersSnapshotBuilder();
    }
}