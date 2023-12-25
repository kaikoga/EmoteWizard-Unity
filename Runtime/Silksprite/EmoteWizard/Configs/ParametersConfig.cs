using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Parameters Wizard Config", 12000)]
    public class ParametersConfig : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionParameters outputAsset;

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public ParametersContext GetContext(EmoteWizardEnvironment env) => new ParametersContext(env, this);
    }
}