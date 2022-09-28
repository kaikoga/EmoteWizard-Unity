using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimationClipExtension
    {
        public static void SetLoopTime(this AnimationClip clip, bool value)
        {
            var serializedObject = new SerializedObject(clip);
            serializedObject.FindProperty ("m_AnimationClipSettings.m_LoopTime").boolValue = value;
            serializedObject.ApplyModifiedProperties();
        }
    }
}