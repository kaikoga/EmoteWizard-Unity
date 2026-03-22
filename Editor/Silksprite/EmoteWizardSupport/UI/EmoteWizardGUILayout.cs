using System;
using System.Diagnostics.CodeAnalysis;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class EmoteWizardGUILayout
    {
        const float GenerateButtonWidth = 64f;

        static readonly Color ConfigUIColor = new Color(0.80f, 0.80f, 0.82f);
        static readonly Color OutputUIColor = new Color(0.3f, 1.0f, 0.9f);

        public static void Undoable(LocalizedContent loc, Action<IUndoable> onClick) => Undoable(loc, loc.Tr, onClick);

        public static void Undoable(LocalizedContent loc, string undoLabel, Action<IUndoable> onClick)
        {
            if (LGUILayout.Button(loc)) onClick(new EditorUndoable(undoLabel)); 
        }

        public static bool Undoable(LocalizedContent loc, [MaybeNullWhen(false)] out IUndoable undoable) => Undoable(loc, loc.Tr, out undoable);

        public static bool Undoable(LocalizedContent loc, string undoLabel, [MaybeNullWhen(false)] out IUndoable undoable)
        {
            if (LGUILayout.Button(loc))
            {
                undoable = new EditorUndoable(undoLabel);
                return true;
            }
            else
            {
                undoable = null;
                return false;
            }
        }

        public static void ConfigUIArea(Action action)
        {
            using (new BoxLayoutScope(ConfigUIColor))
            {
                action();
            }
        }

        public static void OutputUIArea(bool persistGeneratedAssets, Action action) => OutputUIArea(persistGeneratedAssets, Loc("EWS::Output zone"), action);

        public static void OutputUIArea(bool persistGeneratedAssets, LocalizedContent? loc, Action action)
        {
            if (!persistGeneratedAssets) return;
            using (new BoxLayoutScope(OutputUIColor))
            {
                if (loc != null) LGUILayout.Label(loc);
                action();
            }
        }

        public static void PropWithGenerate(LocalizedProperty lop, Func<Object> generate)
        {
            using (new GUILayout.HorizontalScope())
            {
                LEditorGUILayout.Prop(lop);
                if (lop.Property.objectReferenceValue == null && LGUILayout.Button(Loc("EWS::Generate"), GUILayout.Width(GenerateButtonWidth)))
                {
                    lop.Property.objectReferenceValue = generate();
                }
            }
        }
    }
}