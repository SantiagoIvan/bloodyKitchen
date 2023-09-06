using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static Scene targetScene;

    /* Al hacer enum . toString(), lo transforma en estas etiquetas asi que la idea es que estos nombres estén tal cual
     * en el UnityUI
     */
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    // Si hago que primero cargue el Load screen y dsp la otra escena, el load ni se va a ver y va a cargar la segunda. 
    // Para forzar a que se vea el load screen, se crea un objeto en LoadScene que al primer update, llame a ese Loader callback y asi carga la escena final
    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
