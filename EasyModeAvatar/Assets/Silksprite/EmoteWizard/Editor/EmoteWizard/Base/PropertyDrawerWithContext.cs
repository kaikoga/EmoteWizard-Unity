using EmoteWizard.Base.DrawerContexts;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Base
{
    public abstract class PropertyDrawerWithContext<T> : PropertyDrawer
    where T : DrawerContextBase<T>, new()
    {
        static T _context;

        protected static T StartContext(T context)
        {
            if (_context != null) Debug.LogWarning("Internal: context is not disposed");
            return _context = context;
        }

        protected internal static void EndContext() => _context = null;

        protected T EnsureContext(SerializedProperty property)
        {
            if (_context != null) return _context;
            Debug.LogWarning("Internal: context is null", property.serializedObject.targetObject);
            _context = new T();
            return _context;
        }
    }
}