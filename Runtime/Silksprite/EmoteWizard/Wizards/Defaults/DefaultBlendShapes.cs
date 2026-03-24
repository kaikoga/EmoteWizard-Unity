using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Templates;

namespace Silksprite.EmoteWizard.Wizards.Defaults
{
    public static class DefaultBlendShapes
    {
        public static IEnumerable<IEmoteTemplate> EnumerateDefaultBlendShapes(EmoteTemplatePath path)
        {
            // note: enumerate directly to unpacked templates because unpacked templates are already platform agnostic 
            return DefaultBlendShape.Defaults().SelectMany(def => def.ToEmoteTemplates(path));
        }
    }
}