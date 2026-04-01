using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal.Builders
{
    public class ParametersSnapshotBuilder
    {
        readonly Dictionary<string, ParameterInstanceBuilder> _allParameterItems = new Dictionary<string, ParameterInstanceBuilder>();

        readonly List<ParameterInstanceBuilder> _parameterItems = new List<ParameterInstanceBuilder>();
        readonly List<ParameterInstanceBuilder> _implicitParameterItems = new List<ParameterInstanceBuilder>();
        readonly List<ParameterInstanceBuilder> _defaultParameterItems = new List<ParameterInstanceBuilder>();

        public ParameterInstanceBuilder FindOrCreate(string name) => DoFindOrCreate(name, name, _parameterItems);
        public ParameterInstanceBuilder FindOrCreateImplicit(string name) => DoFindOrCreate(name, name, _implicitParameterItems);
        public ParameterInstanceBuilder FindOrCreateDefault(string reference, string name) => DoFindOrCreate(reference, name, _defaultParameterItems);

        ParameterInstanceBuilder DoFindOrCreate(string reference, string name, List<ParameterInstanceBuilder> list)
        {
            if (!_allParameterItems.TryGetValue(reference, out var result))
            {
                result = ParameterInstanceBuilder.Populate(name);
                list.Add(result);
                _allParameterItems.Add(reference, result);
            }

            result.AddReferenceUsage(reference);
            return result;
        }

        public ParametersSnapshot ToSnapshot()
        {
            return new ParametersSnapshot(
                parameterItems: _parameterItems.Where(item => item.HasWriteUsages).Select(item => item.ToInstance()).ToList(),
                implicitParameterItems: _implicitParameterItems.Select(item => item.ToInstance()).ToList(),
                defaultParameterItems: _defaultParameterItems.Select(item => item.ToInstance()).ToList());
        }
    }
}