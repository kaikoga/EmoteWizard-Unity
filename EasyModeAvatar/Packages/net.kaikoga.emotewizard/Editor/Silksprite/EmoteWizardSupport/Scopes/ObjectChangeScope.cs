using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.Scopes
{
    public class ObjectChangeScope : IDisposable
    {
        readonly Object _obj;
        bool _changed;

        public bool Changed
        {
            get
            {
                if (EditorGUI.EndChangeCheck()) _changed = true;
                EditorGUI.BeginChangeCheck();
                return _changed;
            }
        }

        public ObjectChangeScope(Object obj) : this(obj, $"Inspector {obj.GetType().Name}")
        {
        }

        public ObjectChangeScope(Object obj, string undoName)
        {
            _obj = obj;
            Undo.RecordObject(_obj, undoName);
            EditorGUI.BeginChangeCheck();
        }

        public void Dispose()
        {
            if (EditorGUI.EndChangeCheck()) _changed = true;
            if (_changed) EditorUtility.SetDirty(_obj);
        }
    }
}