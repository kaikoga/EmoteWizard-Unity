using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ExpressionContext : IExpressionWizardContext
    {
        readonly ExpressionWizard _wizard;

        public ExpressionContext(ExpressionWizard wizard)
        {
            _wizard = wizard;
        }

        IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

        GameObject IBehaviourContext.GameObject => _wizard.gameObject;

        VRCExpressionsMenu IOutputContext<VRCExpressionsMenu>.OutputAsset
        {
            get => _wizard.outputAsset;
            set => _wizard.outputAsset = value;
        }

        void IBehaviourContext.DisconnectOutputAssets()
        {
            _wizard.outputAsset = null;
        }

        bool IExpressionWizardContext.BuildAsSubAsset => _wizard.buildAsSubAsset;

        IEnumerable<ExpressionItem> IExpressionWizardContext.CollectExpressionItems() => _wizard.CollectExpressionItems();
    }
}