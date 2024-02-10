using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Parameters Wizard Config", 12000)]
    public class ParametersConfig : EmoteConfigBase
    {
#if EW_VRCSDK3_AVATARS
        [SerializeField] public VRCExpressionParameters outputAsset;
#else
        [SerializeField] public ScriptableObject outputAsset;
#endif

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public ParametersContext GetContext(EmoteWizardEnvironment env) => new ParametersContext(env, this);
    }
}