using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi.Base
{
    [CustomEditor(typeof(MultiAnimationMixinSourceBase), true)]
    public class MultiAnimationMixinSourceBaseEditor : Editor
    {
        MultiAnimationMixinSourceBase _multiAnimationMixinSource;

        AnimationMixinDrawerState _baseMixinsState;
        AnimationMixinDrawerState _mixinsState;

        ExpandableReorderableList<AnimationMixin> _baseMixinsList;
        ExpandableReorderableList<AnimationMixin> _mixinsList;

        void OnEnable()
        {
            _multiAnimationMixinSource = (MultiAnimationMixinSourceBase)target;

            _baseMixinsState = new AnimationMixinDrawerState();
            _mixinsState = new AnimationMixinDrawerState();

            _baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref _multiAnimationMixinSource.baseMixins);
            _mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Mixins", ref _multiAnimationMixinSource.mixins);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiAnimationMixinSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiAnimationMixinSource))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                var relativePath = GeneratedAssetLocator.MixinDirectoryPath(_multiAnimationMixinSource.LayerName);
                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, _baseMixinsState).StartContext())
                {
                    _baseMixinsList.DrawAsProperty(_multiAnimationMixinSource.baseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, _mixinsState).StartContext())
                {
                    _mixinsList.DrawAsProperty(_multiAnimationMixinSource.mixins, emoteWizardRoot.listDisplayMode);
                }
            }
            
            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiAnimationMixinSource);
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial => 
            string.Join("\n",
                "常時再生したいBlendTreeを登録します。",
                "",
                "Base Mixins: 他のレイヤーの上に追加されます。",
                "Mixins: 他のレイヤーの下に追加されます。");

    }
}