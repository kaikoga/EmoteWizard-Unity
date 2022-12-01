using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParametersSnapshot
    {
        public List<ParameterInstance> ParameterItems;
        static readonly List<ParameterInstance> DefaultParameterItems = DefaultParameters.Populate();

        public IEnumerable<ParameterInstance> AllParameters => ParameterItems.Concat(DefaultParameterItems);

        public bool AssertParameterExists(string parameterName, ParameterItemKind itemKind)
        {
            foreach (var item in AllParameters)
            {
                if (item.Name != parameterName) continue;

                if (itemKind == ParameterItemKind.Auto || itemKind == item.ItemKind) return true;
                Debug.LogWarning($"Ignored invalid parameter: {parameterName}, expected ${itemKind}, was ${item.ItemKind}");
                return false;
            }

            Debug.LogWarning($"Ignored unknown parameter: {parameterName}");
            return false;
        }

        public VRCExpressionParameters.Parameter[] ToParameters()
        {
            return ParameterItems
                .Select(parameter => parameter.ToParameter())
                .ToArray();
        }

        public bool IsInvalidParameter(string parameterName)
        {
            return !string.IsNullOrEmpty(parameterName) && AllParameters.All(item => item.Name != parameterName);
        }
    }
}