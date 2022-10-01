using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ExpressionWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionsMenu outputAsset;
        [SerializeField] public string defaultPrefix = "Default/";
        [SerializeField] public bool buildAsSubAsset = true;

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }

        public IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            return EmoteWizardRoot.GetComponentsInChildren<IExpressionItemSource>().SelectMany(source => source.ExpressionItems)
                .Where(item => item.enabled);
        }
    }
}