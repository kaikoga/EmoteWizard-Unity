using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Builders;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteTrigger
    {
        [SerializeField] public string name;
        [SerializeField] public int priority;
        [SerializeField] public List<EmoteCondition> conditions = new List<EmoteCondition>();

        public bool LooksLikeMirrorItem
        {
            get
            {
                foreach (var condition in conditions)
                {
                    if (EmoteWizardConstants.Params.IsMirrorParameter(condition.parameter)) return true;
                }

                return false;
            }
        }

        public static EmoteTriggerBuilder Builder(string name)
        {
            return new EmoteTriggerBuilder(new EmoteTrigger
            {
                name = name,
            });
        }

        public EmoteTriggerInstance ToInstance(EmoteWizardEnvironment environment) =>
            new EmoteTriggerInstance(name, priority, conditions.Select(condition => condition.ToInstance(environment)));
    }
}