using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Emote Item Source", 0)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/emote_item_source")]
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public EmoteTrigger trigger = new EmoteTrigger();

        [SerializeField] public EmoteSequenceSourceBase? sequence;

        [SerializeField] public bool hasExpressionItem;
        [ItemPath]
        [SerializeField] public string expressionItemPath = "";
        [SerializeField] public Texture2D? expressionItemIcon;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate()
        {
            return new EmoteItemTemplate(SelfPath,
                trigger,
                FindEmoteSequenceSource()?.ToEmoteFactoryTemplate(), // TODO: sequence?.ToEmoteFactoryTemplate() after sequence resolving in templates 
                hasExpressionItem,
                expressionItemPath,
                expressionItemIcon);
        }
        
        public EmoteSequenceSourceBase? FindEmoteSequenceSource()
        {
            if (sequence) return sequence;

            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .FirstOrDefault();
        }

        EmoteItemTemplate ToTemplate()
        {
            IEmoteSequenceFactoryTemplate? FindEmoteFactory()
            {
                var source = FindEmoteSequenceSource();
                return source != null ? source.ToEmoteFactoryTemplate() : null;
            }

            // TODO cache me?
            return new EmoteItemTemplate(SelfPath, trigger, FindEmoteFactory(), hasExpressionItem, expressionItemPath, expressionItemIcon);
        }

        public bool LooksLikeMirrorItem => ToTemplate().LooksLikeMirrorItem;

        public bool CanAutoExpression(IPlatformFeatures platformFeatures) => ToTemplate().CanAutoExpression(platformFeatures);

        public bool IsAutoExpression(IPlatformFeatures platformFeatures) => ToTemplate().IsAutoExpression(platformFeatures);
    }
}