using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates;

namespace Silksprite.EmoteWizard.Contexts.Ephemeral
{
    [UsedImplicitly]
    public class MixinContext : ContextBase
    {
        List<MixinInstance>? _mixins;

        public MixinContext(EmoteWizardEnvironment env) : base(env) { }
        
        IEnumerable<MixinInstance> CollectAllMixins() =>
            Enumerable.Empty<MixinInstance>()
                .Concat(Environment.GetComponentsInChildren<IAnimatorControllerMixinTemplate>(true)
                    .SelectMany(source => source.ToAnimatorControllerMixins())
                    .Select(mixin => mixin.ToInstance()))
                .Concat(Environment.GetComponentsInChildren<IAnimationClipMixinTemplate>(true)
                    .SelectMany(source => source.ToAnimationClipMixins())
                    .Select(mixin => mixin.ToInstance()));

        IEnumerable<MixinInstance> AllMixins() => _mixins ??= CollectAllMixins().ToList();

        public IEnumerable<MixinInstance> Mixins(LayerKind layerKind)
            => AllMixins().Where(item => item.LayerKind == layerKind);
    }
}