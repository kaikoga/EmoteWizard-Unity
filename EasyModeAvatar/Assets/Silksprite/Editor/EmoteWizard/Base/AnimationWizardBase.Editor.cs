using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimationWizardBaseEditor : Editor
    {
        AnimationWizardBase AnimationWizardBase => target as AnimationWizardBase;

        protected static string Tutorial =>
            string.Join("\n",
                "Write Defaultsはオフになります。",
                "EmotesとParameter Emotesで使われているシェイプキーなどをリセットするアニメーションがResetレイヤーに自動的に設定されます。",
                "",
                "BaseMixins: 常時再生したいBlendTreeなどの設定",
                "Emotes: ハンドサインに基づくアニメーションの設定",
                "Parameter Emotes: パラメーターに基づくアニメーションの設定",
                "Mixins: 上書きして再生したいBlendTreeなどの設定");

        protected AnimationClip BuildResetClip(AnimationClip clip)
        {
            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(AnimationWizardBase.emotes.SelectMany(e => e.AllClips()))
                .Concat(AnimationWizardBase.parameterEmotes.SelectMany(p => p.AllClips()));
            var allParameters = allClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Select(curve => (curve.path, curve.propertyName, curve.type) ) 
                .Distinct().OrderBy(x => x);
            
            clip.ClearCurves();
            clip.frameRate = 60f;
            foreach (var (path, propertyName, type) in allParameters)
            {
                clip.SetCurve(path, type, propertyName, AnimationCurve.Constant(0f, 1 / 60f, 0f));
            }

            return clip;
        }
    }
}