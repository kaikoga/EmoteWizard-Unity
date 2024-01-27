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
    [CustomEditor(typeof(CustomActionWizard))]
    public class CustomActionWizardEditor : EmoteWizardEditorBase<CustomActionWizard>
    {
        LocalizedProperty _hasExpressionItemSource;
        LocalizedProperty _emoteSequenceFactoryKind;
        LocalizedProperty _actionIndex;
        LocalizedProperty _itemPath;
        LocalizedProperty _parameterName;

        void OnEnable()
        {
            _hasExpressionItemSource = Lop(nameof(CustomActionWizard.hasExpressionItemSource), Loc("CustomActionWizard::hasExpressionItemSource"));
            _emoteSequenceFactoryKind = Lop(nameof(EmoteItemWizard.emoteSequenceFactoryKind), Loc("CustomActionWizard::emoteSequenceFactoryKind"));
            _actionIndex = Lop(nameof(CustomActionWizard.actionIndex), Loc("CustomActionWizard::actionIndex"));
            _itemPath = Lop(nameof(CustomActionWizard.itemPath), Loc("CustomActionWizard::itemPath"));
            _parameterName = Lop(nameof(CustomActionWizard.parameterName), Loc("CustomActionWizard::parameterName"));
        }

        protected override void OnInnerInspectorGUI()
        {
            using (new LabelWidthScope(200f))
            using (var checkInvalid = new CheckInvalidValueScope())
            {
                EmoteWizardGUILayout.Prop(_itemPath);
                EmoteWizardGUILayout.Prop(_actionIndex);

                using (new BoxLayoutScope())
                {
                    EmoteWizardGUILayout.Header(Loc("CustomActionWizard::Advanced Settings"));
                    EmoteWizardGUILayout.Prop(_parameterName);
                    EmoteWizardGUILayout.Prop(_hasExpressionItemSource);
                    EmoteWizardGUILayout.Prop(_emoteSequenceFactoryKind);
                }

                serializedObject.ApplyModifiedProperties();

                using (new EditorGUI.DisabledScope(checkInvalid.IsInvalid))
                {
                    if (EmoteWizardGUILayout.Undoable(Loc("CustomActionWizard::Add"), "Add from Custom Action Wizard") is IUndoable undoable)
                    {
                        soleTarget.Explode(undoable, true);
                    }
                }
            }
        }
    }
}