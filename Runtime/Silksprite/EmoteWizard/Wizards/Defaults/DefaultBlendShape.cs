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
                    _vrm1Expression = Vrm1ExpressionPreset.Aa
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LipSync,
                    _name = "I",
                    _vrm0BlendShape = Vrm0BlendShapePreset.I,
                    _vrm1Expression = Vrm1ExpressionPreset.Ih
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LipSync,
                    _name = "U",
                    _vrm0BlendShape = Vrm0BlendShapePreset.U,
                    _vrm1Expression = Vrm1ExpressionPreset.Ou
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LipSync,
                    _name = "E",
                    _vrm0BlendShape = Vrm0BlendShapePreset.E,
                    _vrm1Expression = Vrm1ExpressionPreset.Ee
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LipSync,
                    _name = "O",
                    _vrm0BlendShape = Vrm0BlendShapePreset.O,
                    _vrm1Expression = Vrm1ExpressionPreset.Oh
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Blink,
                    _name = "Blink",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Blink,
                    _vrm1Expression = Vrm1ExpressionPreset.Blink
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Blink,
                    _name = "BlinkLeft",
                    _vrm0BlendShape = Vrm0BlendShapePreset.BlinkL,
                    _vrm1Expression = Vrm1ExpressionPreset.BlinkLeft
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Blink,
                    _name = "BlinkRight",
                    _vrm0BlendShape = Vrm0BlendShapePreset.BlinkR,
                    _vrm1Expression = Vrm1ExpressionPreset.BlinkRight
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LookAt,
                    _name = "LookUp",
                    _vrm0BlendShape = Vrm0BlendShapePreset.LookUp,
                    _vrm1Expression = Vrm1ExpressionPreset.LookUp
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LookAt,
                    _name = "LookDown",
                    _vrm0BlendShape = Vrm0BlendShapePreset.LookDown,
                    _vrm1Expression = Vrm1ExpressionPreset.LookDown
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LookAt,
                    _name = "LookLeft",
                    _vrm0BlendShape = Vrm0BlendShapePreset.LookLeft,
                    _vrm1Expression = Vrm1ExpressionPreset.LookLeft
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.LookAt,
                    _name = "LookRight",
                    _vrm0BlendShape = Vrm0BlendShapePreset.LookRight,
                    _vrm1Expression = Vrm1ExpressionPreset.LookRight
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Joy_0",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Joy,
                    _vrm1Expression = Vrm1ExpressionPreset.Custom
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Angry_0",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Angry,
                    _vrm1Expression = Vrm1ExpressionPreset.Custom
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Sorrow_0",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Sorrow,
                    _vrm1Expression = Vrm1ExpressionPreset.Custom
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Fun_0",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Fun,
                    _vrm1Expression = Vrm1ExpressionPreset.Custom
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Happy_1",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Unknown,
                    _vrm1Expression = Vrm1ExpressionPreset.Happy
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Angry_1",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Unknown,
                    _vrm1Expression = Vrm1ExpressionPreset.Angry
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Sad_1",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Unknown,
                    _vrm1Expression = Vrm1ExpressionPreset.Sad
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Relaxed_1",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Unknown,
                    _vrm1Expression = Vrm1ExpressionPreset.Relaxed
                },
                new DefaultBlendShape
                {
                    _group = EmoteWizardConstants.Defaults.Groups.Emotion,
                    _name = "Surprised_1",
                    _vrm0BlendShape = Vrm0BlendShapePreset.Unknown,
                    _vrm1Expression = Vrm1ExpressionPreset.Surprised
                }
            };
        }

        IEnumerable<IEmoteTemplate> ToEmoteTemplates()
        {
            var builder = GenericEmoteSequence.Builder(LayerKind.None, _group);
            builder.AddFixedDuration(true);
            builder.AddClip(null, 0f, 0.1f);
            yield return new GenericEmoteSequenceTemplate(_name, builder.ToGenericEmoteSequenceFactory());
            if (_vrm0BlendShape != Vrm0BlendShapePreset.Unknown)
            {
                yield return new GenericEmoteTriggerTemplate($"{_name}/VRM0_{_vrm0BlendShape}", GenericEmoteTrigger.FromVrm0BlendShape(_vrm0BlendShape));
            }
            if (_vrm1Expression != Vrm1ExpressionPreset.Custom)
            {
                yield return new GenericEmoteTriggerTemplate($"{_name}/VRM1_{_vrm1Expression}", GenericEmoteTrigger.FromVrm1Expression(_vrm1Expression));
            }
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultBlendShapes()
        {
            return Defaults().SelectMany(def => def.ToEmoteTemplates());
        }
    }
}