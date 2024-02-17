using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Builders;
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
    }
}