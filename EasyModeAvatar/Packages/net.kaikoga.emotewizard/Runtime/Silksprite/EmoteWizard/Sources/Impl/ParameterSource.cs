using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class ParameterSource : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ParameterItem> parameterItems = new List<ParameterItem>();
    }
}