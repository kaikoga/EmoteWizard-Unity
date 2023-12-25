using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    public class ExpressionConfig : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionsMenu outputAsset;
        [SerializeField] public string defaultPrefix = "Default/";
        [SerializeField] public bool buildAsSubAsset = true;

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public ExpressionContext GetContext(EmoteWizardEnvironment env) => new ExpressionContext(env, this);
    }
}