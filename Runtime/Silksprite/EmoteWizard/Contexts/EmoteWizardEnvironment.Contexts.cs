using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        readonly List<IBehaviourContext> _contexts = new List<IBehaviourContext>();

        void CollectOtherContexts()
        {
            var contexts = GetComponentsInChildren<IContextProvider>(true).Select(component => component.ToContext(this));
            foreach (var context in contexts)
            {
                if (_contexts.Any(c => c.GetType() == context.GetType() && c.GameObject == context.GameObject)) continue;
                _contexts.Add(context);
            }
        }

        public bool HasContext<T>() => _contexts.OfType<T>().Any();

        public T GetContext<T>() where T : IBehaviourContext
        {
            var context = _contexts.OfType<T>().FirstOrDefault();
            if (context == null)
            {
                context = (T)Activator.CreateInstance(typeof(T), this);
                _contexts.Add(context);
            }
            return context;
        }
    }
}