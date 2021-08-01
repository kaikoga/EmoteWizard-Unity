using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class DrawerContextBase<T, TContext> : IDisposable
        where TContext : DrawerContextBase<T, TContext>
    {
        static TContext _context;

        public void Dispose() => DoEndContext();

        public DrawerContextDisposable<T, TContext> StartContext()
        {
            if (this is TContext context)
            {
                DoStartContext(context);
                return new DrawerContextDisposable<T, TContext>(context);
            }

            Debug.LogWarning($"Internal: Failed to start context {typeof(TContext)}");
            return new DrawerContextDisposable<T, TContext>(null);
        }

        static void DoStartContext(TContext context)
        {
            if (_context != null) Debug.LogWarning("Internal: context is not disposed");
            _context = context;
        }

        internal static void DoEndContext() => _context = null;

        internal static TContext EnsureContext(SerializedProperty property)
        {
            if (_context != null) return _context;
            Debug.LogWarning("Internal: context is null", property.serializedObject.targetObject);
            _context = CreateContext();
            return _context;
        }

        internal static TContext EnsureContext(T item)
        {
            if (_context != null) return _context;
            Debug.LogWarning($"Internal: context is null: {item.GetType().Name}");
            _context = CreateContext();
            return _context;
        }

        internal static TContext EnsureContext()
        {
            if (_context != null) return _context;
            Debug.LogWarning("Internal: context is null");
            _context = CreateContext();
            return _context;
        }

        static TContext CreateContext()
        {
            var type = typeof(TContext);
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                return (TContext) ctor.Invoke(null);
            }

            throw new InvalidOperationException("Internal: could not instantiate context");
        }
    }

    public class DrawerContextDisposable<T, TContext> : IDisposable
        where TContext : DrawerContextBase<T, TContext>
    {
        public TContext Context { get; }

        public DrawerContextDisposable(TContext context)
        {
            Context = context;
        }

        public void Dispose() => DrawerContextBase<T, TContext>.DoEndContext();
    }
}