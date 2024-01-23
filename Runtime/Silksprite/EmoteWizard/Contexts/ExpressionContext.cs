using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ExpressionContext : OutputContextBase<ExpressionConfig, VRCExpressionsMenu>
    {
        List<ExpressionItem> _expressionItems;

        VRCExpressionsMenu _outputAsset;

        public override VRCExpressionsMenu OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Config) Config.outputAsset = value;
            }
        }

        public readonly bool BuildAsSubAsset = true;

        [UsedImplicitly]
        public ExpressionContext(EmoteWizardEnvironment env) : base(env)
        {
        }

        public ExpressionContext(EmoteWizardEnvironment env, ExpressionConfig config) : base(env, config)
        {
            if (env.PersistGeneratedAssets)
            {
                _outputAsset = config.outputAsset;
            }

            BuildAsSubAsset = config.buildAsSubAsset;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
        }

        IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            return Environment.GetComponentsInChildren<IExpressionItemSource>(true)
                .SelectMany(source => source.ToExpressionItems());
        }

        public IEnumerable<ExpressionItem> AllExpressionItems()
        {
            return _expressionItems = _expressionItems ?? CollectExpressionItems().ToList();
        }
    }
}