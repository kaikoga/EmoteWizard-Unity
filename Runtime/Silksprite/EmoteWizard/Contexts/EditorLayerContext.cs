using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class EditorLayerContext : OutputContextBase<EditorLayerWizard, RuntimeAnimatorController>
    {
        RuntimeAnimatorController _outputAsset;
        public override RuntimeAnimatorController OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Wizard) Wizard.outputAsset = value;
            }
        }

        [UsedImplicitly]
        public EditorLayerContext(EmoteWizardEnvironment env) : base(env) { }
        public EditorLayerContext(EmoteWizardEnvironment env, EditorLayerWizard wizard) : base(env, wizard, true)
        {
            _outputAsset = wizard.outputAsset;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.AllEmoteItems();
    }
}