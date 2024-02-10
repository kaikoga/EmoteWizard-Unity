using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace Silksprite.EmoteWizard.Contexts
{
#if EW_VRCSDK3_AVATARS
    public class ExpressionContext : OutputContextBase<ExpressionConfig, VRCExpressionsMenu>
#else
    public class ExpressionContext : ContextBase<ExpressionConfig>
#endif
    {
        List<ExpressionItem> _expressionItems;

#if EW_VRCSDK3_AVATARS
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
#endif

        public readonly bool BuildAsSubAsset = true;

        [UsedImplicitly]
        public ExpressionContext(EmoteWizardEnvironment env) : base(env)
        {
        }

        public ExpressionContext(EmoteWizardEnvironment env, ExpressionConfig config) : base(env, config)
        {
            if (env.PersistGeneratedAssets)
            {
#if EW_VRCSDK3_AVATARS
                _outputAsset = config.outputAsset;
#endif
            }

            BuildAsSubAsset = config.buildAsSubAsset;
        }

        public override void DisconnectOutputAssets()
        {
#if EW_VRCSDK3_AVATARS
            OutputAsset = null;
#endif
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