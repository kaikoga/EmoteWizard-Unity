using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class MultiParameterSource : EmoteWizardDataSourceContainerBase, IParameterSource
    {
        [SerializeField] public List<ParameterItem> parameterItems = new List<ParameterItem>();

        public IEnumerable<ParameterItem> ParameterItems => parameterItems;
    }
}