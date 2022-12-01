using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class ParameterSource : EmoteWizardDataSourceBase, IParameterSource
    {
        [SerializeField] public ParameterItem parameterItem;

        public IEnumerable<ParameterItem> ParameterItems
        {
            get { yield return parameterItem; }
        }
    }
}