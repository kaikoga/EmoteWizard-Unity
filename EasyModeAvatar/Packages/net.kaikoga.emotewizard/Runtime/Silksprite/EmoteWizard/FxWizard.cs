using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxWizard : AnimationWizardBase<IFxEmoteSource, IFxParameterEmoteSource, IFxAnimationMixinSource>, IParameterSource
    {
        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public string handSignOverrideParameter = EmoteWizardConstants.Defaults.Params.FxHandSignOverride;

        public IEnumerable<ParameterItem> ParameterItems
        {
            get
            {
                IEnumerable<ParameterItem> EnumerateParameterItems()
                {
                    if (handSignOverrideEnabled)
                    {
                        yield return ParameterItem.Build(handSignOverrideParameter, ParameterItemKind.Int);
                    }
                }
                return EnumerateParameterItems();
            }
        }

        public override void DisconnectOutputAssets()
        {
            base.DisconnectOutputAssets();
            resetClip = null;
        }
    }
}