using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
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

        public override IBehaviourContext ToContext() => GetContext();
        public IExpressionWizardContext GetContext() => new ExpressionContext(this);

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }

        public IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            return Environment.GetComponentsInChildren<IExpressionItemSource>().SelectMany(source => source.ExpressionItems);
        }

        class ExpressionContext : IExpressionWizardContext
        {
            readonly ExpressionWizard _wizard;

            public ExpressionContext(ExpressionWizard wizard)
            {
                _wizard = wizard;
            }

            IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

            Component IBehaviourContext.Component => _wizard;

            VRCExpressionsMenu IOutputContext<VRCExpressionsMenu>.OutputAsset
            {
                get => _wizard.outputAsset;
                set => _wizard.outputAsset = value;
            }

            bool IExpressionWizardContext.BuildAsSubAsset => _wizard.buildAsSubAsset;

            IEnumerable<ExpressionItem> IExpressionWizardContext.CollectExpressionItems() => _wizard.CollectExpressionItems();
        }
    }
}