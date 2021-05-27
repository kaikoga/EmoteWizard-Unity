using System.Linq;
using EmoteWizard.Base;
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

        public VRCExpressionParameters.Parameter[] ToParameters(bool customOnly = false)
        {
            var vrcDefaultParametersStub = ExpressionParameterBuilder.ParameterStub.VrcDefaultParameters;
            var oldParameters = outputAsset.parameters.ToList();
            var builder = new ExpressionParameterBuilder();

            builder.Import(vrcDefaultParametersStub); // create VRC default parameters entry

            builder.Import(oldParameters);

            foreach (var expressionItem in ExpressionWizard.expressionItems)
            {
                builder.FindOrCreate(expressionItem.parameter).AddUsage(expressionItem.value);
            }

            builder.Import(vrcDefaultParametersStub); // override VRC default parameters with default values

            return builder.Export(customOnly);
        }
    }
}