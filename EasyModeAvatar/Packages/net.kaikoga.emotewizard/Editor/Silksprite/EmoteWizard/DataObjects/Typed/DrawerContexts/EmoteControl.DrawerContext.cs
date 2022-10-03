using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts
{
    [UsedImplicitly]
    public class EmoteControlDrawerContext : EmoteWizardDrawerContextBase<EmoteControl, EmoteControlDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;
        public readonly bool AsGesture;
        public readonly bool IsEditing;

        public EmoteControlDrawerContext() : base(null) { }
        public EmoteControlDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, bool asGesture, bool isEditing) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            AsGesture = asGesture;
            IsEditing = isEditing;
        }
    }
}