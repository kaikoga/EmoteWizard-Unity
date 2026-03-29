using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteTriggerInstance
    {
        public readonly string Name;
        public readonly int Priority;
        public readonly List<EmoteConditionInstance> Conditions = new List<EmoteConditionInstance>();

        public EmoteTriggerInstance(string name, int priority, IEnumerable<EmoteConditionInstance> conditions)
        {
            Name = name;
            Priority = priority;
            Conditions.AddRange(conditions);
        }

        public EmoteTriggerInstance ResolveMirror(IPlatformFeatures platformFeatures, EmoteHand hand)
        {
            return new EmoteTriggerInstance(Name, Priority,
                Conditions.Select(condition => condition.ResolveMirror(platformFeatures, hand)));
        }

        public bool LooksLikeMirrorItem
        {
            get
            {
                foreach (var condition in Conditions)
                {
                    if (EmoteWizardConstants.Params.IsMirrorParameter(condition.Parameter)) return true;
                }

                return false;
            }
        }

    }
}
