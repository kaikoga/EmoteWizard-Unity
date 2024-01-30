using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public interface IUndoable
    {
        void RecordObject(Object obj);

        GameObject CreateGameObject(string name);

        T AddComponent<T>(GameObject component) where T : Component;
        T AddComponent<T>(Component component) where T : Component;

        void DestroyObject(Object obj);
        
        void SetActiveObjectWithContext(Object obj, Object context);
        
        string GetUniqueNameForSibling(Transform parent, string name);
    }
}