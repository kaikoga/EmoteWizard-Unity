using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class ContextBase : IContext
    {
        public EmoteWizardEnvironment Environment { get; }
        protected ContextBase(EmoteWizardEnvironment env) => Environment = env;
    }

    public abstract class ContextBase<TConfig> : ContextBase, IBehaviourContext
        where TConfig : EmoteConfigBase
    {
        [CanBeNull]
        protected readonly TConfig Config;

        public GameObject GameObject => Config != null ? Config.gameObject : null;

        protected ContextBase(EmoteWizardEnvironment env) : base(env) { }

        protected ContextBase(EmoteWizardEnvironment env, TConfig config, bool alwaysPersist = false) : base(env)
        {
            if (env.PersistGeneratedAssets || alwaysPersist) Config = config;
        }

        public abstract void DisconnectOutputAssets();
    }

    public abstract class OutputContextBase<TWizard, TOut> : ContextBase<TWizard>, IOutputContext<TOut>
        where TWizard : EmoteConfigBase
    {
        protected OutputContextBase(EmoteWizardEnvironment env) : base(env) { }
        protected OutputContextBase(EmoteWizardEnvironment env, TWizard config, bool alwaysPersist = false) : base(env, config, alwaysPersist) { }
        public abstract TOut OutputAsset { get; set; }
    }
}