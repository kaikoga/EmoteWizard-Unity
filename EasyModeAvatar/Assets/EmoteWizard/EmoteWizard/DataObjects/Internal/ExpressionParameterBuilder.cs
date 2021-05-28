using System.Collections.Generic;
using System.Linq;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects.Internal
{
    public class ExpressionParameterBuilder
    {
        List<ParameterItem> parameters;

        public ExpressionParameterBuilder()
        {
            parameters = new List<ParameterItem>();
        }

        public ParameterItem FindOrCreate(string name)
        {
            var result = parameters.FirstOrDefault(parameter => parameter.Name == name);
            if (result != null) return result;
            result = ParameterItem.Populate(name);
            parameters.Add(result);
            return result;
        }

        public void Import(IEnumerable<VRCExpressionParameters.Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                FindOrCreate(parameter.name).Import(parameter);
            }
        }

        public void Import(IEnumerable<ParameterItem> parameters)
        {
            foreach (var parameter in parameters)
            {
                FindOrCreate(parameter.Name).Import(parameter);
            }
        }

        public VRCExpressionParameters.Parameter[] Export(bool customOnly = false) =>
            parameters.Where(parameter => !customOnly || !parameter.DefaultParameter)
                .Select(parameter => parameter.Export())
                .ToArray();
    }
}