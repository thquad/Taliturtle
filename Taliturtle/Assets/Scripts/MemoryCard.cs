﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MemoryCard
{
    public static int levelSize = 3; //manually change with level amount, maybe fix later
    public static readonly string SELECTED_LEVEL = "selectedLevel";
    public static readonly string UNLOCKED_LEVEL = "unlockedLevel";
    public static readonly string HIGHSCORE_FILE = "highscore";

    //-------------------------------------------------------------------------------- Scene Management

    public static Dictionary<int,string> GetScenes()
    {
        Dictionary<int, string> scenes = new Dictionary<int, string>();

        scenes[-3] = "menu_levelselectscreen";
        scenes[-2] = "menu_splashscreen";
        scenes[-1] = "menu_loadingscreen";
        scenes[0] = "map_01";
        scenes[1] = "map_02";
        scenes[2] = "map_03";

        return scenes;
    }

    public static string GetScene()
    {
        return GetScene(PlayerPrefs.GetInt(SELECTED_LEVEL, -1));
    }

    public static string GetScene(int index)
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

        PlayerPrefs.SetInt(SELECTED_LEVEL, index);

        if (index > PlayerPrefs.GetInt(UNLOCKED_LEVEL, -1))
            PlayerPrefs.SetInt(UNLOCKED_LEVEL, index);

        SceneManager.LoadScene(GetScene(index));
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(GetScene(-1));
    }

    public static void LoadSplash()
    {
        SceneManager.LoadScene(GetScene(-2));
    }

    public static void LoadLevelSelection()
    {
        SceneManager.LoadScene(GetScene(-3));
    }

    public static void LoadNextLevel()
    {
        int loadIndex = PlayerPrefs.GetInt(SELECTED_LEVEL, 0);
        loadIndex++;

        //check if end has been reached
        if (loadIndex >= levelSize)
            LoadMenu();
        else
            LoadLevel(loadIndex);
    }

    public static void LoadSelectedLevel()
    {
        int loadIndex = PlayerPrefs.GetInt(SELECTED_LEVEL, 0);

        if (loadIndex < 0) {
            loadIndex = 0;
            PlayerPrefs.SetInt(SELECTED_LEVEL, loadIndex);
        }

        SceneManager.LoadScene(GetScene(loadIndex));
    }

    public static int GetSelectedLevelIndex()
    {
        return PlayerPrefs.GetInt(SELECTED_LEVEL, 0);
    }

    public static void AddToSelectedLevel(int index)
    {
        int newIndex = PlayerPrefs.GetInt(SELECTED_LEVEL, 0);
        newIndex += index;
        newIndex = mod(newIndex, levelSize);
        int unlockedLevel = PlayerPrefs.GetInt(UNLOCKED_LEVEL, 0);

        if (newIndex > unlockedLevel)
            newIndex = 0;

        PlayerPrefs.SetInt(SELECTED_LEVEL, newIndex);
    }

    private static int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public static int GetUnlockedLevelIndex()
    {
        return PlayerPrefs.GetInt(UNLOCKED_LEVEL, 0);
    }

    //-------------------------------------------------------------------------------- FileSystem

    public static void SaveHighscore(Highscore highscore)
    {
        Dictionary<string, Highscore> highscoreDict = LoadAllHighscores();
        highscoreDict[highscore.mapName] = highscore;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, HIGHSCORE_FILE);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        try
        {
            formatter.Serialize(fileStream, highscoreDict);
        }
        finally
        {
            fileStream.Close();
        }
    }

    public static Dictionary<string, Highscore> LoadAllHighscores()
    {
        Dictionary<string, Highscore> highscoreDict = null;
        string path = Path.Combine(Application.persistentDataPath, HIGHSCORE_FILE);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            try
            {
                highscoreDict = formatter.Deserialize(fileStream) as Dictionary<string, Highscore>;
            }
            finally
            {
                fileStream.Close();
            }
        }

        if (highscoreDict != null)
            return highscoreDict;
        else
            return new Dictionary<string, Highscore>();
    }

    public static Highscore LoadHighScore()
    {
        return LoadHighScore(GetScene());
    }

    public static Highscore LoadHighScore(string mapName)
    {
        Dictionary<string, Highscore> highscoreDict = LoadAllHighscores();
        if (highscoreDict != null && highscoreDict.ContainsKey(mapName))
        {
            return highscoreDict[mapName];
        }

        return new Highscore(mapName);
    }
}
