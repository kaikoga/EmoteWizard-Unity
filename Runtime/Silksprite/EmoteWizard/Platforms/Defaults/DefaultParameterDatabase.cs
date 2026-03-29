using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.Platforms.Defaults
{
    class DefaultParameterDatabase
    {
        readonly (string reference, string name, ParameterItemKind kind, int[] states)[] _tuples;

        public DefaultParameterDatabase((string reference, string name, ParameterItemKind kind, int[] states)[] tuples)
        {
            _tuples = tuples;
        }
        
        public string ResolveParameterReference(string parameterReference)
        {
            return _tuples.FirstOrDefault(tuple => tuple.reference == parameterReference).name ?? parameterReference;
        }

        public bool IsDefaultParameterReference(string parameterReference)
        {
            return _tuples.Any(data => parameterReference == data.name);
        }

        public List<ParameterInstance> ToInstances()
        {
            return _tuples.Select(tuple =>
            {
                var (reference, name, kind, states) = tuple;
                var writeUsageKind = kind switch
                {
                    ParameterItemKind.Auto => ParameterWriteUsageKind.Int,
                    ParameterItemKind.Bool => ParameterWriteUsageKind.Int,
                    ParameterItemKind.Int => ParameterWriteUsageKind.Int,
                    ParameterItemKind.Float => ParameterWriteUsageKind.Int,
                    ParameterItemKind.HandSign => ParameterWriteUsageKind.HandSign,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return new ParameterInstance(
                    name: name,
                    itemKind: kind,
                    saved: false,
                    defaultValue: ParameterValue.Default,
                    synced: true,
                    referenceUsages: new List<string> {
                        reference
                    },
                    writeUsages: states.Select(state => new ParameterWriteUsage(writeUsageKind, state, ParameterWriteSourceKind.NoUI)),
                    readUsages: states.Select(state => new ParameterReadUsage(ParameterValue.Create(kind, state))));
            }).ToList();
        }
    }
}
