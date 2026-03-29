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
    [CustomEditor(typeof(EmoteItemWizard))]
    public class EmoteItemWizardEditor : EmoteWizardEditorBase<EmoteItemWizard>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRCPlatforms;

        LocalizedProperty _hasExpressionItemSource = null!;
        LocalizedProperty _emoteSequenceFactoryKind = null!;
        LocalizedProperty _itemPath = null!;
        LocalizedProperty _hasGroupName = null!;
        LocalizedProperty _groupName = null!;
        LocalizedProperty _hasParameterName = null!;
        LocalizedProperty _parameterName = null!;

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
                LEditorGUILayout.Prop(_itemPath);

                LEditorGUILayout.PropAsFoldout(_hasGroupName, () =>
                {
                    LEditorGUILayout.Prop(_groupName);
                });

                LEditorGUILayout.PropAsFoldout(_hasParameterName, () =>
                {
                    LEditorGUILayout.Prop(_parameterName);
                });

                LEditorGUILayout.Prop(_hasExpressionItemSource);
                LEditorGUILayout.PropAsEnumPopup<EmoteSequenceFactoryKind>(_emoteSequenceFactoryKind);

                serializedObject.ApplyModifiedProperties();

                using (new EditorGUI.DisabledScope(checkInvalid.IsInvalid))
                {
                    if (EmoteWizardSupportGUILayout.Undoable(Loc("EmoteItemWizard::Add"), "Add From Emote Item Wizard", out var undoable))
                    {
                        soleTarget.Explode(CachedEnv(), undoable, true);
                    }
                }
            }
        }
    }
}