using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterSnapshotBuilder
    {
        readonly List<ParameterInstanceBuilder> _parameterItems = new List<ParameterInstanceBuilder>();
        readonly List<ParameterInstanceBuilder> _implicitParameterItems = new List<ParameterInstanceBuilder>();

        public ParameterInstanceBuilder FindOrCreate(string name) => DoFindOrCreate(name, _parameterItems);
        public ParameterInstanceBuilder FindOrCreateImplicit(string name) => DoFindOrCreate(name, _implicitParameterItems);

        static ParameterInstanceBuilder DoFindOrCreate(string name, List<ParameterInstanceBuilder> list)
        {
            var result = list.FirstOrDefault(parameter => parameter.Name == name);
            if (result == null)
            {
                result = ParameterInstanceBuilder.Populate(name);
                list.Add(result);
            }

            return result;
        }

        public ParametersSnapshot ToSnapshot()
        {
            return new ParametersSnapshot
            {
                ParameterItems = _parameterItems.Where(item => item.HasWriteUsages).Select(item => item.ToInstance()).ToList(),
                ImplicitParameterItems = _implicitParameterItems.Select(item => item.ToInstance()).ToList()
            };
        }
    }
}