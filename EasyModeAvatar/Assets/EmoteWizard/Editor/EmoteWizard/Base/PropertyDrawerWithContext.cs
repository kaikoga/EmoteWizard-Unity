using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EmoteWizard.Base
{
    public abstract class PropertyDrawerWithContext<T> : PropertyDrawer
    where T : PropertyDrawerWithContext<T>.ContextBase, new()
    {
        static T _context;

        protected static T StartContext(T context)
        {
            if (_context != null) Debug.LogWarning("Internal: context is not disposed");
            return _context = context;
        }

        static void EndContext() => _context = null;

        protected T EnsureContext(SerializedProperty property)
        {
            if (_context != null) return _context;
            Debug.LogWarning("Internal: context is null", property.serializedObject.targetObject);
            _context = new T();
            return _context;
        }

        public abstract class ContextBase : IDisposable
        {
            readonly EmoteWizardRoot _emoteWizardRoot;

            protected ContextBase(EmoteWizardRoot emoteWizardRoot)
            {
                _emoteWizardRoot = emoteWizardRoot;
            }

            public EmoteWizardRoot EmoteWizardRoot => _emoteWizardRoot ? _emoteWizardRoot : Object.FindObjectOfType<EmoteWizardRoot>();

            public void Dispose() => EndContext();
        }
    }
}