using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Tools;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(MirroredMotion))]
    public class MirroredMotionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var useMirroredSettings = serializedProperty.Lop(nameof(MirroredMotion.useMirroredSettings), Loc("MirroredMotion::useMirroredSettings"));
            var clipLeft = serializedProperty.Lop(nameof(MirroredMotion.clipLeft), Loc("MirroredMotion::clipLeft"));
            var clipRight = serializedProperty.Lop(nameof(MirroredMotion.clipRight), Loc("MirroredMotion::clipRight"));

            LEditorGUI.Prop(position.UISliceV(0), useMirroredSettings);
            if (useMirroredSettings.Property.boolValue)
            {
                LEditorGUI.Prop(position.UISliceV(1), clipLeft);
                LEditorGUI.Prop(position.UISliceV(2), clipRight);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            var useMirroredSettings = serializedProperty.Lop(nameof(MirroredMotion.useMirroredSettings), Loc("MirroredMotion::useMirroredSettings"));
            return PropertyDrawerUITools.LineHeight(useMirroredSettings.Property.boolValue ? 3 : 1);
        }
    }
}