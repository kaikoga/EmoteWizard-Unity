using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IParameterItemTemplate : IEmoteTemplate
    {
        IEnumerable<ParameterItem> ToParameterItems();
    }
}