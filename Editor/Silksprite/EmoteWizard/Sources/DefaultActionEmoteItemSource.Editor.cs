using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DefaultActionEmoteItemSource))]
    public class DefaultActionEmoteItemSourceEditor : EmoteWizardEditorBase<DefaultActionEmoteItemSource>
    {
        LocalizedProperty _defaultActionIndex = null!;

        void OnEnable()
        {
            _defaultActionIndex = Lop(nameof(DefaultActionEmoteItemSource.defaultActionIndex), Loc("DefaultActionEmoteItemSource::defaultActionIndex"));
        }

        protected override void OnInnerInspectorGUI()
        {
            LEditorGUILayout.PropAsEnumPopup<DefaultActionIndex>(_defaultActionIndex);

            serializedObject.ApplyModifiedProperties();
            
            if (EmoteWizardGUILayout.Undoable(Loc("DefaultActionEmoteItemSource::Unpack"), "Unpack Default Action Emote Item Source", out var undoable))
            {
                soleTarget.Explode(CachedEnv(), undoable, true);
            }
        }
    }
}
