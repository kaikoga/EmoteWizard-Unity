using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        List<IContext> _contextsCache;
        IEnumerable<IContext> ContextsCache => _contextsCache ?? (_contextsCache = CollectContexts());

        List<IContext> CollectContexts()
        {
            _contextsCache = new List<IContext>();
            var contexts = GetComponentsInChildren<IContextProvider>(true).Select(component => component.ToContext(this));
            foreach (var context in contexts)
            {
                if (_contextsCache.Any(c => c.GetType() == context.GetType() && c is IBehaviourContext bc && bc.GameObject == context.GameObject)) continue;
                _contextsCache.Add(context);
            }
            return _contextsCache;
        }

        public bool HasContext<T>()
        where T : IContext
        {
            return ContextsCache.OfType<T>().Any();
        }

        public T GetContext<T>() where T : IContext
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