using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ClipBuilders
{
    public struct CurveBindings
    {
        public readonly List<EditorCurveBinding> Floats;
        public readonly List<EditorCurveBinding> Objects;

        CurveBindings(IEnumerable<EditorCurveBinding> bindings, IEnumerable<EditorCurveBinding> objectBindings)
        {
            Floats = bindings.ToList();
            Objects = objectBindings.ToList();
        }

        public static CurveBindings Collect(IEnumerable<AnimationClip> clips) => Collect(clips.ToArray());

        static CurveBindings Collect(params AnimationClip[] clips)
        {
            var curveBindings = clips
                .SelectMany(AnimationUtility.GetCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            var objectReferenceCurveBindings = clips
                .SelectMany(AnimationUtility.GetObjectReferenceCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            return new CurveBindings(curveBindings, objectReferenceCurveBindings);
        }
    }
}