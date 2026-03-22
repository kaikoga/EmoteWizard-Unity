using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Override Controller Wizard Config", 10101)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/configs#override-controller-wizard-config")]
    public class OverrideControllerConfig : EmoteConfigBase
    {
        [SerializeField] public AnimatorOverrideController? outputAsset;

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public OverrideControllerContext GetContext(EmoteWizardEnvironment env) => new OverrideControllerContext(env, this);
    }
}