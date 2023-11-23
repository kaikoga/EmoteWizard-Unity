using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class AnimatorLayerContextBase : IOutputContext<RuntimeAnimatorController>
    {
        readonly AnimatorLayerWizardBase _wizard;

        protected AnimatorLayerContextBase(AnimatorLayerWizardBase wizard) => _wizard = wizard;

        public EmoteWizardEnvironment Environment => _wizard.Environment;

        public GameObject GameObject => _wizard.gameObject;

        public RuntimeAnimatorController OutputAsset
        {
            get => _wizard.outputAsset;
            set => _wizard.outputAsset = value;
        }

        public void DisconnectOutputAssets()
        {
            _wizard.resetClip = null;
            _wizard.outputAsset = null;
        }

        public LayerKind LayerKind => _wizard.LayerKind;

        public AvatarMask DefaultAvatarMask => _wizard.defaultAvatarMask;

        public bool HasResetClip => _wizard.HasResetClip;

        public AnimationClip ResetClip
        {
            get => _wizard.resetClip;
            set => _wizard.resetClip = value;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => _wizard.Environment.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == _wizard.LayerKind);
    }
}