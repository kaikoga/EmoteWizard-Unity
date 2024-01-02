using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        List<IBehaviourContext> _contextsCache;
        IEnumerable<IBehaviourContext> ContextsCache => _contextsCache ?? (_contextsCache = CollectContexts());

        List<IBehaviourContext> CollectContexts()
        {
            _contextsCache = new List<IBehaviourContext>();
            var contexts = GetComponentsInChildren<IContextProvider>(true).Select(component => component.ToContext(this));
            foreach (var context in contexts)
            {
                if (_contextsCache.Any(c => c.GetType() == context.GetType() && c.GameObject == context.GameObject)) continue;
                _contextsCache.Add(context);
            }
            return _contextsCache;
        }

        public bool HasContext<T>()
        {
            return ContextsCache.OfType<T>().Any();
        }

        public T GetContext<T>() where T : IBehaviourContext
        {
            var context = ContextsCache.OfType<T>().FirstOrDefault();
            if (context == null)
            {
                context = (T)Activator.CreateInstance(typeof(T), this);
                _contextsCache.Add(context);
            }
            return context;
        }
    }
}