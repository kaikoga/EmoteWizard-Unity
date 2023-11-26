using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
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

        public readonly bool BuildAsSubAsset = true;

        [UsedImplicitly]
        public ExpressionContext(EmoteWizardEnvironment env) : base(env) { }
        public ExpressionContext(EmoteWizardEnvironment env, ExpressionWizard wizard) : base(env, wizard)
        {
            if (env.PersistGeneratedAssets)
            {
                _outputAsset = wizard.outputAsset;
            }

            BuildAsSubAsset = Wizard.buildAsSubAsset;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
        }

        public IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            return Environment.GetComponentsInChildren<IExpressionItemSource>().SelectMany(source => source.ExpressionItems);
        }
    }
}