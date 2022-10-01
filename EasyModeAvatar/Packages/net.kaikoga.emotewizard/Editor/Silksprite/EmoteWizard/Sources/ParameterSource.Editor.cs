using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ParameterSource))]
    public class ParameterSourceEditor : Editor
    {
        ParameterSource _parameterSource;
        ExpandableReorderableList<ParameterItem> _parameterItemsList;

        void OnEnable()
        {
            _parameterSource = (ParameterSource) target;
            
            _parameterItemsList = new ExpandableReorderableList<ParameterItem>(new ParameterItemListHeaderDrawer(), new ParameterItemDrawer(), "Parameter Items", ref _parameterSource.parameterItems);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _parameterSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_parameterSource))
            {
                using (new ParameterItemDrawerContext(emoteWizardRoot, true).StartContext())
                {
                    _parameterItemsList.DrawAsProperty(_parameterSource.parameterItems, emoteWizardRoot.listDisplayMode);
                }
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial => 
            string.Join("\n",
                "外部アセットが利用するExpression Parameterを追加したい場合、ここから登録してください。",
                "（Emote Wizardで利用しているパラメータは自動で追加されるため、ここで登録する必要はありません。）");
    }
}