using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ExpressionParameterBuilder
    {
        readonly List<ParameterItemBuilder> _parameterItems;
        public IEnumerable<ParameterItem> ParameterItems => _parameterItems.Where(item => item.enabled).Select(item => item.Export());

        public ExpressionParameterBuilder()
        {
            _parameterItems = new List<ParameterItemBuilder>();
        }

        public ParameterItemBuilder FindOrCreate(string name, bool enable = false)
        {
            var result = _parameterItems.FirstOrDefault(parameter => parameter.name == name);
            if (result == null)
            {
                result = ParameterItemBuilder.Populate(name);
                _parameterItems.Add(result);
            }

            if (enable) result.Enable();
            return result;
        }

        public void Import(IEnumerable<ParameterItem> parameters)
        {
            foreach (var parameter in parameters.Where(i => i.enabled))
            {
                FindOrCreate(parameter.name).Import(parameter);
            }
        }
    }
}