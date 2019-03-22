using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticInformation
{
    private static int levelSize = 3;

    public static Dictionary<int,string> GetScenes()
    {
        Dictionary<int, string> scenes = new Dictionary<int, string>();
        
        scenes[-2] = "menu_splashscreen";
        scenes[-1] = "menu_loadingscreen";
        scenes[0] = "map_01";
        scenes[1] = "map_02";
        scenes[2] = "map_03";

        return scenes;
    }

    public static string GetLevel(int index)
    {
        if (index < levelSize)
        {
            Dictionary<int, string> myDictionary = GetScenes();
            return myDictionary[index];
        }
        else
        {
            Dictionary<int, string> myDictionary = GetScenes();
            return myDictionary[-1];
        }
    }

    public static void LoadLevel(int index)
    {
        PlayerPrefs.SetInt("currentLevel", index);
        SceneManager.LoadScene(GetLevel(index));
    }

    public static void LoadNextLevel()
    {
        int loadIndex = PlayerPrefs.GetInt("currentLevel", -1);
        loadIndex++;
        string nextLevel = GetLevel(loadIndex);

        //write the correct index in playerprefs
        if (loadIndex >= levelSize)
            loadIndex = -1;
        PlayerPrefs.SetInt("currentLevel", loadIndex);

        SceneManager.LoadScene(nextLevel);
    }

    public static void LoadCurrentProgress()
    {
        int loadIndex = PlayerPrefs.GetInt("currentLevel", 0);

        if (loadIndex < 0) {
            loadIndex = 0;
            PlayerPrefs.SetInt("currentLevel", loadIndex);
        }

        SceneManager.LoadScene(GetLevel(loadIndex));
    }
}
