using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ParameterSource))]
    public class ParameterSourceEditor : EmoteWizardEditorBase<ParameterSource>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRCPlatforms;

        LocalizedProperty _name = null!;
        LocalizedProperty _itemKind = null!;
        LocalizedProperty _defaultValue = null!;
        LocalizedProperty _saved = null!;
        LocalizedProperty _synced = null!;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(ParameterSource.parameterItem), Loc("ParameterSource::parameterItem"));

            _name = serializedItem.Lop(nameof(ParameterItem.name), Loc("ParameterItem::name"));
            _itemKind = serializedItem.Lop(nameof(ParameterItem.itemKind), Loc("ParameterItem::itemKind"));
            _defaultValue = serializedItem.Lop(nameof(ParameterItem.defaultValue), Loc("ParameterItem::defaultValue"));
            _saved = serializedItem.Lop(nameof(ParameterItem.saved), Loc("ParameterItem::saved"));
            _synced = serializedItem.Lop(nameof(ParameterItem.synced), Loc("ParameterItem::synced"));
        }

        protected override void OnInnerInspectorGUI()
        {
            LEditorGUILayout.Prop(_name);
            LEditorGUILayout.PropAsEnumPopup<ParameterItemKind>(_itemKind);
            EmoteWizardGUILayout.PropAsParameterValue(_defaultValue, (ParameterItemKind)_itemKind.Property.intValue);
            LEditorGUILayout.Prop(_saved);
            LEditorGUILayout.Prop(_synced);

            serializedObject.ApplyModifiedProperties();
        }
    }
}