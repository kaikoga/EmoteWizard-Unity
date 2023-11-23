using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ExpressionContext : OutputContextBase<ExpressionWizard, VRCExpressionsMenu>
    {
        public ExpressionContext(ExpressionWizard wizard) : base(wizard) { }

        public override VRCExpressionsMenu OutputAsset
        {
            get => Wizard.outputAsset;
            set => Wizard.outputAsset = value;
        }

        public override void DisconnectOutputAssets()
        {
            Wizard.outputAsset = null;
        }

        public bool BuildAsSubAsset => Wizard.buildAsSubAsset;

        public IEnumerable<ExpressionItem> CollectExpressionItems() => Wizard.CollectExpressionItems();
    }
}