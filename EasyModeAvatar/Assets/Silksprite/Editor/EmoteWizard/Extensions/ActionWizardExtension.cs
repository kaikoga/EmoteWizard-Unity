using Silksprite.EmoteWizard.Internal;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ActionWizardExtension
    {
        public static void RepopulateDefaultActionEmotes(this ActionWizard actionWizard)
        {
            actionWizard.actionEmotes = DefaultActionEmote.PopulateDefaultActionEmotes();
            actionWizard.afkEmotes = DefaultActionEmote.PopulateDefaultAfkEmotes();
        }

        public static void BuildOutputAsset(this ActionWizard actionWizard)
        {
            var builder = new ActionControllerBuilder
            {
                ActionWizard = actionWizard,
                DefaultRelativePath = "Action/@@@Generated@@@Action.controller"
            };

            builder.BuildActionLayer();
            builder.BuildParameters();
        }
    }
}