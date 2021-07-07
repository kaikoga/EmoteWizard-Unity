using System.Collections.Generic;
using System.Linq;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ExpressionParameterBuilder
    {
        readonly List<ParameterItemBuilder> parameterItems;
        public IEnumerable<ParameterItem> ParameterItems => parameterItems.Select(item => item.Export());

        public ExpressionParameterBuilder()
        {
            parameterItems = new List<ParameterItemBuilder>();
        }

        public ParameterItemBuilder FindOrCreate(string name)
        {
            var result = parameterItems.FirstOrDefault(parameter => parameter.name == name);
            if (result != null) return result;
            result = ParameterItemBuilder.Populate(name);
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