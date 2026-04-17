using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Animation Clip Mixin Source", 5000)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/animation_clip_mixin_source")]
    public class AnimationClipMixinSource : EmoteWizardDataSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public AnimationClip? animationClip;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate()
        {
            return new AnimationClipMixinTemplate(SelfPath);
        }
    }
}