using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : EmoteWizardBehaviour
    {
        protected abstract IEnumerable<IEmoteTemplate> SourceTemplates();

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

        public void Explode(IUndoable undoable, bool andSelect)
        {
            var sourceTemplates = SourceTemplates().ToArray();

            var parent = transform.parent;

            var children = sourceTemplates
                .Select(template => template.Path)
                .OrderBy(path => path.Length)
                .Distinct()
                .ToDictionary(path => path, path => undoable.AddChildGameObject(parent, path));

            foreach (var template in sourceTemplates)
            {
                var child = children[template.Path];
                template.PopulateSources(undoable, child.transform);
            }

            if (andSelect)
            {
                var firstChild = children.Values.FirstOrDefault();
                if (firstChild) undoable.SetActiveObjectWithContext(firstChild, firstChild);
            }

            if (gameObject.GetComponents<Component>().Length == 2)
            {
                undoable.DestroyObject(gameObject);
            }
            else
            {
                undoable.DestroyObject(this);
            }
        }
    }
}