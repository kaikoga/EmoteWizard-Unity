using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;

namespace Silksprite.EmoteWizard.Legacy
{
    [DisallowMultipleComponent]
    public class ActionWizard : EmoteWizardBase, IParameterSource
    {
        [SerializeField] public bool fixedTransitionDuration = true;
        [SerializeField] public string actionSelectParameter = EmoteWizardConstants.Defaults.Params.ActionSelect;
        [SerializeField] public bool afkSelectEnabled = false;
        [SerializeField] public string afkSelectParameter = EmoteWizardConstants.Defaults.Params.AfkSelect;

        [SerializeField] public ActionEmote defaultAfkEmote;
        [SerializeField] public RuntimeAnimatorController outputAsset;

        public IEnumerable<ParameterItem> ParameterItems
        {
            get
            {

                IEnumerable<ParameterItem> EnumerateParameterItems()
                {
                    yield return ParameterItem.Build(actionSelectParameter, ParameterItemKind.Int);
                    if (afkSelectEnabled)
                    {
                        yield return ParameterItem.Build(afkSelectParameter, ParameterItemKind.Int);
                    }
                }
                return EnumerateParameterItems();
            }
        }

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }
    }
}