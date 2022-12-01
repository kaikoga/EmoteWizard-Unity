using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources.Legacy.Base
{
    [CustomEditor(typeof(MultiAnimationMixinSourceBase), true)]
    public class MultiAnimationMixinSourceBaseEditor : Editor
    {
        MultiAnimationMixinSourceBase _multiAnimationMixinSource;

        void OnEnable()
        {
            _multiAnimationMixinSource = (MultiAnimationMixinSourceBase)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiAnimationMixinSource.mixins.Count + _multiAnimationMixinSource.baseMixins.Count} Items");
            
            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}