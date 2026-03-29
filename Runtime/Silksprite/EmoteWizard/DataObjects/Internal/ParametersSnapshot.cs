using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal.Builders;
using Silksprite.Loch;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public class ParametersSnapshot
    {
        [SerializeField] List<string> validReferenceUsages;

        [SerializeField] List<ParameterInstance> parameterItems;
        [SerializeField] List<ParameterInstance> implicitParameterItems;
        [SerializeField] List<ParameterInstance> defaultParameterItems;

        public IEnumerable<string> ValidReferenceUsages => validReferenceUsages;
        public IEnumerable<ParameterInstance> ParameterItems => parameterItems;
        public IEnumerable<ParameterInstance> ImplicitParameterItems => implicitParameterItems;
        public IEnumerable<ParameterInstance> DefaultParameterItems => defaultParameterItems;
        public IEnumerable<ParameterInstance> AllParameters => parameterItems.Concat(implicitParameterItems).Concat(defaultParameterItems);

        public ParametersSnapshot(List<ParameterInstance> parameterItems, List<ParameterInstance> implicitParameterItems, List<ParameterInstance> defaultParameterItems)
        {
            this.parameterItems = parameterItems;
            this.implicitParameterItems = implicitParameterItems;
            this.defaultParameterItems = defaultParameterItems;
            validReferenceUsages = AllParameters.SelectMany(item => item.referenceUsages).ToList();
        }

        public bool TryResolveParameterWithType(string parameterName, ParameterItemKind itemKind,
            [MaybeNullWhen(false)] out ParameterInstance parameterInstance,
            out ParameterValueKind valueKind,
            Action<LocalizedContent, Substitution> onError)
        {
            parameterInstance = AllParameters.FirstOrDefault(item => item.name == parameterName);
            if (parameterInstance == null)
            {
                valueKind = default;
                onError(Loc("Warn::Parameter::NotFound."), new Substitution
                {
                    ["parameterName"] = parameterName 
                });
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
                    onError(Loc("Warn::Parameter::UnexpectedHandSignConversion."),
                        new Substitution
                        {
                            ["parameterName"] = parameterName,
                            ["itemKind"] = $"{itemKind}",
                            ["resolvedItemKind"] = $"{valueKind}"
                        });
                    break;
                default:
                    onError(Loc("Warn::Parameter::TypeMismatch."),
                        new Substitution
                        {
                            ["parameterName"] = parameterName,
                            ["itemKind"] = $"{itemKind}",
                            ["resolvedItemKind"] = $"{valueKind}"
                        });
                    break;
            }
            return true;
        }

        public bool IsInvalidParameterReference(string parameterReference)
        {
            return !string.IsNullOrEmpty(parameterReference) && !validReferenceUsages.Contains(parameterReference);
        }

        public static ParametersSnapshotBuilder Builder(EmoteWizardEnvironment env) => new ParametersSnapshotBuilder(env);
    }
}