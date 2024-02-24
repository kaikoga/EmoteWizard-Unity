using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal.Builders
{
    public class ParametersSnapshotBuilder
    {
        readonly List<ParameterInstanceBuilder> _parameterItems = new List<ParameterInstanceBuilder>();
        readonly List<ParameterInstanceBuilder> _implicitParameterItems = new List<ParameterInstanceBuilder>();

        public ParameterInstanceBuilder FindOrCreate(string name) => DoFindOrCreate(name, _parameterItems, _parameterItems);
        public ParameterInstanceBuilder FindOrCreateImplicit(string name) => DoFindOrCreate(name, _implicitParameterItems, _implicitParameterItems);
        public ParameterInstanceBuilder FindOrCreateAny(string name) => DoFindOrCreate(name, _parameterItems.Concat(_implicitParameterItems), _parameterItems);

        static ParameterInstanceBuilder DoFindOrCreate(string name, IEnumerable<ParameterInstanceBuilder> existing, List<ParameterInstanceBuilder> list)
        {
            var result = existing.FirstOrDefault(parameter => parameter.Name == name);
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
                parameterItems = _parameterItems.Where(item => item.HasWriteUsages).Select(item => item.ToInstance()).ToList(),
                implicitParameterItems = _implicitParameterItems.Select(item => item.ToInstance()).ToList()
            };
        }
    }
}