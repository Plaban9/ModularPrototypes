namespace ModularPrototypes.Scene
{
    public class SceneModules
    {
        #region Empty Scene Module Info
        public static SceneModuleInfo EMPTY = new SceneModuleInfo("EMPTY", "", "", "");
        #endregion

        #region Main Top Level Scene
        public static SceneModuleInfo MAIN = new SceneModuleInfo("TopLevel_UI", "TopLevel_UI", "Main Scene", "The main scene that contains the UI and other top-level elements.");
        #endregion

        #region Prototypes
        public static SceneModuleInfo VORONOI_SPLITSCREEN_PROTOTYPE = new SceneModuleInfo("Voronoi_SplitScreen_Prototype", "Voronoi_SplitScreen_Prototype", "Voronoi Split Screen", "Dynamic Split Screen System based on Voronoi Regions.");
        public static SceneModuleInfo VORONOI_REGION_GENERATOR_PROTOTYPE = new SceneModuleInfo("Voronoi_Region_Generator_Prototype", "Voronoi_Region_Generator_Prototype", "Voronoi Region Generator", "Region Generation system based on Voronoi.");
        public static SceneModuleInfo PLATFORM_WITH_PROJECTILES_PROTOTYPE = new SceneModuleInfo("PlatformWithProjectiles_Prototype", "PlatformWithProjectiles_Prototype", "Combined: Platformer and Bullet Hell", "Combination of Platformer and Bullet Hell to form unique patterns and interactions.");
        public static SceneModuleInfo PLATFORMER_PROTOTYPE = new SceneModuleInfo("Platform_Prototype", "Platform_Prototype", "Platformer", "Platform Locomotion and Transformation system based on Mathematical Functions.");
        public static SceneModuleInfo BULLETHELL_PROTOTYPE = new SceneModuleInfo("BulletHell_Prototype", "BulletHell_Prototype", "Bullet Hell Pattern", "Projetile spawn and pattern system based on Mathematical Functions.");
        #endregion
    }

    public readonly struct SceneModuleInfo
    {
        private readonly string Name;
        private readonly string SceneName;
        private readonly string DisplayName;
        private readonly string Description;

        public SceneModuleInfo(string name, string sceneName, string displayName, string description)
        {
            Name = name;
            SceneName = sceneName;
            DisplayName = displayName;
            Description = description;
        }

        public string GetName() => Name;
        public string GetSceneName() => SceneName;
        public string GetDisplayName() => DisplayName;
        public string GetDescription() => Description;
    }
}
