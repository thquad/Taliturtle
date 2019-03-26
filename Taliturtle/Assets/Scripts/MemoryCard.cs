using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The static MemoryCard class.
/// Used for loading/saving maps/scenes/highscores.
/// </summary>
public static class MemoryCard
{
    public static readonly int levelSize = 14; //manually change with level amount, maybe fix later
    public static readonly string SELECTED_LEVEL = "selectedLevel";
    public static readonly string UNLOCKED_LEVEL = "unlockedLevel";
    public static readonly string HIGHSCORE_FILE = "highscore";

    //-------------------------------------------------------------------------------- Scene Management

    /// <summary>
    /// Generates a Dictionary with the names of all scenes and returns it.
    /// </summary>
    /// <returns>Dictionary containing the names of all scenes.</returns>
    public static Dictionary<int,string> GetScenes()
    {
        Dictionary<int, string> scenes = new Dictionary<int, string>();

        scenes[-3] = "menu_levelselectscreen";
        scenes[-2] = "menu_splashscreen";
        scenes[-1] = "menu_loadingscreen";
        scenes[0] = "map_04";
        scenes[1] = "map_05";
        scenes[2] = "map_03";
        scenes[3] = "map_08";
        scenes[4] = "map_06";
        scenes[5] = "map_07";
        scenes[6] = "map_09";
        scenes[7] = "map_11";
        scenes[8] = "map_12";
        scenes[9] = "map_13";
        scenes[10] = "map_14";
        scenes[11] = "map_15";
        scenes[12] = "map_16";
        scenes[13] = "map_10";

        return scenes;
    }

    /// <summary>
    /// Gets name of the currently selected scene.
    /// </summary>
    /// <returns>Name of the currently selected scene.</returns>
    public static string GetScene()
    {
        return GetScene(PlayerPrefs.GetInt(SELECTED_LEVEL, 0));
    }

    /// <summary>
    /// Gets name of specific scene.
    /// Returns loading menu if index is larger than level amount.
    /// </summary>
    /// <param name="index">Index of the scene.</param>
    /// <returns>Name of the scene.</returns>
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

    /// <summary>
    /// Loads the specific level.
    /// </summary>
    /// <param name="index">Index of the scene.</param>
    public static void LoadLevel(int index)
    {

        PlayerPrefs.SetInt(SELECTED_LEVEL, index);

        if (index > PlayerPrefs.GetInt(UNLOCKED_LEVEL, 0))
            PlayerPrefs.SetInt(UNLOCKED_LEVEL, index);

        SceneManager.LoadScene(GetScene(index));
    }

    /// <summary>
    /// Loads the loading menu.
    /// </summary>
    public static void LoadMenu()
    {
        SceneManager.LoadScene(GetScene(-1));
    }

    /// <summary>
    /// Loads the splashscreen.
    /// </summary>
    public static void LoadSplash()
    {
        SceneManager.LoadScene(GetScene(-2));
    }

    /// <summary>
    /// Loads the level selection screen.
    /// </summary>
    public static void LoadLevelSelection()
    {
        SceneManager.LoadScene(GetScene(-3));
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
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

    /// <summary>
    /// Loads the currently selected level.
    /// </summary>
    public static void LoadSelectedLevel()
    {
        int loadIndex = PlayerPrefs.GetInt(SELECTED_LEVEL, 0);

        if (loadIndex < 0) {
            loadIndex = 0;
            PlayerPrefs.SetInt(SELECTED_LEVEL, loadIndex);
        }

        SceneManager.LoadScene(GetScene(loadIndex));
    }

    /// <summary>
    /// Gets the index of the currently selected level.
    /// </summary>
    /// <returns></returns>
    public static int GetSelectedLevelIndex()
    {
        return PlayerPrefs.GetInt(SELECTED_LEVEL, 0);
    }

    /// <summary>
    /// Increment or decrement the currently selected level.
    /// </summary>
    /// <param name="add">Value to add.</param>
    public static void AddToSelectedLevel(int add)
    {
        int newIndex = PlayerPrefs.GetInt(SELECTED_LEVEL, 0);
        int unlockedLevel = PlayerPrefs.GetInt(UNLOCKED_LEVEL, 0)+1;

        newIndex += add;
        newIndex = newIndex%unlockedLevel;

        //wrap around like normal modulo
        if (newIndex < 0)
            newIndex = unlockedLevel + newIndex;

        //wrap around 
        if (newIndex > unlockedLevel)
            newIndex = 0;

        PlayerPrefs.SetInt(SELECTED_LEVEL, newIndex);
    }

    /// <summary>
    /// Getter for unlocked levels.
    /// </summary>
    /// <returns>Unlocked levels.</returns>
    public static int GetUnlockedLevelIndex()
    {
        return PlayerPrefs.GetInt(UNLOCKED_LEVEL, 0);
    }

    //-------------------------------------------------------------------------------- FileSystem

    /// <summary>
    /// Saves the given highscore in a file.
    /// </summary>
    /// <param name="highscore">Highscore to save.</param>
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

    /// <summary>
    /// Reads the file and returns a Dictionary with all the saved highscores.
    /// </summary>
    /// <returns>Dictionary with all saved highscores.</returns>
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

    /// <summary>
    /// Loads highscore of currently selected level.
    /// </summary>
    /// <returns>Highscore of currently selected level.</returns>
    public static Highscore LoadHighScore()
    {
        return LoadHighScore(GetScene());
    }

    /// <summary>
    /// Loads the highscore with the scene name or "map name".
    /// </summary>
    /// <param name="mapName">Which map to load.</param>
    /// <returns>Highscore of the map.</returns>
    public static Highscore LoadHighScore(string mapName)
    {
        Dictionary<string, Highscore> highscoreDict = LoadAllHighscores();
        if (highscoreDict != null && highscoreDict.ContainsKey(mapName))
        {
            return highscoreDict[mapName];
        }

        return new Highscore(mapName);
    }

    /// <summary>
    /// Checks if a savestate exists and resets the PlayerPrefs.
    /// </summary>
    public static void CheckNewGame()
    {
        string path = Path.Combine(Application.persistentDataPath, HIGHSCORE_FILE);

        if (!File.Exists(path))
        {
            PlayerPrefs.SetInt(SELECTED_LEVEL, 0);
            PlayerPrefs.SetInt(UNLOCKED_LEVEL, 0);
        }
    }
}
