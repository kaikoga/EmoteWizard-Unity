using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimatorLayerContextBase : IAnimatorLayerWizardContext
    {
        readonly AnimatorLayerWizardBase _wizard;

        protected AnimatorLayerContextBase(AnimatorLayerWizardBase wizard) => _wizard = wizard;

        IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

        GameObject IBehaviourContext.GameObject => _wizard.gameObject;

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

        IEnumerable<EmoteItem> IAnimatorLayerWizardContext.CollectEmoteItems() => _wizard.Environment.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == _wizard.LayerKind);
    }
}