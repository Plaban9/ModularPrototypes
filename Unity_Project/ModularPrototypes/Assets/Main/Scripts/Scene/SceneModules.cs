namespace ModularPrototypes.Scene
{
    public class SceneModules
    {
        public const string Main = "TopLevel_UI";

        public const string Voronoi_SplitScreen_Prototype = "Voronoi_SplitScreen_Prototype";
        public const string Voronoi_Region_Generator_Prototype = "Voronoi_Region_Generator_Prototype";
        public const string PlatformWithProjectiles_Prototype = "PlatformWithProjectiles_Prototype";
        public const string Platformer_Prototype = "Platform_Prototype";
        public const string BulletHell_Prototype = "BulletHell_Prototype";
    }

    public struct SceneModuleInfo
    {
        private readonly string Name;
        private readonly string DisplayName;
        private readonly string Description;

        public SceneModuleInfo(string name, string displayName, string description)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
        }

        public string GetName() => Name;
        public string GetDisplayName() => DisplayName;
        public string GetDescription() => Description;
    }
}
