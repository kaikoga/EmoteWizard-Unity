using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ExpressionWizard : EmoteWizardBase
    {
        [FormerlySerializedAs("expressionItems")]
        [SerializeField] public List<ExpressionItem> legacyExpressionItems;

        [SerializeField] public VRCExpressionsMenu outputAsset;
        [SerializeField] public string defaultPrefix = "Default/";
        [SerializeField] public bool buildAsSubAsset = true;

        public IEnumerable<ExpressionItem> CollectExpressionItems() => legacyExpressionItems.Where(item => item.enabled);
    }
}