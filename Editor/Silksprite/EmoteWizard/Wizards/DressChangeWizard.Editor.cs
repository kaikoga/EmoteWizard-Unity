using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(DressChangeWizard))]
    public class DressChangeWizardEditor : EmoteWizardEditorBase<DressChangeWizard>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRChat;

        LocalizedProperty _expressionKind;
        LocalizedProperty _itemCount;
        LocalizedProperty _emoteSequenceFactoryKind;
        LocalizedProperty _itemPath;
        LocalizedProperty _hasGroupName;
        LocalizedProperty _groupName;
        LocalizedProperty _hasParameterName;
        LocalizedProperty _parameterName;

        void OnEnable()
        {
            _expressionKind = Lop(nameof(DressChangeWizard.expressionKind), Loc("DressChangeWizard::expressionKind"));
            _itemCount = Lop(nameof(DressChangeWizard.itemCount), Loc("DressChangeWizard::itemCount"));
            _emoteSequenceFactoryKind = Lop(nameof(DressChangeWizard.emoteSequenceFactoryKind), Loc("DressChangeWizard::emoteSequenceFactoryKind"));
            _itemPath = Lop(nameof(DressChangeWizard.itemPath), Loc("DressChangeWizard::itemPath"));
            _hasGroupName = Lop(nameof(DressChangeWizard.hasGroupName), Loc("DressChangeWizard::hasGroupName"));
            _groupName = Lop(nameof(DressChangeWizard.groupName), Loc("DressChangeWizard::groupName"));
            _hasParameterName = Lop(nameof(DressChangeWizard.hasParameterName), Loc("DressChangeWizard::hasParameterName"));
            _parameterName = Lop(nameof(DressChangeWizard.parameterName), Loc("DressChangeWizard::parameterName"));
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

                EmoteWizardGUILayout.Prop(_expressionKind);
                if ((DressChangeWizard.ExpressionKind)_expressionKind.Property.enumValueIndex != DressChangeWizard.ExpressionKind.SimpleToggle)
                {
                    EmoteWizardGUILayout.Prop(_itemCount);
                }
                EmoteWizardGUILayout.Prop(_emoteSequenceFactoryKind);

                serializedObject.ApplyModifiedProperties();

                using (new EditorGUI.DisabledScope(checkInvalid.IsInvalid))
                {
                    if (EmoteWizardGUILayout.Undoable(Loc("DressChangeWizard::Add"), "Add from Dress Change Wizard") is IUndoable undoable)
                    {
                        soleTarget.Explode(undoable, true);
                    }
                }
            }
        }
    }
}