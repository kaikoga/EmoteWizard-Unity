using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class PropertyDrawerWithContext<T> : PropertyDrawer
    where T : DrawerContextBase<T>
    {
        static T _context;

        protected static T StartContext(T context)
        {
            if (_context != null) Debug.LogWarning("Internal: context is not disposed");
            return _context = context;
        }

        internal static void EndContext() => _context = null;

        protected T EnsureContext(SerializedProperty property)
        {
            if (_context != null) return _context;
            Debug.LogWarning("Internal: context is null", property.serializedObject.targetObject);
            _context = CreateContext();
            return _context;
        }

        static T CreateContext()
        {
            var type = typeof(T);
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                return (T) ctor.Invoke(null);
            }

            throw new InvalidOperationException("Internal: could not instantiate context");
        }
    }
}