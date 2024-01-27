using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(EmoteItemWizard))]
    public class EmoteItemWizardEditor : EmoteWizardEditorBase<EmoteItemWizard>
    {
        LocalizedProperty _hasExpressionItemSource;
        LocalizedProperty _emoteSequenceFactoryKind;
        LocalizedProperty _itemPath;
        LocalizedProperty _hasGroupName;
        LocalizedProperty _groupName;
        LocalizedProperty _hasParameterName;
        LocalizedProperty _parameterName;

        void OnEnable()
        {
            _hasExpressionItemSource = Lop(nameof(EmoteItemWizard.hasExpressionItemSource), Loc("EmoteItemWizard::hasExpressionItemSource"));
            _emoteSequenceFactoryKind = Lop(nameof(EmoteItemWizard.emoteSequenceFactoryKind), Loc("EmoteItemWizard::emoteSequenceFactoryKind"));
            _itemPath = Lop(nameof(EmoteItemWizard.itemPath), Loc("EmoteItemWizard::itemPath"));
            _hasGroupName = Lop(nameof(EmoteItemWizard.hasGroupName), Loc("EmoteItemWizard::hasGroupName"));
            _groupName = Lop(nameof(EmoteItemWizard.groupName), Loc("EmoteItemWizard::groupName"));
            _hasParameterName = Lop(nameof(EmoteItemWizard.hasParameterName), Loc("EmoteItemWizard::hasParameterName"));
            _parameterName = Lop(nameof(EmoteItemWizard.parameterName), Loc("EmoteItemWizard::parameterName"));
        }

        protected override void OnInnerInspectorGUI()
        {
            using (new LabelWidthScope(200f))
            using (var checkInvalid = new CheckInvalidValueScope())
            {
                EmoteWizardGUILayout.Prop(_itemPath);

                EmoteWizardGUILayout.PropertyFoldout(_hasGroupName, () =>
                {
                    EmoteWizardGUILayout.Prop(_groupName);
                });

                EmoteWizardGUILayout.PropertyFoldout(_hasParameterName, () =>
                {
                    EmoteWizardGUILayout.Prop(_parameterName);
                });

                EmoteWizardGUILayout.Prop(_hasExpressionItemSource);
                EmoteWizardGUILayout.Prop(_emoteSequenceFactoryKind);

                serializedObject.ApplyModifiedProperties();

                using (new EditorGUI.DisabledScope(checkInvalid.IsInvalid))
                {
                    if (EmoteWizardGUILayout.Undoable(Loc("EmoteItemWizard::Add"), "Add From Emote Item Wizard") is IUndoable undoable)
                    {
                        soleTarget.Explode(undoable, true);
                    }
                }
            }
        }
    }
}