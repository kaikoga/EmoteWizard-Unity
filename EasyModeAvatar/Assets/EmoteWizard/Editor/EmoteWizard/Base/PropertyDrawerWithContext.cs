using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Base
{
    public abstract class PropertyDrawerWithContext<T> : PropertyDrawer
    where T : PropertyDrawerWithContext<T>.ContextBase, new()
    {
        static T _context;

        protected static void StartContext(T context) => _context = context;

        public static void EndContext() => _context = null;

        protected T EnsureContext(SerializedProperty property)
        {
            if (_context != null) return _context;
            Debug.LogWarning("Internal: context is null", property.serializedObject.targetObject);
            _context = new T();
            return _context;
        }

        public abstract class ContextBase
        {
            readonly EmoteWizardRoot _emoteWizardRoot;

            protected ContextBase(EmoteWizardRoot emoteWizardRoot)
            {
                _emoteWizardRoot = emoteWizardRoot;
            }

            public EmoteWizardRoot EmoteWizardRoot => _emoteWizardRoot ? _emoteWizardRoot : Object.FindObjectOfType<EmoteWizardRoot>();
        }
    }
}