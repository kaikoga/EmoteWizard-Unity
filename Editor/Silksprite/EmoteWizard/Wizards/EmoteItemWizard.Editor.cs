using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(EmoteItemWizard))]
    public class EmoteItemWizardEditor : EmoteWizardEditorBase<EmoteItemWizard>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.UnityPlatforms;

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
                LEditorGUILayout.Prop(_itemPath);

                Action content = () =>
                {
                    LEditorGUILayout.Prop(_groupName);
                };
                LEditorGUILayout.PropAsFoldout(_hasGroupName, content);

                Action content1 = () =>
                {
                    LEditorGUILayout.Prop(_parameterName);
                };
                LEditorGUILayout.PropAsFoldout(_hasParameterName, content1);

                LEditorGUILayout.Prop(_hasExpressionItemSource);
                LEditorGUILayout.Prop(_emoteSequenceFactoryKind);

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