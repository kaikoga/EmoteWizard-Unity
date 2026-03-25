using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : EmoteWizardDataSourceBase
    {
        protected abstract IEnumerable<IEmoteTemplate> SourceTemplates(EmoteWizardEnvironment environment);

        public void Explode(EmoteWizardEnvironment environment, IUndoable undoable, bool andSelect)
        {
            var sourceTemplates = SourceTemplates(environment).ToArray();

            var root = environment.AvatarRoot.transform;

            var children = sourceTemplates
                .Select(template => template.Path)
                .OrderBy(path => path.PathFromAvatarRoot.Count(c => c == '/'))
                .DistinctBy(path => path.PathFromAvatarRoot)
                .ToDictionary(path => path.PathFromAvatarRoot,
                    path => undoable.AddChildGameObject(root, path.PathFromAvatarRoot, path.FileName != name));

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