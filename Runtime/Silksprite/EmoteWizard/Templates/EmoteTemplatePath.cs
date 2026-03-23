using System.IO;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Utils;

namespace Silksprite.EmoteWizard.Templates
{
    public readonly struct EmoteTemplatePath
    {
        public readonly string PathFromAvatarRoot;

        public string FileName => Path.GetFileName(PathFromAvatarRoot);

        EmoteTemplatePath(string pathFromAvatarRoot)
        {
            PathFromAvatarRoot = pathFromAvatarRoot;
        }

        public static EmoteTemplatePath SelfPath(EmoteWizardEnvironment environment, EmoteWizardBehaviour context)
        {
            var pathFromAvatarRoot = RuntimeUtil.RelativePath(environment.AvatarRoot, context.transform)!;
            return new EmoteTemplatePath(pathFromAvatarRoot);
        }

        public static EmoteTemplatePath Context(EmoteWizardEnvironment environment, EmoteWizardBehaviour context)
        {
            var pathFromAvatarRoot = Path.GetDirectoryName(RuntimeUtil.RelativePath(environment.AvatarRoot, context.transform))!;
            return new EmoteTemplatePath(pathFromAvatarRoot);
        }

        public EmoteTemplatePath Join(string relativePath) => new EmoteTemplatePath(Path.Join(PathFromAvatarRoot, relativePath));
    }
}