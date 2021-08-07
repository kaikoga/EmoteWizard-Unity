using UnityEditor;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimationWizardBaseEditor : Editor
    {
        protected static string Tutorial =>
            string.Join("\n",
                "Write Defaultsはオフになります。",
                "各アニメーションで使われているシェイプキーなどをリセットするアニメーションがResetレイヤーに自動的に設定されます。",
                "",
                "Base Mixins: 常時再生したいBlendTreeなどの設定",
                "Emotes: ハンドサインに基づくアニメーションの設定",
                "Parameter Emotes: パラメーターに基づくアニメーションの設定",
                "Mixins: 上書きして再生したいBlendTreeなどの設定");
    }
}