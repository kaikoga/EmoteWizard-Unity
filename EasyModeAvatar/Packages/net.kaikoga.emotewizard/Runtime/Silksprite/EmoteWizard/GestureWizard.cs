using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard
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

        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}