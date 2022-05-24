using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    public class ParameterSource : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ParameterItem> parameterItems;
    }
}