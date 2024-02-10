using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Expression Wizard Config", 11000)]
    public class ExpressionConfig : EmoteConfigBase
    {
#if EW_VRCSDK3_AVATARS
        [SerializeField] public VRCExpressionsMenu outputAsset;
#else
        [SerializeField] public ScriptableObject outputAsset;
#endif
        [SerializeField] public string defaultPrefix = "Default/";
        [SerializeField] public bool buildAsSubAsset = true;

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public ExpressionContext GetContext(EmoteWizardEnvironment env) => new ExpressionContext(env, this);
    }
}