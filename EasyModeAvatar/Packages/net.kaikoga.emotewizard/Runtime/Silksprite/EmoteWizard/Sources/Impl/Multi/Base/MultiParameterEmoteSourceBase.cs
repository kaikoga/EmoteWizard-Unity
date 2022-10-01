using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi.Base
{
    public abstract class MultiParameterEmoteSourceBase : EmoteWizardDataSourceBase, IParameterEmoteSourceBase
    {
        [SerializeField] public List<ParameterEmote> parameterEmotes = new List<ParameterEmote>();
        
        public IEnumerable<ParameterEmote> ParameterEmotes => parameterEmotes;

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
                GenerateSingleParameter(parameterItem);
            }
        }

        public void GenerateSingleParameter(ParameterItem parameterItem)
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