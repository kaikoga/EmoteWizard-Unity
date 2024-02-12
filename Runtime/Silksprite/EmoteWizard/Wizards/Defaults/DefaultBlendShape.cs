using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;

namespace Silksprite.EmoteWizard.Wizards.Defaults
{
    public class DefaultBlendShape
    {
        string _group;
        string _name;
        Vrm0BlendShapePreset _vrm0BlendShape;
        Vrm1ExpressionPreset _vrm1Expression;

        static IEnumerable<DefaultBlendShape> Defaults()
        {
            return new List<DefaultBlendShape>
            {
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LipSync,
                    _name = "A",
                    _vrm0BlendShape = Vrm0BlendShapePreset.A,
                    _vrm1Expression = Vrm1ExpressionPreset.Aa,
                },
            };
        }

        IEnumerable<IEmoteTemplate> ToEmoteTemplates()
        {
            yield return new GenericEmoteSequenceTemplate(_name, GenericEmoteSequence.Builder(LayerKind.None, _group).ToGenericEmoteSequenceFactory());
            yield return new GenericEmoteTriggerTemplate($"{_name}/VRM0_{_vrm0BlendShape}", GenericEmoteTrigger.FromVrm0BlendShape(_vrm0BlendShape));
            yield return new GenericEmoteTriggerTemplate($"{_name}/VRM1_{_vrm1Expression}", GenericEmoteTrigger.FromVrm1Expression(_vrm1Expression));
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultBlendShapes()
        {
            return Defaults().SelectMany(def => def.ToEmoteTemplates());
        }
    }
}