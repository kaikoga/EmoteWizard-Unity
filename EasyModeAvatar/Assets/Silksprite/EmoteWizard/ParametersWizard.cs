using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ParametersWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionParameters outputAsset;

        [NonSerialized] public List<ParameterItem> ParameterItems;
        [NonSerialized] public List<ParameterItem> DefaultParameterItems;

        public IEnumerable<ParameterItem> AllParameterItems
        {
            get
            {
                if (DefaultParameterItems == null) RefreshParameters();
                return ParameterItems.Where(item => item.enabled).Concat(DefaultParameterItems);
            }
        }

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }

        IEnumerable<ParameterItem> CollectSourceParameterItems()
        {
            return EmoteWizardRoot.GetComponentsInChildren<ParameterSource>()
                .SelectMany(source => source.parameterItems)
                .Where(item => item.enabled);
        }

        public bool AssertParameterExists(string parameterName, ParameterItemKind itemKind)
        {
            foreach (var item in AllParameterItems)
            {
                if (item.name != parameterName) continue;

                if (itemKind == ParameterItemKind.Auto || itemKind == item.itemKind) return true;
                Debug.LogWarning($"Ignored invalid parameter: {parameterName}, expected ${itemKind}, was ${item.itemKind}");
                return false;
            }

            Debug.LogWarning($"Ignored unknown parameter: {parameterName}");
            return false;
        }

        public void RefreshParameters()
        {
            var builder = new ExpressionParameterBuilder();

            var expressionWizard = GetWizard<ExpressionWizard>();
            if (expressionWizard != null)
            {
                foreach (var expressionItem in expressionWizard.CollectExpressionItems())
                {
                    if (!string.IsNullOrEmpty(expressionItem.parameter))
                    {
                        builder.FindOrCreate(expressionItem.parameter).AddUsage(expressionItem.value);
                    }

                    if (!expressionItem.IsPuppet) continue;
                    foreach (var subParameter in expressionItem.subParameters.Where(subParameter => !string.IsNullOrEmpty(subParameter)))
                    {
                        builder.FindOrCreate(subParameter).AddPuppetUsage(expressionItem.itemKind == ExpressionItemKind.TwoAxisPuppet);
                    }
                }
            }

            var gestureWizard = GetWizard<GestureWizard>();
            if (gestureWizard != null && gestureWizard.handSignOverrideEnabled)
            {
                builder.FindOrCreate(gestureWizard.handSignOverrideParameter).AddIndexUsage();
                foreach (var gestureEmote in gestureWizard.CollectEmotes().Where(emote => emote.OverrideAvailable))
                {
                    builder.FindOrCreate(gestureWizard.handSignOverrideParameter).AddUsage(gestureEmote.overrideIndex);
                }
            }
            var fxWizard = GetWizard<FxWizard>();
            if (fxWizard != null && fxWizard.handSignOverrideEnabled)
            {
                builder.FindOrCreate(fxWizard.handSignOverrideParameter).AddIndexUsage();
                foreach (var fxEmote in fxWizard.CollectEmotes().Where(emote => emote.OverrideAvailable))
                {
                    builder.FindOrCreate(fxWizard.handSignOverrideParameter).AddUsage(fxEmote.overrideIndex);
                }
            }
            var actionWizard = GetWizard<ActionWizard>();
            if (actionWizard != null && actionWizard.SelectableAfkEmotes)
            {
                builder.FindOrCreate(actionWizard.afkSelectParameter).AddIndexUsage();
                foreach (var afkEmote in actionWizard.CollectAfkEmotes())
                {
                    builder.FindOrCreate(actionWizard.afkSelectParameter).AddUsage(afkEmote.emoteIndex);
                }
            }

            builder.Import(CollectSourceParameterItems());

            ParameterItems = builder.ParameterItems.ToList();
            DefaultParameterItems = DefaultParameterItems ?? ParameterItem.PopulateDefaultParameters();
        }

        public VRCExpressionParameters.Parameter[] ToParameters()
        {
            RefreshParameters(); 
            return ParameterItems
                .Where(parameter => parameter.enabled)
                .Select(parameter => parameter.ToParameter())
                .ToArray();
        }
    }
}