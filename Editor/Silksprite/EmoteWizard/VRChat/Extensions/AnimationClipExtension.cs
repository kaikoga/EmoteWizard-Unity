using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
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

        public static GenericEmoteSequence ToGenericEmoteSequence(this AnimationClip clip, GameObject animationRoot)
        {
            var animatedEnable = new List<GenericEmoteSequence.AnimatedEnable>();
            var animatedBlendShapes = new List<GenericEmoteSequence.AnimatedBlendShape>();

            var curveBindings = AnimationUtility.GetCurveBindings(clip);
            var objectReferenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach (var curveBinding in curveBindings)
            {
                if (curveBinding.type == typeof(GameObject))
                {
                    switch (curveBinding.propertyName)
                    {
                        case "m_IsActive":
                            animatedEnable.Add(new GenericEmoteSequence.AnimatedEnable
                            {
                                target = animationRoot.transform.Find(curveBinding.path),
                                isEnable = AnimationUtility.GetEditorCurve(clip, curveBinding).Evaluate(0f) != 0f 
                            });
                            break;
                    }
                } else if (curveBinding.type == typeof(SkinnedMeshRenderer))
                {
                    if (curveBinding.propertyName.StartsWith("blendShape."))
                    {
                        animatedBlendShapes.Add(new GenericEmoteSequence.AnimatedBlendShape
                        {
                            target = animationRoot.transform.Find(curveBinding.path).GetComponent<SkinnedMeshRenderer>(),
                            blendShapeName = curveBinding.propertyName.Substring("blendShape.".Length),
                            value = AnimationUtility.GetEditorCurve(clip, curveBinding).Evaluate(0f)
                        });
                    }
                }
            }

            foreach (var objectReferenceCurveBinding in objectReferenceCurveBindings)
            {
                
            }
            
            return new GenericEmoteSequence
            {
                animatedEnable = animatedEnable.ToArray(),
                animatedBlendShapes = animatedBlendShapes.ToArray()
            };
        }
    }
}