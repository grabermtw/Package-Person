using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string DEFAULT_DEVICE_NAME = "Mystery Arcade";
    private static bool exists;

    private int score;
    public int Score { get => score; set => score = value; }
    private int messageMode;


    void Awake()
    {
        if (exists)
            Destroy(gameObject);
        else
            exists = true;
        DontDestroyOnLoad(gameObject);

        messageMode = PlayerPrefs.GetInt("messageMode", 0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetMessageMode(int mode)
    {
        messageMode = mode;
        PlayerPrefs.SetInt("messageMode", mode);
    }

    public int GetMessageMode()
    {
        return messageMode;
    }

    public void SetDeviceName(string newDeviceName)
    {
        PlayerPrefs.SetString("deviceName", newDeviceName);
    }

    public string GetDeviceName()
    {
        return PlayerPrefs.GetString("deviceName", DEFAULT_DEVICE_NAME);
    }

    public List<PlayerEntry> GetHighScoresList()
    {
        List<PlayerEntry> highScoresList = new List<PlayerEntry>();
        for (int i = 0; i < 10; i++)
        {
            string entryJson = PlayerPrefs.GetString("highScoreEntry" + i, "");
            if (entryJson != "")
            {
                highScoresList.Add(JsonUtility.FromJson<PlayerEntry>(entryJson));
            }
        }
        return highScoresList;
    }

    // returns the player who was ejected off the leaderboard, if there was one
    public PlayerEntry AddHighScore(List<PlayerEntry> highScores, PlayerEntry newPlayer)
    {
        // find the position to insert into
        if (highScores.Count == 0)
        {
            highScores.Add(newPlayer);
        }
        else
        {
            int i = 0;
            while (i < highScores.Count && highScores[i].playerData.score >= newPlayer.playerData.score)
            {
                i++;
            }
            highScores.Insert(i, newPlayer);
        }
        
        // update currentPlace and previousPlace for each player        
        // convert each entry to json and save it in the playerprefs
        for (int j = 0; j < Mathf.Min(highScores.Count, 10); j++)
        {
            highScores[j].playerData.previousPlace = highScores[j].playerData.currentPlace;
            highScores[j].playerData.currentPlace = j + 1;
            string hiJson = JsonUtility.ToJson(highScores[j]);
            PlayerPrefs.SetString("highScoreEntry" + j, hiJson);
        }
        
        // return whoever got ejected from the leaderboard
        if (highScores.Count == 11)
        {
            highScores[10].playerData.previousPlace = highScores[10].playerData.currentPlace;
            highScores[10].playerData.currentPlace = 11;
            return highScores[10];
        }
        else
            return null;
    }

    public string HighScoresToString(List<PlayerEntry> highScoresList)
    {
        string highScoresString = "";
        int i;
        for (i = 0; i < highScoresList.Count; i++)
        {
            highScoresString += (i + 1) + ") " + highScoresList[i].playerData.score + " " + highScoresList[i].playerData.playerName + "\n";
        }
        while (i < 10)
        {
            highScoresString += (i + 1) + ")\n";
            i++;
        }
        return highScoresString;
    }

    // use with caution
    public void ClearHighScores()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetString("highScoreEntry" + i, "");
        }
    }
}
