using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class WizardExploder
    {
        public static void ExplodeImmediate(IUndoable undoable, EmoteWizardBase wizard, bool andSelect = true)
        {
            var sourceTemplates = wizard.SourceTemplates().ToArray();

            var parent = wizard.transform.parent;

            var children = sourceTemplates
                .Select(template => template.Path)
                .OrderBy(path => path.Length)
                .Distinct()
                .ToDictionary(path => path, path => undoable.AddChildGameObject(parent, path));

            foreach (var template in sourceTemplates)
            {
                var child = children[template.Path];
                template.PopulateSources(undoable, child.transform);
            }

            if (andSelect)
            {
                var firstChild = children.Values.FirstOrDefault();
                if (firstChild) undoable.SetActiveObjectWithContext(firstChild, firstChild);
            }

            var gameObject = wizard.gameObject;
            if (gameObject.GetComponents<Component>().Length == 2)
            {
                undoable.DestroyObject(gameObject);
            }
            else
            {
                undoable.DestroyObject(wizard);
            }
        }
    }
}