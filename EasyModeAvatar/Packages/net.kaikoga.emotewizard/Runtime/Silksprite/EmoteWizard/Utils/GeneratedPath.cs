using System.IO;
using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Utils
{
    public readonly struct GeneratedPath
    {
        const string GeneratedPrefix = "@@@Generated@@@";

        readonly string _relativePath;

        public GeneratedPath(string relativePath) => _relativePath = relativePath;

        public string Resolve(EmoteWizardEnvironment environment)
        {
            if (!environment.Root)
            {
                return Path.GetFileNameWithoutExtension(_relativePath.Replace(GeneratedPrefix, ""));
            }

            var relativePath = _relativePath.Replace(GeneratedPrefix, environment.Root.generatedAssetPrefix);
            return Path.Combine(environment.Root.generatedAssetRoot, relativePath);
        }
    }
}