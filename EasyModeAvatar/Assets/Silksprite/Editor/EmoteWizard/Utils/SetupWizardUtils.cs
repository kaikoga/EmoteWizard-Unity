using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;

namespace Silksprite.EmoteWizard.Utils
{
    public static class SetupWizardUtils
    {
        public static void RepopulateParameters(ParametersWizard parametersWizard)
        {
            parametersWizard.parameterItems = new List<ParameterItem>();
            parametersWizard.ForceRefreshParameters();
        }

        public static void RepopulateDefaultExpressionItems(ExpressionWizard expressionWizard)
        {
            expressionWizard.expressionItems = new List<ExpressionItem>();
            PopulateDefaultExpressionItems(expressionWizard);
        }

        public static void PopulateDefaultExpressionItems(ExpressionWizard expressionWizard)
        {
            var icon = VrcSdkAssetLocator.PersonDance();
            var expressionItems = Enumerable.Range(1, 8)
                .Select(i => ExpressionItem.PopulateDefault(icon, expressionWizard.defaultPrefix, i));
            if (expressionWizard.expressionItems == null) expressionWizard.expressionItems = new List<ExpressionItem>();
            expressionWizard.expressionItems.AddRange(expressionItems);
            expressionWizard.expressionItems = expressionWizard.expressionItems
                .DistinctBy(item => item.path)
                .ToList();
        }

        public static void RepopulateDefaultEmotes(AnimationWizardBase animationWizardBase)
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            animationWizardBase.emotes = newEmotes;
        }

        public static void RepopulateDefaultEmotes14(AnimationWizardBase animationWizardBase)
        {
            var newEmotes = Enumerable.Empty<Emote>()
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther),
                        control = EmoteControl.Populate(handSign)
                    }))
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.NotEqual),
                        control = EmoteControl.Populate(handSign)
                    }))
                .ToList();
            animationWizardBase.emotes = newEmotes;
        }

        public static void RepopulateParameterEmotes(ParametersWizard parametersWizard, AnimationWizardBase animationWizardBase)
        {
            parametersWizard.TryRefreshParameters();
            animationWizardBase.parameterEmotes = new List<ParameterEmote>();
            animationWizardBase.RefreshParameters(parametersWizard);
        }
    }
}