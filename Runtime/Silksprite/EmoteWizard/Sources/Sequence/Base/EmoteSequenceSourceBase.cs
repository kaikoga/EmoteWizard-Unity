using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence.Base
{
    [DisallowMultipleComponent]
    public abstract class EmoteSequenceSourceBase : EmoteWizardDataSourceBase
    {
        public abstract IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate();

        public void ExplodeEmoteSequencesImmediate(IUndoable undoable, IClipBuilder clipBuilder)
        {
            var environment = CreateEnv();
            
            ExplodeEmoteSequences(undoable, environment, this, this, clipBuilder);

            undoable.DestroyObject(this);
        }

        static void ExplodeEmoteSequences(IUndoable undoable, EmoteWizardEnvironment environment, EmoteSequenceSourceBase source, Component destination, IClipBuilder clipBuilder)
        {
            var sequence = source.ToEmoteFactoryTemplate().Build(environment, clipBuilder);
            var gameObject = destination.gameObject;
            undoable.DestroyObject(source);
            
            var child = undoable.AddComponent<EmoteSequenceSource>(gameObject);
            child.sequence = sequence;
        }
    }
}