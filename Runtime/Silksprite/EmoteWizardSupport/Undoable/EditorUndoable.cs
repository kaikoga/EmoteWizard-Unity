#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public class EditorUndoable : IUndoable
    {
        readonly string _undoName;
        public EditorUndoable(string undoName) => _undoName = undoName;

        public void RecordObject(Object obj) => Undo.RecordObject(obj, _undoName);

        public GameObject CreateGameObject(string name)
        {
            var gameObject = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(gameObject, _undoName);
            return gameObject;
        }

        public T AddComponent<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.AddComponent<T>();
            Undo.RegisterCreatedObjectUndo(component, _undoName);
            return component;
        }

        public T AddComponent<T>(Component component) where T : Component => AddComponent<T>(component.gameObject);

        public void DestroyObject(Object obj) => Undo.DestroyObjectImmediate(obj);

        public void SetActiveObjectWithContext(Object obj, Object context) => Selection.SetActiveObjectWithContext(obj, context);

        public string GetUniqueNameForSibling(Transform parent, string name) => GameObjectUtility.GetUniqueNameForSibling(parent, name);
    }
}

#endif