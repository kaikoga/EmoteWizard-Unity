using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(CustomActionWizard))]
    public class CustomActionWizardEditor : EmoteWizardEditorBase<CustomActionWizard>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRChat;

        LocalizedProperty _hasExpressionItemSource = null!;
        LocalizedProperty _emoteSequenceFactoryKind = null!;
        LocalizedProperty _actionIndex = null!;
        LocalizedProperty _itemPath = null!;
        LocalizedProperty _parameterName = null!;

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
                LEditorGUILayout.Prop(_itemPath);
                LEditorGUILayout.Prop(_actionIndex);

                using (new BoxLayoutScope())
                {
                    LGUILayout.Heading(Loc("CustomActionWizard::Advanced Settings"));
                    LEditorGUILayout.Prop(_parameterName);
                    LEditorGUILayout.Prop(_hasExpressionItemSource);
                    LEditorGUILayout.PropAsEnumPopup<EmoteSequenceFactoryKind>(_emoteSequenceFactoryKind);
                }

                serializedObject.ApplyModifiedProperties();

                using (new EditorGUI.DisabledScope(checkInvalid.IsInvalid))
                {
                    if (EmoteWizardGUILayout.Undoable(Loc("CustomActionWizard::Add"), "Add from Custom Action Wizard", out var undoable))
                    {
                        soleTarget.Explode(CachedEnv(), undoable, true);
                    }
                }
            }
        }
    }
}