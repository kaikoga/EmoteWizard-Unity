using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ParameterSource))]
    public class ParameterSourceEditor : EmoteWizardEditorBase<ParameterSource>
    {
        LocalizedProperty _name;
        LocalizedProperty _itemKind;
        LocalizedProperty _defaultValue;
        LocalizedProperty _saved;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(ParameterSource.parameterItem), Loc("ParameterSource::parameterItem"));

            _name = serializedItem.Lop(nameof(ParameterItem.name), Loc("ParameterItem::name"));
            _itemKind = serializedItem.Lop(nameof(ParameterItem.itemKind), Loc("ParameterItem::itemKind"));
            _defaultValue = serializedItem.Lop(nameof(ParameterItem.defaultValue), Loc("ParameterItem::defaultValue"));
            _saved = serializedItem.Lop(nameof(ParameterItem.saved), Loc("ParameterItem::saved"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_name);
            EmoteWizardGUILayout.Prop(_itemKind);
            EmoteWizardGUILayout.Prop(_defaultValue);
            EmoteWizardGUILayout.Prop(_saved);

            serializedObject.ApplyModifiedProperties();
        }
    }
}