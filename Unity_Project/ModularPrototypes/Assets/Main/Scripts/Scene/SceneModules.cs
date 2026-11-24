namespace ModularPrototypes.Scene
{
    public class SceneModules
    {
        #region Empty Scene Module Info
        // Placeholder for empty scene module info. Used as starting scene which pseudo unloads.
        public static SceneModuleInfo EMPTY = new SceneModuleInfo(name: "EMPTY",
                                                                  sceneName: "",
                                                                  "",
                                                                  ""); 
        #endregion

        #region Main Top Level Scene
        public static SceneModuleInfo MAIN = new SceneModuleInfo(name: "TopLevel_UI",
                                                                 sceneName: "TopLevel_UI",
                                                                 displayName: "Main Scene",
                                                                 description: "The main scene that contains the UI and other top-level elements.");
        #endregion

        #region Prototypes
        public static SceneModuleInfo VORONOI_SPLITSCREEN_PROTOTYPE = new SceneModuleInfo(name: "Voronoi_SplitScreen_Prototype",
                                                                                          sceneName: "Voronoi_SplitScreen_Prototype",
                                                                                          displayName: "Voronoi Dynamic Split Screen",
                                                                                          description: "Dynamic Split Screen System based on Voronoi Regions.");

        public static SceneModuleInfo VORONOI_REGION_GENERATOR_PROTOTYPE = new SceneModuleInfo(name: "Voronoi_Region_Generator_Prototype",
                                                                                               sceneName: "Voronoi_Region_Generator_Prototype",
                                                                                               displayName: "Voronoi Region Generator",
                                                                                               description: "Region Generation system based on Voronoi.");

        public static SceneModuleInfo PLATFORM_WITH_PROJECTILES_PROTOTYPE = new SceneModuleInfo(name: "PlatformWithProjectiles_Prototype",
                                                                                                sceneName: "PlatformWithProjectiles_Prototype",
                                                                                                displayName: "Combined: Platformer and Bullet Hell",
                                                                                                description: "Combination of Platformer and Bullet Hell to form unique patterns and interactions.");

        public static SceneModuleInfo PLATFORMER_PROTOTYPE = new SceneModuleInfo(name: "Platform_Prototype",
                                                                                 sceneName: "Platform_Prototype",
                                                                                 displayName: "Platformer",
                                                                                 description: "Platform Locomotion and Transformation system based on Mathematical Functions.");

        public static SceneModuleInfo BULLETHELL_PROTOTYPE = new SceneModuleInfo(name: "BulletHell_Prototype",
                                                                                 sceneName: "BulletHell_Prototype",
                                                                                 displayName: "Bullet Hell Pattern",
                                                                                 description: "Projetile spawn and pattern system based on Mathematical Functions.");
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
