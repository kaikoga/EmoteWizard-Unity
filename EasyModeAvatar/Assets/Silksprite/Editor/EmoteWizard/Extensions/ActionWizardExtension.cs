using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ActionWizardExtension
    {
        public static void RepopulateDefaultActionEmotes(this ActionWizard actionWizard)
        {
            actionWizard.actionEmotes = DefaultActionEmote.PopulateDefaultActionEmotes();
            actionWizard.afkEmotes = new List<ActionEmote>();
            actionWizard.defaultAfkEmote = DefaultActionEmote.PopulateDefaultAfkEmote();
        }

        public static RuntimeAnimatorController BuildOutputAsset(this ActionWizard actionWizard)
        {
            var builder = new ActionControllerBuilder
            {
                ActionWizard = actionWizard,
                DefaultRelativePath = "Action/@@@Generated@@@Action.controller"
            };

            builder.BuildActionLayer();
            builder.BuildParameters();

            return actionWizard.outputAsset;
        }
    }
}