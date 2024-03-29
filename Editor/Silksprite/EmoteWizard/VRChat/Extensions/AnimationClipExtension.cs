using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimationClipExtension
    {
        public static void SetLoopTime(this AnimationClip clip, bool value)
        {
            var path = AssetDatabase.GetAssetPath(clip);
            if (path.StartsWith("Packages/com.vrchat")) return;
            var serializedObject = new SerializedObject(clip);
            var property = serializedObject.FindProperty("m_AnimationClipSettings.m_LoopTime");
            if (property.boolValue == value) return;
            property.boolValue = value;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(clip);
        }
    }
}