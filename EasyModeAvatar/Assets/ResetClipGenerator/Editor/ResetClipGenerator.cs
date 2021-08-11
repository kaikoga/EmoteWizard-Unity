// MIT License
//
// Copyright (c) 2021 kaikoga
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.ResetClipGenerator
{
    public class ResetClipGeneratorWindow : EditorWindow
    {
        [SerializeField] VRCAvatarDescriptor avatarDescriptor;
        [SerializeField] List<AnimationClip> resetClips;

        public void OnEnable()
        {
            titleContent = new GUIContent("Reset Clip Generator");
        }

        public void OnGUI()
        {
            void HelpLabel(string message)
            {
                GUILayout.Label(message.Replace(" ", " "), new GUIStyle{wordWrap = true});
            }

            var serializedObject = new SerializedObject(this);
            GUILayout.Label("リセットアニメーション自動生成くん", new GUIStyle{fontStyle = FontStyle.Bold});
            GUILayout.Space(4f);
            HelpLabel("1. あらかじめリセットアニメーション用の空のAnimation ClipをAnimator Controllerの一番上のレイヤーに組み込んでおく");
            HelpLabel("2. シーン上のAvatarDescriptorを↓にセットする");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("avatarDescriptor"));
            HelpLabel("3. Find Reset Clipsボタンを押す");
            if (GUILayout.Button("Find Reset Clips"))
            {
                FindResetClips();
            }
            HelpLabel("4. ↓のリストにリセットアニメーションの候補が入るが、余計なものが入ってるのでリセットアニメーションではないものを人の手で取り除く");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("resetClips"));
            HelpLabel("5. Generate Reset Clipsボタンを押す");
            using (new EditorGUI.DisabledScope(avatarDescriptor == null || resetClips.Count == 0))
            {
                if (GUILayout.Button("Generate Reset Clips"))
                {
                    GenerateResetClips();
                }
            }
            HelpLabel("6. リセットアニメーションが含まれるAnimator Controllerが動かす全てのプロパティがシーン上の現在のアバターの状態にリセットされるリセットアニメーションが自動生成される！");
            serializedObject.ApplyModifiedProperties();
        }

        void FindResetClips()
        {
            resetClips = CollectAnimatorControllers().SelectMany(animatorController => CollectClips(animatorController).Take(1)).ToList();
        }

        void GenerateResetClips()
        {
            var clipCaches = CollectAnimatorControllers().ToDictionary(animatorController => animatorController, CollectClips);
            
            foreach (var resetClip in resetClips)
            {
                if (resetClip == null) continue;
                var animatorController = clipCaches.FirstOrDefault(clipCache => clipCache.Value.Contains(resetClip)).Key;
                if (animatorController == null) continue;

                var clips = CollectClips(animatorController).Where(clip => clip != resetClip).ToArray();
                GenerateResetClip(clips, resetClip);
            }
        }

        IEnumerable<AnimatorController> CollectAnimatorControllers()
        {
            var layers = Enumerable.Empty<VRCAvatarDescriptor.CustomAnimLayer>()
                .Concat(avatarDescriptor.baseAnimationLayers)
                .Concat(avatarDescriptor.specialAnimationLayers);
            return layers.Select(layer => layer.animatorController).OfType<AnimatorController>();
        }

        static IEnumerable<AnimationClip> CollectClips(AnimatorController animatorController)
        {
            IEnumerable<AnimatorStateMachine> VisitStateMachines(AnimatorStateMachine stateMachine)
            {
                yield return stateMachine;
                foreach (var child in stateMachine.stateMachines)
                {
                    foreach (var sm in VisitStateMachines(child.stateMachine)) yield return sm;
                }
            }
            var stateMachines = animatorController.layers.SelectMany(layer => VisitStateMachines(layer.stateMachine)).Distinct();
            var states = stateMachines.SelectMany(stateMachine => stateMachine.states.Select(childState => childState.state)).Distinct();
            var motions = states.Select(state => state.motion).Distinct();
            IEnumerable<AnimationClip> VisitClips(Motion motion)
            {
                switch (motion)
                {
                    case AnimationClip clip:
                        yield return clip;
                        break;
                    case BlendTree blendTree:
                        foreach (var child in blendTree.children)
                        {
                            foreach (var childClip in VisitClips(child.motion)) yield return childClip;
                        }
                        break;
                }
            }
            return motions.SelectMany(VisitClips).Distinct();
        }

        void GenerateResetClip(AnimationClip[] sourceClips, AnimationClip targetClip)
        {
            var curveBindings = sourceClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            var objectReferenceCurveBindings = sourceClips.SelectMany(AnimationUtility.GetObjectReferenceCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            
            var avatar = avatarDescriptor != null ? avatarDescriptor.gameObject : null;

            targetClip.ClearCurves();
            targetClip.frameRate = 60f;
            foreach (var curveBinding in curveBindings)
            {
                var value = 0f;
                if (avatar)
                {
                    AnimationUtility.GetFloatValue(avatar, curveBinding, out value);
                }
                targetClip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, AnimationCurve.Constant(0f, 1 / 60f, value));
            }
            foreach (var curveBinding in objectReferenceCurveBindings)
            {
                Object value = null;
                if (avatar)
                {
                    AnimationUtility.GetObjectReferenceValue(avatar, curveBinding, out value);
                }
                AnimationUtility.SetObjectReferenceCurve(targetClip, curveBinding, new []
                {
                    new ObjectReferenceKeyframe { time = 0, value = value },
                    new ObjectReferenceKeyframe { time = 1 / 60f, value = value }
                });
            }
        }

        [MenuItem("Window/Silksprite/Reset Clip Generator", false, 60000)]
        public static void CreateWindow()
        {
            CreateInstance<ResetClipGeneratorWindow>().Show();
        }
    }
}