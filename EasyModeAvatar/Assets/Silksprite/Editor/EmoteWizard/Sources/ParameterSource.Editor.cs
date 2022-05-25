using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
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
        }
    }
}