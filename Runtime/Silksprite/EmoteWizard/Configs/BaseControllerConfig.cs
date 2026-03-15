using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Base Controller Wizard Config", 10100)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/configs#action-wizard-config")]
    public class BaseControllerConfig : AnimatorControllerConfigBase
    {
        public override AnimatorControllerContextBase GetContext(EmoteWizardEnvironment env) => new BaseControllerContext(env, this);

        protected override void Reset()
        {
            base.Reset();
            var context = new BaseControllerContext(CreateEnv());
            hasResetClip = context.HasResetClip;
        }
    }
}