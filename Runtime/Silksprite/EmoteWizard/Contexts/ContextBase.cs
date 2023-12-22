using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class ContextBase<TWizard> : IBehaviourContext
        where TWizard : EmoteWizardBase
    {
        [CanBeNull]
        protected readonly TWizard Wizard;

        public EmoteWizardEnvironment Environment { get; }
        public GameObject GameObject => Wizard != null ? Wizard.gameObject : null;

        protected ContextBase(EmoteWizardEnvironment env)
        {
            Environment = env;
        }

        protected ContextBase(EmoteWizardEnvironment env, TWizard wizard, bool alwaysPersist = false)
        {
            Environment = env;
            if (env.PersistGeneratedAssets || alwaysPersist) Wizard = wizard;
        }

        public abstract void DisconnectOutputAssets();
    }

    public abstract class OutputContextBase<TWizard, TOut> : ContextBase<TWizard>, IOutputContext<TOut>
        where TWizard : EmoteWizardBase
    {
        protected OutputContextBase(EmoteWizardEnvironment env) : base(env) { }
        protected OutputContextBase(EmoteWizardEnvironment env, TWizard wizard, bool alwaysPersist = false) : base(env, wizard, alwaysPersist) { }
        public abstract TOut OutputAsset { get; set; }
    }
}