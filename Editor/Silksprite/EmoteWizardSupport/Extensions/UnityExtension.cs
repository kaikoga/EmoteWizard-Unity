using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class UnityExtension
    {
        static IUndoable Undoable => new EditorUndoable("Emote Wizard");

        public static T EnsureComponent<T>(this Component component, Action<T> initializer = null) 
            where T : Component
        {
            return Undoable.EnsureComponent(component, initializer);
        }

        public static GameObject AddChildGameObject(this Component component, string path)
        {
            return Undoable.AddChildGameObject(component, path);
        }

        public static T AddChildComponent<T>(this Component component, string path = null, Action<T> initializer = null)
            where T : Component
        {
            return Undoable.AddChildComponent(component, path, initializer);
        }

        public static T FindOrCreateChildComponent<T>(this Component component, string path = null, Action<T> initializer = null)
            where T : Component
        {
            return Undoable.AddChildComponent(component, path, initializer);
        }

        public static T AddChildComponentAndSelect<T>(this Component component, string path = null)
        where T : Component
        {
            var result = component.AddChildComponent<T>(path);
            Selection.SetActiveObjectWithContext(result.gameObject, result);
            return result;
        }
    }
}