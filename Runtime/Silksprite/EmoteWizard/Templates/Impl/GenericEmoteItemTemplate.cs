using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteItemTemplate : IEmoteItemTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly GenericEmoteTrigger _trigger;
        readonly IEmoteSequenceFactoryTemplate? _sequenceFactory;

        public GenericEmoteItemTemplate(EmoteTemplatePath path,
            GenericEmoteTrigger trigger, 
            IEmoteSequenceFactoryTemplate? sequenceFactory)
        {
            _path = path;
            _trigger = trigger;
            _sequenceFactory = sequenceFactory;
        }

        EmoteItem? ToEmoteItem()
        {
            if (_sequenceFactory == null) return null;

            if (!_trigger.TryGetHandSign(out var handSign)) return null;
            
            return new EmoteItem(new EmoteTriggerInstance
                (
                    name: handSign.ToString(),
                    priority: 0,
                    conditions: new List<EmoteConditionInstance>
                    {
                        new EmoteConditionInstance(
                            kind: ParameterItemKind.Auto,
                            parameter: EmoteWizardConstants.Params.Gesture,
                            mode: EmoteConditionMode.Equals,
                            value: ParameterValue.HandSign(handSign)
                        )
                    }
                ),
                _sequenceFactory);
        }

        GenericEmoteItem? ToGenericEmoteItem()
        {
            if (!(_sequenceFactory is IGenericEmoteSequenceFactory genericSequenceFactory)) return null;

            return new GenericEmoteItem(_trigger, genericSequenceFactory);
        }

        IEnumerable<EmoteItem> IEmoteItemTemplate.ToEmoteItems(IPlatformFeatures platformFeatures)
        {
            if (ToEmoteItem() is { } emoteItem)
            {
                yield return emoteItem;
            }
        }

        public IEnumerable<GenericEmoteItem> ToGenericEmoteItems()
        {
            if (ToGenericEmoteItem() is { } emoteItem)
            {
                yield return emoteItem;
            }
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<GenericEmoteItemSource>(target);
            source.trigger = _trigger;
            _sequenceFactory?.PopulateSequenceSource(undoable, source);
        }
    }
}
