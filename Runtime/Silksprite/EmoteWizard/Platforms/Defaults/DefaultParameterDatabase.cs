using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.DataObjects.Internal.Builders;

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

        public void BuildTo(ParametersSnapshotBuilder builder)
        {
            foreach (var tuple in _tuples)
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

                var parameter = builder.FindOrCreateDefault(reference, name);
                parameter.AddValueKind(kind);
                parameter.AddSynced();
                foreach (var state in states)
                {
                    parameter.AddWriteValue(writeUsageKind, state, ParameterWriteSourceKind.NoUI);
                    parameter.AddReadValue(ParameterValue.Create(kind, state));
                }
            }
        }
    }
}
