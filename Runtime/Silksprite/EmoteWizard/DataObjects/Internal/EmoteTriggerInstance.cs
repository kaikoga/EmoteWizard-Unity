using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteTriggerInstance
    {
        public string Name;
        public int Priority;
        public readonly List<EmoteConditionInstance> Conditions = new List<EmoteConditionInstance>();

        public EmoteTriggerInstance(string name, int priority, IEnumerable<EmoteConditionInstance> conditions)
        {
            Name = name;
            Priority = priority;
            Conditions.AddRange(conditions);
        }

        public EmoteTriggerInstance(EmoteTriggerInstance other) : this(other.Name, other.Priority, other.Conditions.Select(condition => new EmoteConditionInstance(condition)))
        {
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
