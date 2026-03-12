using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Emote Item Source", 0)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/emote_item_source")]
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource
    {
        [SerializeField] public EmoteTrigger trigger;

        [SerializeField] public EmoteSequenceSourceBase sequence;

        [SerializeField] public bool hasExpressionItem;
        [ItemPath]
        [SerializeField] public string expressionItemPath;
        [SerializeField] public Texture2D expressionItemIcon;

        public EmoteSequenceSourceBase FindEmoteSequenceSource()
        {
            if (sequence) return sequence;

            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .FirstOrDefault();
        }

        EmoteItemTemplate ToTemplate()
        {
            IEmoteSequenceFactoryTemplate FindEmoteFactory()
            {
                var source = FindEmoteSequenceSource();
                if (!source) return null;

                return source.ToEmoteFactoryTemplate();
            }

            // TODO cache me?
            return new EmoteItemTemplate(gameObject.name, trigger, FindEmoteFactory(), hasExpressionItem, expressionItemPath, expressionItemIcon);
        }

        public bool LooksLikeMirrorItem => ToTemplate().LooksLikeMirrorItem;

        public bool CanAutoExpression(EmoteWizardEnvironment environment) => ToTemplate().CanAutoExpression(environment);

        public bool IsAutoExpression(EmoteWizardEnvironment environment) => ToTemplate().IsAutoExpression(environment);

        public IEnumerable<EmoteItem> ToEmoteItems(EmoteWizardEnvironment environment) => ToTemplate().ToEmoteItems(environment);

        public IEnumerable<ExpressionItem> ToExpressionItems(EmoteWizardEnvironment environment) => ToTemplate().ToExpressionItems(environment);
    }
}