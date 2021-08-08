using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ParametersWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionParameters outputAsset;
        [SerializeField] public List<ParameterItem> parameterItems;
        [SerializeField] public List<ParameterItem> defaultParameterItems;

        public IEnumerable<ParameterItem> AllParameterItems => parameterItems.Concat(defaultParameterItems).Where(item => item.enabled);

        public bool AssertParameterExists(string parameterName)
        {
            var result = AllParameterItems.Any(item => item.name == parameterName);
            if (!result) Debug.LogWarning($"Ignored unknown parameter: {parameterName}");
            return result;
        }

        public void TryRefreshParameters()
        {
            var expressionWizard = GetWizard<ExpressionWizard>();
            if (expressionWizard == null)
            {
                Debug.LogWarning("ExpressionWizard not found. Parameters are unchanged.");
                return;
            }
            DoRefreshParameters(expressionWizard);
        }

        public void ForceRefreshParameters()
        {
            var expressionWizard = GetWizard<ExpressionWizard>();
            if (expressionWizard == null)
            {
                throw new Exception("ExpressionWizard not found. Parameters are unchanged.");
            }
            DoRefreshParameters(expressionWizard);
        }

        void DoRefreshParameters(ExpressionWizard expressionWizard)
        {
            var builder = new ExpressionParameterBuilder();

            if (parameterItems != null) builder.Import(parameterItems);

            foreach (var expressionItem in expressionWizard.expressionItems)
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

            var gestureWizard = GetWizard<GestureWizard>();
            if (gestureWizard != null && gestureWizard.handSignOverrideEnabled)
            {
                builder.FindOrCreate(gestureWizard.handSignOverrideParameter).AddIndexUsage();
                foreach (var gestureEmote in gestureWizard.emotes.Where(emote => emote.OverrideAvailable))
                {
                    builder.FindOrCreate(gestureWizard.handSignOverrideParameter).AddUsage(gestureEmote.overrideIndex);
                }
            }
            var fxWizard = GetWizard<FxWizard>();
            if (fxWizard != null && fxWizard.handSignOverrideEnabled)
            {
                builder.FindOrCreate(fxWizard.handSignOverrideParameter).AddIndexUsage();
                foreach (var fxEmote in fxWizard.emotes.Where(emote => emote.OverrideAvailable))
                {
                    builder.FindOrCreate(fxWizard.handSignOverrideParameter).AddUsage(fxEmote.overrideIndex);
                }
            }
            var actionWizard = GetWizard<ActionWizard>();
            if (actionWizard != null && actionWizard.SelectableAfkEmotes)
            {
                builder.FindOrCreate(actionWizard.afkSelectParameter).AddIndexUsage();
                foreach (var afkEmote in actionWizard.afkEmotes)
                {
                    builder.FindOrCreate(actionWizard.afkSelectParameter).AddUsage(afkEmote.emoteIndex);
                }
            }

            parameterItems = builder.ParameterItems.ToList();
            defaultParameterItems = ParameterItem.PopulateDefaultParameters(defaultParameterItems ?? new List<ParameterItem>());
        }

        public VRCExpressionParameters.Parameter[] ToParameters()
        {
            TryRefreshParameters(); 
            return parameterItems
                .Where(parameter => parameter.enabled)
                .Select(parameter => parameter.ToParameter())
                .ToArray();
        }
    }
}