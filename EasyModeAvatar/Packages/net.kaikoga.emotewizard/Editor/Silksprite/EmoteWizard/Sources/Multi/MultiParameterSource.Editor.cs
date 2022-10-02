using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi
{
    [CustomEditor(typeof(MultiParameterSource))]
    public class MultiParameterSourceEditor : Editor
    {
        MultiParameterSource _multiParameterSource;
        ExpandableReorderableList<ParameterItem> _parameterItemsList;

        void OnEnable()
        {
            _multiParameterSource = (MultiParameterSource) target;
            
            _parameterItemsList = new ExpandableReorderableList<ParameterItem>(new ParameterItemListHeaderDrawer(), new ParameterItemDrawer(), "Parameter Items", ref _multiParameterSource.parameterItems);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiParameterSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiParameterSource))
            {
                using (new ParameterItemDrawerContext(emoteWizardRoot, true).StartContext())
                {
                    _parameterItemsList.DrawAsProperty(_multiParameterSource.parameterItems, emoteWizardRoot.listDisplayMode);
                }
            }

            if (GUILayout.Button("Explode"))
            {
                Explode();
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        void Explode()
        {
            foreach (var parameterItem in _multiParameterSource.ParameterItems)
            {
                var child = _multiParameterSource.FindOrCreateChildComponent<ParameterSource>(parameterItem.name);
                child.parameterItem = SerializableUtils.Clone(parameterItem);
            }
        }

        static string Tutorial => 
            string.Join("\n",
                "外部アセットが利用するExpression Parameterを追加したい場合、ここから登録してください。",
                "（Emote Wizardで利用しているパラメータは自動で追加されるため、ここで登録する必要はありません。）");
    }
}