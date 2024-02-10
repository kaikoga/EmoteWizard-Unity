using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace Silksprite.EmoteWizard.Contexts
{

#if EW_VRCSDK3_AVATARS
    public class ParametersContext : OutputContextBase<ParametersConfig, VRCExpressionParameters>
#else
    public class ParametersContext : ContextBase<ParametersConfig>
#endif

    {
        ParametersSnapshot _snapshot;

#if EW_VRCSDK3_AVATARS
        VRCExpressionParameters _outputAsset;
        public override VRCExpressionParameters OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Config) Config.outputAsset = value;
            }
        }
#endif

        [UsedImplicitly]
        public ParametersContext(EmoteWizardEnvironment env) : base(env) { }
        public ParametersContext(EmoteWizardEnvironment env, ParametersConfig config) : base(env, config)
        {
            if (env.PersistGeneratedAssets)
            {
#if EW_VRCSDK3_AVATARS
                _outputAsset = config.outputAsset;
#endif
            }
        }

        public override void DisconnectOutputAssets()
        {
#if EW_VRCSDK3_AVATARS
            OutputAsset = null;
#endif
        }

        IEnumerable<ParameterItem> CollectSourceParameterItems()
        {
            return Environment.GetComponentsInChildren<IParameterSource>(true)
                .SelectMany(source => source.ToParameterItems());
        }

        ParametersSnapshot BuildSnapshot()
        {
            var builder = new ParameterSnapshotBuilder();

            foreach (var expressionItem in Environment.GetContext<ExpressionContext>().AllExpressionItems())
            {
                if (!string.IsNullOrEmpty(expressionItem.parameter))
                {
                    builder.FindOrCreate(expressionItem.parameter).AddWriteValue(expressionItem.value);
                }

                if (!expressionItem.IsPuppet) continue;
                foreach (var subParameter in expressionItem.subParameters.Where(subParameter => !string.IsNullOrEmpty(subParameter)))
                {
                    builder.FindOrCreate(subParameter).AddWritePuppet(expressionItem.itemKind == ExpressionItemKind.TwoAxisPuppet);
                }
            }

            foreach (var emoteItem in Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems())
            {
                foreach (var condition in emoteItem.Trigger.conditions)
                {
                    builder.FindOrCreate(condition.parameter).AddReadValue(condition.kind, condition.threshold);
                }
            }

            foreach (var parameter in CollectSourceParameterItems())
            {
                builder.FindOrCreate(parameter.name).Import(parameter);
            }

            return builder.ToSnapshot();
        }

        public ParametersSnapshot Snapshot() => _snapshot = _snapshot ?? BuildSnapshot();
    }
}