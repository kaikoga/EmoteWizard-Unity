using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteItemTemplate : IEmoteTemplate
    {
        public string Path { get; }
        public readonly HandSign HandSign;
        public readonly IEmoteSequenceFactoryTemplate SequenceFactory;

        public GenericEmoteItemTemplate(string path,
            HandSign handSign,
            IEmoteSequenceFactoryTemplate sequenceFactory)
        {
            Path = path;
            HandSign = handSign;
            SequenceFactory = sequenceFactory;
        }

        public bool LooksLikeMirrorItem => true;

        EmoteItem ToEmoteItem()
        {
            if (SequenceFactory == null) return null;

            return new EmoteItem(new EmoteTrigger
                {
                    name = HandSign.ToString(),
                    priority = 0,
                    conditions = new List<EmoteCondition>
                    {
                        new EmoteCondition
                        {
                            kind = ParameterItemKind.Auto,
                            parameter = EmoteWizardConstants.Params.Gesture,
                            mode = EmoteConditionMode.Equals,
                            threshold = (int)HandSign
                        }
                    }
                },
                SequenceFactory);
        }

        public IEnumerable<EmoteItem> ToEmoteItems()
        {
            var emoteItem = ToEmoteItem();
            if (emoteItem != null) yield return emoteItem;
        }

        public IEnumerable<ExpressionItem> ToExpressionItems() => Enumerable.Empty<ExpressionItem>();

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<GenericEmoteItemSource>(target);
            source.handSign = HandSign;
            SequenceFactory?.PopulateSequenceSource(undoable, source);
        }
    }
}
