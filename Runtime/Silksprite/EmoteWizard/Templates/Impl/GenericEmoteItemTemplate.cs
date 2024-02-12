using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteItemTemplate : IEmoteTemplate
    {
        public string Path { get; }
        public readonly GenericEmoteTrigger Trigger;
        public readonly IEmoteSequenceFactoryTemplate SequenceFactory;

        public GenericEmoteItemTemplate(string path,
            GenericEmoteTrigger trigger, 
            IEmoteSequenceFactoryTemplate sequenceFactory)
        {
            Path = path;
            Trigger = trigger;
            SequenceFactory = sequenceFactory;
        }

        public bool LooksLikeMirrorItem => true;

        EmoteItem ToEmoteItem()
        {
            if (!(SequenceFactory is IGenericEmoteSequenceFactory genericSequenceFactory)) return null;

            return new EmoteItem(new EmoteTrigger
                {
                    name = Trigger.handSign.ToString(),
                    priority = 0,
                    conditions = new List<EmoteCondition>
                    {
                        new EmoteCondition
                        {
                            kind = ParameterItemKind.Auto,
                            parameter = EmoteWizardConstants.Params.Gesture,
                            mode = EmoteConditionMode.Equals,
                            threshold = (int)Trigger.handSign
                        }
                    }
                },
                genericSequenceFactory);
        }

        public IEnumerable<EmoteItem> ToEmoteItems()
        {
            var emoteItem = ToEmoteItem();
            if (emoteItem != null) yield return emoteItem;
        }

        GenericEmoteItem ToGenericEmoteItem()
        {
            if (!(SequenceFactory is IGenericEmoteSequenceFactory genericSequenceFactory)) return null;

            return new GenericEmoteItem(Trigger, genericSequenceFactory);
        }

        public IEnumerable<GenericEmoteItem> ToGenericEmoteItems()
        {
            var emoteItem = ToGenericEmoteItem();
            if (emoteItem != null) yield return emoteItem;
        }

        public IEnumerable<ExpressionItem> ToExpressionItems() => Enumerable.Empty<ExpressionItem>();

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<GenericEmoteItemSource>(target);
            source.trigger = Trigger;
            SequenceFactory?.PopulateSequenceSource(undoable, source);
        }
    }
}
