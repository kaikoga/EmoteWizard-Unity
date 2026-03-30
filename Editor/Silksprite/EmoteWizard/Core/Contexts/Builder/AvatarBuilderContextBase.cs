using Silksprite.EmoteWizardSupport.Undoable;

namespace Silksprite.EmoteWizard.Contexts.Builder
{
    public abstract class AvatarBuilderContextBase : ContextBase
    {
        protected AvatarBuilderContextBase(EmoteWizardEnvironment environment) : base(environment) { }

        public abstract void CleanupAvatar();

        public abstract void BuildAvatar(IUndoable undoable, bool manualBuild);
    }
}
