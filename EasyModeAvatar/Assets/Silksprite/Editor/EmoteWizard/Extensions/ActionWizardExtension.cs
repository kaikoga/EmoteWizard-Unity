using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ActionWizardExtension
    {
        [Obsolete]
        public static void RepopulateDefaultActionEmotes(this ActionWizard actionWizard)
        {
            actionWizard.legacyActionEmotes = DefaultActionEmote.PopulateDefaultActionEmotes();
            actionWizard.legacyAfkEmotes = new List<ActionEmote>();
            actionWizard.defaultAfkEmote = DefaultActionEmote.PopulateDefaultAfkEmote();
        }

        public static void RepopulateDefaultAfkEmote(this ActionWizard actionWizard)
        {
            actionWizard.defaultAfkEmote = DefaultActionEmote.PopulateDefaultAfkEmote();
        }

        public static RuntimeAnimatorController BuildOutputAsset(this ActionWizard actionWizard)
        {
            var builder = new ActionControllerBuilder(actionWizard, "Action/@@@Generated@@@Action.controller");

            builder.BuildActionLayer();
            builder.BuildParameters();

            return actionWizard.outputAsset;
        }
    }
}