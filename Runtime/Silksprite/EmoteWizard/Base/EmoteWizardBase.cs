using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Sequence;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : EmoteWizardBehaviour
    {
        public abstract IEnumerable<IEmoteTemplate> SourceTemplates();

        protected IEmoteSequenceFactoryTemplate GenerateEmoteSequenceFactoryTemplate(EmoteSequenceFactoryKind kind, LayerKind layerKind, string groupName)
        {
            switch (kind)
            {
                case EmoteSequenceFactoryKind.EmoteSequence:
                    if (layerKind == LayerKind.Action)
                    {
                        return GenerateActionSequenceFactoryTemplate();
                    }
                    return new EmoteSequenceFactory(new EmoteSequence
                    {
                        layerKind = layerKind,
                        groupName = groupName
                    });
                case EmoteSequenceFactoryKind.GenericEmoteSequence:
                    return new GenericEmoteSequenceFactory(new GenericEmoteSequence
                    {
                        layerKind = layerKind,
                        groupName = groupName
                    }, gameObject.name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        static IEmoteSequenceFactoryTemplate GenerateActionSequenceFactoryTemplate() =>
            new EmoteSequenceFactory(new EmoteSequence
            {
                layerKind = LayerKind.Action,
                groupName = EmoteWizardConstants.Defaults.Groups.Action,
                hasLayerBlend = true,
                hasTrackingOverrides = true,
                trackingOverrides = new[]
                {
                    TrackingTarget.Head,
                    TrackingTarget.LeftHand,
                    TrackingTarget.RightHand,
                    TrackingTarget.Hip,
                    TrackingTarget.LeftFoot,
                    TrackingTarget.RightFoot,
                    TrackingTarget.LeftFingers,
                    TrackingTarget.RightFingers
                }.Select(t => new TrackingOverride { target = t }).ToList(),
                blendIn = 0.5f,
                blendOut = 0.25f
            });

        public enum EmoteSequenceFactoryKind
        {
            EmoteSequence,
            GenericEmoteSequence
        }
    }
}