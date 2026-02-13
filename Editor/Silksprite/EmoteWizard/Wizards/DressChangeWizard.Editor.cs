using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.UI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

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

                LEditorGUILayout.Prop(_expressionKind);
                if ((DressChangeWizard.ExpressionKind)_expressionKind.Property.enumValueIndex != DressChangeWizard.ExpressionKind.SimpleToggle)
                {
                    LEditorGUILayout.Prop(_itemCount);
                }
                LEditorGUILayout.Prop(_emoteSequenceFactoryKind);

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