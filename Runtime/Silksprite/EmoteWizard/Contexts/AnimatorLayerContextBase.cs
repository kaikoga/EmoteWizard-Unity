using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class AnimatorLayerContextBase : OutputContextBase<AnimatorLayerWizardBase, RuntimeAnimatorController>
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

        AnimationClip _resetClip;
        public AnimationClip ResetClip
        {
            get => _resetClip;
            set
            {
                _resetClip = value;
                if (Wizard) Wizard.resetClip = value;
            }
        }

        public AvatarMask DefaultAvatarMask { get; protected set; }
        public LayerKind LayerKind { get; protected set; }
        public bool HasResetClip { get; protected set; }

        protected AnimatorLayerContextBase(EmoteWizardEnvironment env) : base(env) { }
        protected AnimatorLayerContextBase(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard)
        {
            if (env.PersistGeneratedAssets)
            {
                _outputAsset = wizard.outputAsset;
                _resetClip = wizard.resetClip;
            }

            DefaultAvatarMask = wizard.defaultAvatarMask;
            LayerKind = wizard.LayerKind;
            HasResetClip = wizard.hasResetClip;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
            ResetClip = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.EmoteItems(LayerKind);
    }
}