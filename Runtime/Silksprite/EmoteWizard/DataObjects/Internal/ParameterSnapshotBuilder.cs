using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterSnapshotBuilder
    {
        readonly List<ParameterInstanceBuilder> _parameterItems;

        public ParameterSnapshotBuilder()
        {
            _parameterItems = new List<ParameterInstanceBuilder>();
        }

        public ParameterInstanceBuilder FindOrCreate(string name)
        {
            var result = _parameterItems.FirstOrDefault(parameter => parameter.Name == name);
            if (result == null)
            {
                result = ParameterInstanceBuilder.Populate(name);
                _parameterItems.Add(result);
            }

            return result;
        }

        public ParametersSnapshot ToSnapshot()
        {
            return new ParametersSnapshot
            {
                ParameterItems = _parameterItems.Where(item => item.HasWriteUsages).Select(item => item.ToInstance()).ToList()
            };
        }
    }
}