using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Base
{
    public static class DrawerContext<T, TContext>
        where TContext : DrawerContextBase<T, TContext>
    {
        static TContext _context;

        internal static TContext StartContext(TContext context)
        {
            if (_context != null) Debug.LogWarning("Internal: context is not disposed");
            return _context = context;
        }

        internal static void EndContext() => _context = null;

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
}