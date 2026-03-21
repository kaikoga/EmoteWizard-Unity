using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DefaultActionEmoteItemSource))]
    public class DefaultActionEmoteItemSourceEditor : EmoteWizardEditorBase<DefaultActionEmoteItemSource>
    {
        LocalizedProperty _defaultActionIndex;

        void OnEnable()
        {
            _defaultActionIndex = Lop(nameof(DefaultActionEmoteItemSource.defaultActionIndex), Loc("DefaultActionEmoteItemSource::defaultActionIndex"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            LEditorGUILayout.Prop(_defaultActionIndex);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
