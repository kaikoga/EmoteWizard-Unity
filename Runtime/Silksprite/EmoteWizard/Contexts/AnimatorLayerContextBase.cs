using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class AnimatorLayerContextBase : OutputContextBase<AnimatorLayerConfigBase, RuntimeAnimatorController>
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

        AnimationClip _resetClip;
        public AnimationClip ResetClip
        {
            get => _resetClip;
            set
            {
                _resetClip = value;
                if (Config) Config.resetClip = value;
            }
        }

        public AvatarMask DefaultAvatarMask { get; protected set; }
        public LayerKind LayerKind { get; protected set; }
        public bool HasResetClip { get; protected set; }

        protected AnimatorLayerContextBase(EmoteWizardEnvironment env) : base(env) { }
        protected AnimatorLayerContextBase(EmoteWizardEnvironment env, AnimatorLayerConfigBase config) : base(env, config)
        {
            if (env.PersistGeneratedAssets)
            {
                _outputAsset = config.outputAsset;
                _resetClip = config.resetClip;
            }

            DefaultAvatarMask = config.defaultAvatarMask;
            LayerKind = config.LayerKind;
            HasResetClip = config.hasResetClip;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
            ResetClip = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.EmoteItems(LayerKind);
    }
}