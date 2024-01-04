using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class UndoableExtension
    {
        public static void AddChildComponentAndSelect<T>(this IUndoable undoable, Component component, string path = null)
        where T : Component
        {
            var result = undoable.AddChildComponent<T>(component, path);
            Selection.SetActiveObjectWithContext(result.gameObject, result);
        }
    }
}