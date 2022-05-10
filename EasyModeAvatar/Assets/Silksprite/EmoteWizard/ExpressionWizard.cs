using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
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

        public bool HasLegacyData => legacyExpressionItems.Any();

        public IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            return GetComponentsInChildren<ExpressionItemSource>().SelectMany(source => source.expressionItems)
                .Where(item => item.enabled);
        }
    }
}