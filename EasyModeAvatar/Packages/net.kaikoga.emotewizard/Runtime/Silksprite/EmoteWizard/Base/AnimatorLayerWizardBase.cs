using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimatorLayerWizardBase : EmoteWizardBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract bool HasResetClip { get; }
        public abstract LayerKind LayerKind { get; }

        public override IBehaviourContext ToContext() => GetContext();
        public abstract IAnimatorLayerWizardContext GetContext();

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == LayerKind);

        protected abstract class AnimatorLayerContextBase : IAnimatorLayerWizardContext
        {
            readonly AnimatorLayerWizardBase _wizard;

            public AnimatorLayerContextBase(AnimatorLayerWizardBase wizard) => _wizard = wizard;

            IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

            Component IBehaviourContext.Component => _wizard;

            RuntimeAnimatorController IOutputContext<RuntimeAnimatorController>.OutputAsset
            {
                get => _wizard.outputAsset;
                set => _wizard.outputAsset = value;
            }

            void IBehaviourContext.DisconnectOutputAssets()
            {
                _wizard.resetClip = null;
                _wizard.outputAsset = null;
            }

            LayerKind IAnimatorLayerWizardContext.LayerKind => _wizard.LayerKind;

            AvatarMask IAnimatorLayerWizardContext.DefaultAvatarMask => _wizard.defaultAvatarMask;

            bool IAnimatorLayerWizardContext.HasResetClip => _wizard.HasResetClip;

            AnimationClip IAnimatorLayerWizardContext.ResetClip
            {
                get => _wizard.resetClip;
                set => _wizard.resetClip = value;
            }

            IEnumerable<EmoteItem> IAnimatorLayerWizardContext.CollectEmoteItems() => _wizard.CollectEmoteItems();
        }
    }
}