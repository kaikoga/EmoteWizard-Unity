using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public abstract class ParameterEmoteSourceBase : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ParameterEmote> parameterEmotes = new List<ParameterEmote>();
        
        public abstract string LayerName { get; }

        public void GenerateParameters(ParametersWizard parametersWizard)
        {
            parametersWizard.RefreshParameters();

            var allParameterItems = parametersWizard.AllParameterItems.ToArray();

            var animationWizardBase = LayerName == "FX" ? (AnimationWizardBase)EmoteWizardRoot.GetWizard<FxWizard>() : EmoteWizardRoot.GetWizard<GestureWizard>();
            var existingParameterNames = animationWizardBase.CollectParameterEmotes(true, true).Select(p => p.parameter).ToList();

            foreach (var existingEmote in parameterEmotes)
            {
                existingEmote.CollectStates(allParameterItems.Single(p => p.name == existingEmote.parameter));
            }

            var parameterItemsToCreate = parametersWizard.ParameterItems
                .Where(p => p.enabled && !existingParameterNames.Contains(p.name));

            foreach (var parameterItem in parameterItemsToCreate)
            {
                var parameter = new ParameterEmote
                {
                    enabled = true,
                    name = parameterItem.name,
                    valueKind = parameterItem.ValueKind,
                    parameter = parameterItem.name,
                    emoteKind = ParameterEmoteKind.Transition
                };
                parameter.CollectStates(parameterItem);
                parameterEmotes.Add(parameter);
            }
        }
    }
}