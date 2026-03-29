using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.References;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        // mostly works like VRChat, but some features may take care of other platforms
        class MaybeVRChatFeatures : VRChatFeatures
        {
            public MaybeVRChatFeatures(IPlatformReferences platformReferences) : base(platformReferences) { }

            static readonly DefaultActionIndex[] Afk = { DefaultActionIndex.Afk };

            public override IEnumerable<DefaultActionIndex> DefaultActionIndexes() =>
                Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                    .Except(Afk)
                    .Concat(Afk);
        }
    }
}
