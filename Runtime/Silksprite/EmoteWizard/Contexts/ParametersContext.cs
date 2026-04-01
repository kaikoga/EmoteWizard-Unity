using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.DataObjects.Internal.Builders;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Templates.Impl;

#if EW_VRCSDK3_AVATARS
using System;
using VRC.Dynamics;
using VRC.SDK3.Avatars.ScriptableObjects;
using VRC.SDK3.Dynamics.Contact.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;
#endif

namespace Silksprite.EmoteWizard.Contexts
{

#if EW_VRCSDK3_AVATARS
    public class ParametersContext : OutputContextBase<ParametersConfig, VRCExpressionParameters>
#else
    public class ParametersContext : ContextBase<ParametersConfig>
#endif

    {
        ParametersSnapshot? _snapshot;
        public ParametersSnapshot Snapshot() => _snapshot ??= BuildSnapshot();

#if EW_VRCSDK3_AVATARS
        VRCExpressionParameters? _outputAsset;
        public override VRCExpressionParameters? OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Config != null) Config.outputAsset = value;
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
            return Environment.GetContext<EmoteTemplateContext>()
                .UnpackedTemplates<ParameterItemTemplate>()
                .SelectMany(template => template.ToParameterItems());
        }

        ParametersSnapshot BuildSnapshot()
        {
            var builder = ParametersSnapshot.Builder();
            Environment.GetPlatformFeatures().BuildDefaultParameters(builder);

            foreach (var expressionItem in Environment.GetContext<ExpressionContext>().AllExpressionItems())
            {
                if (!string.IsNullOrEmpty(expressionItem.parameter))
                {
                    builder.FindOrCreate(expressionItem.parameter).AddWriteValue(expressionItem.value, expressionItem.itemKind.ToWriteSourceKind());
                }

                if (!expressionItem.IsPuppet) continue;
                foreach (var subParameter in expressionItem.subParameters.Where(subParameter => !string.IsNullOrEmpty(subParameter)))
                {
                    builder.FindOrCreate(subParameter).AddWritePuppet(expressionItem.itemKind.ToWriteSourceKind());
                }
            }

            foreach (var parameter in CollectSourceParameterItems())
            {
                builder.FindOrCreate(parameter.name).Import(parameter);
            }

#if EW_VRCSDK3_AVATARS
            if (Environment.AvatarRoot)
            {
                foreach (var contactReceiver in Environment.AvatarRoot.GetComponentsInChildren<VRCContactReceiver>())
                {
                    var parameter = contactReceiver.parameter;
                    if (string.IsNullOrEmpty(parameter)) continue;

                    switch (contactReceiver.receiverType)
                    {
                        case ContactReceiver.ReceiverType.Constant:
                        case ContactReceiver.ReceiverType.OnEnter:
                            builder.FindOrCreateImplicit(parameter).AddWriteValue(ParameterWriteUsageKind.Auto, 1f, ParameterWriteSourceKind.NoUI);
                            break;
                        case ContactReceiver.ReceiverType.Proximity:
                            builder.FindOrCreateImplicit(parameter).AddWriteValue(ParameterWriteUsageKind.Float, 1f, ParameterWriteSourceKind.NoUI);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                foreach (var physBone in Environment.AvatarRoot.GetComponentsInChildren<VRCPhysBone>())
                {
                    var parameter = physBone.parameter;
                    if (string.IsNullOrEmpty(parameter)) continue;

                    builder.FindOrCreateImplicit($"{parameter}_IsGrabbed").AddWriteValue(ParameterWriteUsageKind.Bool, 1f, ParameterWriteSourceKind.NoUI);
                    builder.FindOrCreateImplicit($"{parameter}_IsPosed").AddWriteValue(ParameterWriteUsageKind.Bool, 1f, ParameterWriteSourceKind.NoUI);
                    builder.FindOrCreateImplicit($"{parameter}_Angle").AddWriteValue(ParameterWriteUsageKind.Float, 1f, ParameterWriteSourceKind.NoUI);
                    builder.FindOrCreateImplicit($"{parameter}_Stretch").AddWriteValue(ParameterWriteUsageKind.Float, 1f, ParameterWriteSourceKind.NoUI);
                }
            }
#endif

            foreach (var emoteItem in Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems())
            {
                foreach (var condition in emoteItem.Trigger.Conditions)
                {
                    builder.FindOrCreate(condition.Parameter).AddReadValue(condition.Value);
                }
            }
            
            return builder.ToSnapshot();
        }
    }
}