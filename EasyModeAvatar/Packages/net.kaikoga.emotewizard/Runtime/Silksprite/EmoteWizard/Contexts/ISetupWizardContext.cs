namespace Silksprite.EmoteWizard.Contexts
{
    public interface ISetupWizardContext : IBehaviourContext
    {
        bool IsSetupMode { get; }
    }
}