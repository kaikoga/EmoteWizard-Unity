using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ParametersWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionParameters outputAsset;
        [SerializeField] public List<ParameterItem> parameterItems;

        public bool AssertParameterExists(string parameterName)
        {
            var result = parameterItems.Any(item => item.name == parameterName);
            if (!result) Debug.LogWarning($"Ignored unknown parameter: {parameterName}");
            return result;
        }

        public void TryRefreshParameters()
        {
            var expressionWizard = GetComponent<ExpressionWizard>();
            if (expressionWizard == null)
            {
                Debug.LogWarning("ExpressionWizard not found. Parameters are unchanged.");
                return;
            }
            DoRefreshParameters(expressionWizard);
        }

        public void ForceRefreshParameters()
        {
            var expressionWizard = GetComponent<ExpressionWizard>();
            if (expressionWizard == null)
            {
                throw new Exception("ExpressionWizard not found. Parameters are unchanged.");
            }
            DoRefreshParameters(expressionWizard);
        }

        void DoRefreshParameters(ExpressionWizard expressionWizard)
        {
            var vrcDefaultParametersStub = ParameterItem.VrcDefaultParameters;

            var builder = new ExpressionParameterBuilder();

            builder.Import(vrcDefaultParametersStub); // create VRC default parameters entry

            if (parameterItems != null) builder.Import(parameterItems);

            foreach (var expressionItem in expressionWizard.expressionItems)
            {
                if (!string.IsNullOrEmpty(expressionItem.parameter))
                {
                    builder.FindOrCreate(expressionItem.parameter).AddUsage(expressionItem.value);
                }
                if (!expressionItem.IsPuppet) continue;
                foreach (var subParameter in expressionItem.subParameters.Where(subParameter => !string.IsNullOrEmpty(subParameter)))
                {
                    builder.FindOrCreate(subParameter).AddPuppetUsage(expressionItem.itemKind == ExpressionItemKind.TwoAxisPuppet);
                }
            }

            builder.Import(vrcDefaultParametersStub); // override VRC default parameters with default values

            parameterItems = builder.ParameterItems.ToList();
        }

        public VRCExpressionParameters.Parameter[] ToParameters()
        {
            TryRefreshParameters(); 
            return parameterItems.Select(parameter => parameter.ToParameter()).ToArray();
        }
    }
}