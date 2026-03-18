using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Extensions
{
    public static class ParametersSnapshotExtension
    {
        public static VRCExpressionParameters.Parameter[] ToParameters(this ParametersSnapshot snapshot)
        {
            return snapshot.ParameterItems
                .Select(parameter => parameter.ToParameter())
                .ToArray();
        }
    }
}