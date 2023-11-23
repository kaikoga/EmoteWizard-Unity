using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ExpressionContext : IOutputContext<VRCExpressionsMenu>
    {
        readonly ExpressionWizard _wizard;

        public ExpressionContext(ExpressionWizard wizard)
        {
            _wizard = wizard;
        }

        public IEmoteWizardEnvironment Environment => _wizard.Environment;

        public GameObject GameObject => _wizard.gameObject;

        public VRCExpressionsMenu OutputAsset
        {
            get => _wizard.outputAsset;
            set => _wizard.outputAsset = value;
        }

        public void DisconnectOutputAssets()
        {
            _wizard.outputAsset = null;
        }

        public bool BuildAsSubAsset => _wizard.buildAsSubAsset;

        public IEnumerable<ExpressionItem> CollectExpressionItems() => _wizard.CollectExpressionItems();
    }
}