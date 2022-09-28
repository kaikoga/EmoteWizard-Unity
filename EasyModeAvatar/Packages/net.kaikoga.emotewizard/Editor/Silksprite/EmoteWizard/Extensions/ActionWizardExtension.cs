using Silksprite.EmoteWizard.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ActionWizardExtension
    {
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