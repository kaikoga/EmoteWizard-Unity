using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class ContextBase<TWizard> : IBehaviourContext
        where TWizard : EmoteWizardBase
    {
        protected readonly TWizard Wizard;

        public EmoteWizardEnvironment Environment { get; }
        public GameObject GameObject => Wizard.gameObject;

        protected ContextBase(EmoteWizardEnvironment env)
        {
            Environment = env;
        }

        protected ContextBase(TWizard wizard)
        {
            Environment = wizard.Environment;
            Wizard = wizard;
        }

        public abstract void DisconnectOutputAssets();
    }

    public abstract class OutputContextBase<TWizard, TOut> : ContextBase<TWizard>, IOutputContext<TOut>
        where TWizard : EmoteWizardBase
    {
        protected OutputContextBase(EmoteWizardEnvironment env) : base(env) { }
        protected OutputContextBase(TWizard wizard) : base(wizard) { }
        public abstract TOut OutputAsset { get; set; }
    }
}