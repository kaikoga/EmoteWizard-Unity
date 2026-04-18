using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Animator Controller Mixin Source", 5001)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/animator_controller_mixin_source")]
    public class AnimatorControllerMixinSource : EmoteWizardDataSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public AnimatorControllerMixin mixin = new AnimatorControllerMixin();

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate()
        {
            return new AnimatorControllerMixinTemplate(SelfPath, mixin);
        }
    }
}