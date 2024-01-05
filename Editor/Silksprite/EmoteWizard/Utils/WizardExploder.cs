using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;

namespace Silksprite.EmoteWizard.Utils
{
    public static class WizardExploder
    {
        public static void ExplodeImmediate(IUndoable undoable, EmoteWizardBase wizard)
        {
            var sourceTemplates = wizard.SourceTemplates().ToArray();
            if (sourceTemplates.Length > 1)
            {
                // TODO: IEmoteTemplate should populate sources into children
                for (var i = 0; i < sourceTemplates.Length; i++)
                {
                    var template = sourceTemplates[i];
                    var child = undoable.AddChildGameObject(wizard, $"Item {i + 1}");
                    template.PopulateSources(undoable, child.transform);
                }

                var firstChild = wizard.transform.GetChild(0);
                Selection.SetActiveObjectWithContext(firstChild, firstChild);
            }
            else
            {
                foreach (var template in sourceTemplates)
                {
                    template.PopulateSources(undoable, wizard);
                }
            }
            
            undoable.DestroyObject(wizard);
        }
    }
}