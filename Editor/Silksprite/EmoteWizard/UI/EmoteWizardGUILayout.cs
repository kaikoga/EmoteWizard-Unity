using System;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.UI
{
    public static class EmoteWizardGUILayout
    {
        static readonly GUIStyle HeaderStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(0, 0, 2, 0)
        };

        static readonly Color ConfigUIColor = new Color(0.80f, 0.80f, 0.82f);
        static readonly Color OutputUIColor = new Color(0.3f, 1.0f, 0.9f);

        public static void Header(string label) => EditorGUILayout.LabelField(label, HeaderStyle);

        public static IUndoable Undoable(string name) => Undoable(name, name);

        public static IUndoable Undoable(string name, string undoLabel) => GUILayout.Button(name) ? new EditorUndoable(undoLabel) : null;

        public static void ConfigUIArea(Action action)
        {
            using (new BoxLayoutScope(ConfigUIColor))
            {
                action();
            }
        }

        public static void OutputUIArea(bool persistGeneratedAssets, Action action) => OutputUIArea(persistGeneratedAssets, "Output zone", action);

        public static void OutputUIArea(bool persistGeneratedAssets, string label, Action action)
        {
            if (!persistGeneratedAssets) return;
            using (new BoxLayoutScope(OutputUIColor))
            {
                if (!string.IsNullOrEmpty(label)) GUILayout.Label(label);
                action();
            }
        }

        public static void Tutorial(EmoteWizardEnvironment environment, Action action)
        {
            if (environment?.ShowTutorial != true) return;
            using (new BoxLayoutScope()) action();
        }
        
        public static void Tutorial(EmoteWizardEnvironment environment, string message)
        {
            Tutorial(environment, () => EditorGUILayout.HelpBox(message.Nowrap(), MessageType.Info));
        }
    }
}