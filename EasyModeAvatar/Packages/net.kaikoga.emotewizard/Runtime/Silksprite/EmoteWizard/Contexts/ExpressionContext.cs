using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ExpressionContext : OutputContextBase<ExpressionWizard, VRCExpressionsMenu>
    {
        VRCExpressionsMenu _outputAsset;
        public override VRCExpressionsMenu OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Wizard) Wizard.outputAsset = value;
            }
        }

        public readonly bool BuildAsSubAsset;

        public ExpressionContext(ExpressionWizard wizard) : base(wizard)
        {
            _outputAsset = wizard.outputAsset;
            BuildAsSubAsset = Wizard.buildAsSubAsset;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
        }

        public IEnumerable<ExpressionItem> CollectExpressionItems() => Wizard.CollectExpressionItems();
    }
}