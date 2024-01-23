using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Parameter Source", 2000)]
    public class ParameterSource : EmoteWizardDataSourceBase, IParameterSource
    {
        [SerializeField] public ParameterItem parameterItem;

        public IEnumerable<ParameterItem> ToParameterItems()
        {
            if (parameterItem.IsValid) yield return parameterItem;
        }
    }
}