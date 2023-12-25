using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Templates;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Impl
{
    public class SimpleEmoteFactory : IEmoteFactory
    {
        readonly SimpleEmote _simpleEmote;
        readonly string _clipName;

        public SimpleEmoteFactory(SimpleEmote simpleEmote, string clipName)
        {
            _simpleEmote = simpleEmote;
            _clipName = clipName;
        }

        LayerKind IEmoteFactory.LayerKind => _simpleEmote.layerKind;
        string IEmoteFactory.GroupName => _simpleEmote.groupName;
        bool IEmoteFactory.LooksLikeMirrorItem => false;
        bool IEmoteFactory.LooksLikeToggle => true;

        IEnumerable<Motion> IEmoteFactory.AllClipRefs()
        {
            return Enumerable.Empty<Motion>();
        }

        IEnumerable<TrackingOverride> IEmoteFactory.TrackingOverrides()
        {
            return _simpleEmote.hasTrackingOverrides ? _simpleEmote.trackingOverrides : Enumerable.Empty<TrackingOverride>();
        }

        IEmoteFactoryTemplate IEmoteFactory.ToTemplate()
        {
            return new SimpleEmoteFactoryTemplate(this);
        }

        EmoteSequence IEmoteFactory.Build(IEmoteFactory.IClipBuilder builder)
        {
            var avatarRootTransform = builder.Environment.AvatarDescriptor.transform;
            var clip = builder.Build(_clipName, _simpleEmote.animatedEnable.Select(ae => ae.ToAnimatedValue(avatarRootTransform)));

            return new EmoteSequence
            {
                layerKind = _simpleEmote.layerKind,
                groupName = _simpleEmote.groupName,

                isFixedDuration = _simpleEmote.isFixedDuration,
                clip = clip,
                entryTransitionDuration = _simpleEmote.entryTransitionDuration,
                exitTransitionDuration = _simpleEmote.exitTransitionDuration,

                hasLayerBlend = _simpleEmote.hasLayerBlend,
                blendIn = _simpleEmote.blendIn,
                blendOut = _simpleEmote.blendOut,
                    
                hasTrackingOverrides = _simpleEmote.hasTrackingOverrides,
                trackingOverrides = _simpleEmote.trackingOverrides.ToList()
            };
        }

        class SimpleEmoteFactoryTemplate : IEmoteFactoryTemplate
        {
            readonly SimpleEmoteFactory _factory;

            public SimpleEmoteFactoryTemplate(SimpleEmoteFactory factory) => _factory = factory;

            public IEmoteFactory ToEmoteFactory() => _factory;

            public EmoteSequenceSourceBase AddSequenceSource(Component target)
            {
                var source = target.gameObject.AddComponent<SimpleEmoteSource>();
                source.simpleEmote = _factory._simpleEmote;
                return source;
            }
        }

        public struct AnimatedValue<T>
        {
            public string Path;
            public string PropertyName;
            public Type Type;

            public T Value;
        }
    }
}