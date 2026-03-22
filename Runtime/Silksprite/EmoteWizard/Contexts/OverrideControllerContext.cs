using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class OverrideControllerContext : OutputContextBase<OverrideControllerConfig, AnimatorOverrideController>
    {
        AnimatorOverrideController? _outputAsset;
        public override AnimatorOverrideController? OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Config != null) Config.outputAsset = value;
            }
        }

        [UsedImplicitly]
        public OverrideControllerContext(EmoteWizardEnvironment env) : base(env) { }

        public OverrideControllerContext(EmoteWizardEnvironment env, OverrideControllerConfig config) : base(env, config)
        {
            if (env.PersistGeneratedAssets)
            {
                _outputAsset = config.outputAsset;
            }
        }
        
        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
        }
    }
}