using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(AnimationMixinSourceBase), true)]
    public class AnimationMixinSourceBaseEditor : Editor
    {
        AnimationMixinSourceBase _animationMixinSource;

        AnimationMixinDrawerState _baseMixinsState;
        AnimationMixinDrawerState _mixinsState;

        ExpandableReorderableList<AnimationMixin> _baseMixinsList;
        ExpandableReorderableList<AnimationMixin> _mixinsList;

        void OnEnable()
        {
            _animationMixinSource = (AnimationMixinSourceBase)target;

            _baseMixinsState = new AnimationMixinDrawerState();
            _mixinsState = new AnimationMixinDrawerState();

            _baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref _animationMixinSource.baseMixins);
            _mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Mixins", ref _animationMixinSource.mixins);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _animationMixinSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_animationMixinSource))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                var relativePath = GeneratedAssetLocator.MixinDirectoryPath(_animationMixinSource.LayerName);
                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, _baseMixinsState).StartContext())
                {
                    _baseMixinsList.DrawAsProperty(_animationMixinSource.baseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, _mixinsState).StartContext())
                {
                    _mixinsList.DrawAsProperty(_animationMixinSource.mixins, emoteWizardRoot.listDisplayMode);
                }
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