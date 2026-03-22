using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Silksprite.EmoteWizard.Sources.Sequence
{
    [AddComponentMenu("Emote Wizard/Sources/Generic Emote Sequence Source", 101)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/generic_emote_sequence_source")]
    public class GenericEmoteSequenceSource : EmoteSequenceSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public GenericEmoteSequence sequence = new GenericEmoteSequence();

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new GenericEmoteSequenceTemplate(EmoteTemplatePath.Context(CreateEnv(), this),
                (GenericEmoteSequenceFactory)ToEmoteFactoryTemplate());
        }

        public override IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate() => new GenericEmoteSequenceFactory(sequence, $"{gameObject.name}_{gameObject.GetInstanceID()}");
        
#if UNITY_EDITOR
        void OnValidate()
        {
            var env = CreateEnv();
            if (env.AvatarRoot == null) return;

            var dirty = false;
            foreach (var enable in sequence.animatedEnable)
            {
                if (enable.relativeRef.RefreshRelativePath(env.AvatarRoot)) dirty = true;
            }
            foreach (var blendShape in sequence.animatedBlendShapes)
            {
                if (blendShape.relativeRef.RefreshRelativePath(env.AvatarRoot)) dirty = true;
            }
            if (dirty) EditorUtility.SetDirty(this);
        }
#endif
    }
}