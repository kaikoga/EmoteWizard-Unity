using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Editor Layer Wizard Config", 10003)]
    public class EditorLayerConfig : EmoteWizardBase
    {
        [SerializeField] public RuntimeAnimatorController outputAsset;

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public EditorLayerContext GetContext(EmoteWizardEnvironment env) => new EditorLayerContext(env, this);
    }
}