using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

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
            
            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiAnimationMixinSource);
            }

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}