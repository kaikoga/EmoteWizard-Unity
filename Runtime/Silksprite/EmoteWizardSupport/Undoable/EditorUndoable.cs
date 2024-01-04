#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public class EditorUndoable : IUndoable
    {
        readonly string _undoName;
        public EditorUndoable(string undoName) => _undoName = undoName;
        
        public GameObject CreateGameObject(string name)
        {
            var gameObject = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(gameObject, _undoName);
            return gameObject;
        }

        public T AddComponent<T>(Component component) where T : Component => Undo.AddComponent<T>(component.gameObject);
    }
}

#endif