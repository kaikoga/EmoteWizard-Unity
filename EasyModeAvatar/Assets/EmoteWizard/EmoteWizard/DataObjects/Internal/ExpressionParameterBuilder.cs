using System.Collections.Generic;
using System.Linq;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects.Internal
{
    public class ExpressionParameterBuilder
    {
        readonly List<ParameterItem> parameterItems;
        public IEnumerable<ParameterItem> ParameterItems => parameterItems;

        public ExpressionParameterBuilder()
        {
            parameterItems = new List<ParameterItem>();
        }

        public ParameterItem FindOrCreate(string name)
        {
            var result = parameterItems.FirstOrDefault(parameter => parameter.name == name);
            if (result != null) return result;
            result = ParameterItem.Populate(name);
            parameterItems.Add(result);
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
                FindOrCreate(parameter.name).Import(parameter);
            }
        }
    }
}