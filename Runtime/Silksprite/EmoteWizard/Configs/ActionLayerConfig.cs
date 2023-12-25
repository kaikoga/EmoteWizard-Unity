using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Action Layer Wizard Config", 10002)]
    public class ActionLayerConfig : AnimatorLayerConfigBase
    {
        public override LayerKind LayerKind => LayerKind.Action;


        public override AnimatorLayerContextBase GetContext(EmoteWizardEnvironment env) => new ActionLayerContext(env, this);
    }
}