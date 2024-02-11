namespace Silksprite.EmoteWizard.DataObjects.Builders
{
    public class EmoteTriggerBuilder
    {
        readonly EmoteTrigger _trigger;

        public EmoteTriggerBuilder(EmoteTrigger emoteTrigger)
        {
            _trigger = emoteTrigger;
        }

        public void AddPriority(int priority)
        {
            _trigger.priority = priority;
        }

        public void AddCondition(EmoteCondition condition)
        {
            _trigger.conditions.Add(condition);
        }

        public EmoteTrigger ToEmoteTrigger() => _trigger;
    }
}