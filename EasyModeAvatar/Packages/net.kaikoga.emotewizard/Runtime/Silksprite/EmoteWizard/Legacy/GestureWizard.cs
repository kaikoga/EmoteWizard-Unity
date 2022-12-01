using System.Collections.Generic;
using Silksprite.EmoteWizard.Base.Legacy;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Legacy;
using UnityEngine;

namespace Silksprite.EmoteWizard.Legacy
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase<IGestureEmoteSource, IGestureParameterEmoteSource, IGestureAnimationMixinSource>, IParameterSource
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public string handSignOverrideParameter = EmoteWizardConstants.Defaults.Params.GestureHandSignOverride;

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
    }
}