using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class EditorLayerContext : OutputContextBase<EditorLayerConfig, RuntimeAnimatorController>
    {
        RuntimeAnimatorController _outputAsset;
        public override RuntimeAnimatorController OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Config) Config.outputAsset = value;
            }
        }

        [UsedImplicitly]
        public EditorLayerContext(EmoteWizardEnvironment env) : base(env) { }
        public EditorLayerContext(EmoteWizardEnvironment env, EditorLayerConfig config) : base(env, config, true)
        {
            _outputAsset = config.outputAsset;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.AllEmoteItems();
    }
}