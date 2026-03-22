using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : EmoteWizardBehaviour
    {
        protected abstract IEnumerable<IEmoteTemplate> SourceTemplates();

        public void Explode(IUndoable undoable, bool andSelect)
        {
            var environment = CreateEnv();
            var sourceTemplates = SourceTemplates().ToArray();

            var root = environment.AvatarRoot.transform;

            var children = sourceTemplates
                .Select(template => template.Path.PathFromAvatarRoot)
                .OrderBy(path => path.Count(c => c == '/'))
                .Distinct()
                .ToDictionary(path => path,
                    path => undoable.AddChildGameObject(root, path, path != name));

            foreach (var template in sourceTemplates)
            {
                var child = children[template.Path.PathFromAvatarRoot];
                template.PopulateSources(undoable, child.transform);
            }

            if (andSelect)
            {
                var firstChild = children.Values.FirstOrDefault();
                if (firstChild != null)
                {
                    undoable.SetActiveObjectWithContext(firstChild, firstChild);
                }
            }

            if (gameObject.GetComponents<Component>().Length == 2)
            {
                undoable.DestroyObject(gameObject);
            }
            else
            {
                undoable.DestroyObject(this);
            }
        }
    }
}