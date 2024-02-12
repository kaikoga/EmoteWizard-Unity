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
        Vrm0BlendShapePreset _vrm0BlendShape;
        Vrm1ExpressionPreset _vrm1Expression;

        static IEnumerable<DefaultBlendShape> Defaults()
        {
            return new List<DefaultBlendShape>
            {
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LipSync,
                    _vrm0BlendShape = Vrm0BlendShapePreset.A,
                    _vrm1Expression = Vrm1ExpressionPreset.Aa,
                },
            };
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultBlendShapes()
        {
            return Defaults().Select(def => EmoteItemTemplate.Builder(LayerKind.None, $"{def._vrm0BlendShape}", def._group, GenericEmoteTrigger.FromVrm0BlendShape(def._vrm0BlendShape), EmoteItemKind.GenericEmoteItem, EmoteSequenceFactoryKind.GenericEmoteSequence)
                .AddFixedDuration(true)
                .AddClip(null, 0f, 0.1f)
                .ToEmoteItemTemplate());
        }
    }
}