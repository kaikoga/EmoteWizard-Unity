using System.Collections.Generic;
using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class ExpressionWizard : EmoteWizardBase
    {
        [SerializeField] public List<ExpressionItem> expressionItems;

        [SerializeField] public VRCExpressionsMenu outputAsset;
        [SerializeField] public string defaultPrefix = "Default/";
        [SerializeField] public bool buildAsSubAsset = true;

        public IEnumerable<IGrouping<string, ExpressionItem>> GroupExpressionItems =>
            expressionItems.GroupBy(expressionItem => expressionItem.parameter);
    }
}