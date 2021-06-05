using System.Collections.Generic;
using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.DataObjects.Internal;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ExpressionWizard))]
    public class ParametersWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionParameters outputAsset;
        [SerializeField] public bool vrcDefaultParameters = true;
        [SerializeField] public List<ParameterItem> parameterItems;

        ExpressionWizard ExpressionWizard => GetComponent<ExpressionWizard>();
        public IEnumerable<ParameterItem> CustomParameterItems => parameterItems.Where(parameter => !parameter.defaultParameter);

        public void RefreshParameters()
        {
            var customOnly = !vrcDefaultParameters;
            var vrcDefaultParametersStub = ParameterItem.VrcDefaultParameters;
            
            IEnumerable<VRCExpressionParameters.Parameter> oldParameters;
            if (outputAsset != null)
            {
                oldParameters = outputAsset.parameters?.ToList() ?? Enumerable.Empty<VRCExpressionParameters.Parameter>();
            }
            else
            {
                oldParameters = Enumerable.Empty<VRCExpressionParameters.Parameter>();
            }

            var builder = new ExpressionParameterBuilder();

            builder.Import(vrcDefaultParametersStub); // create VRC default parameters entry

            builder.Import(parameterItems);

            builder.Import(oldParameters);

            foreach (var expressionItem in ExpressionWizard.expressionItems)
            {
                builder.FindOrCreate(expressionItem.parameter).AddUsage(expressionItem.value);
            }

            builder.Import(vrcDefaultParametersStub); // override VRC default parameters with default values

            parameterItems = builder.ParameterItems.Where(parameter => !customOnly || !parameter.defaultParameter).ToList();
        }

        public VRCExpressionParameters.Parameter[] ToParameters()
        {
            RefreshParameters(); 
            return parameterItems.Select(parameter => parameter.ToParameter()).ToArray();
        }
    }
}