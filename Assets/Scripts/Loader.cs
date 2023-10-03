using UnityEngine.SceneManagement;

public static class Loader
{
    public static Scene targetScene;
    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString()); 
    }
}

public enum Scene
{
    Home,
    GameScene,
    LoadingScene
}
