using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Highscore class.
/// Used for saving the highscore of a map in a file.
/// </summary>
[Serializable]
public class Highscore
{
    public string mapName;
    public float timeInSeconds;

    public Highscore(string mapIndex)
    {
        this.mapName = mapIndex;
        timeInSeconds = 0;
    }

    public Highscore(string mapIndex, float timeInSeconds)
    {
        this.mapName = mapIndex;
        this.timeInSeconds = timeInSeconds;
    }
}
