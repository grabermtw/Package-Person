using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class EndMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoresText;
    public TextMeshProUGUI yourScore;
    public TextMeshProUGUI endScreenMessage;
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField phoneInput;
    public TMP_InputField messageInput;
    public GameObject submissionFields;
    public GameObject mainMenuButton;

    private DataManager dataManager;
    private CourierNotifications courierNotifications;


    private List<PlayerEntry> highScoresList;
    

    // Start is called before the first frame update
    void Start()
    {
        dataManager = (DataManager)FindObjectOfType(typeof(DataManager));
        courierNotifications = dataManager.GetComponent<CourierNotifications>();
        highScoresList = dataManager.GetHighScoresList();
        highScoresText.text = dataManager.HighScoresToString(highScoresList);
        yourScore.text = "" + dataManager.Score;
        
        if (highScoresList.Count < 10 || highScoresList.Count > 0 && highScoresList[highScoresList.Count - 1].playerData.score < dataManager.Score)
        {
            endScreenMessage.text = "Congratulations!";
        }
        else
        {
            submissionFields.SetActive(false);
            endScreenMessage.text = "Better luck next time!";
            mainMenuButton.SetActive(true);
        }
        
    }

    public void SubmitScores()
    {
        PlayerEntry newPlayer = new PlayerEntry();
        
        
        newPlayer.recipient_id = GenerateRecipientId(nameInput.text);

        newPlayer.playerData = new PlayerData();
        newPlayer.playerData.score = dataManager.Score;
        newPlayer.playerData.previousPlace = -1;
        newPlayer.playerData.currentPlace = -1;
        newPlayer.playerData.playerName = nameInput.text;
        
        newPlayer.playerProfile = new PlayerProfile();
        newPlayer.playerProfile.email = emailInput.text;
        // make the phone numbers just the digits
        newPlayer.playerProfile.phone_number = String.Concat(phoneInput.text.Where(c => !Char.IsDigit(c)));

        PlayerEntry ejectedPlayer = dataManager.AddHighScore(highScoresList, newPlayer);

        highScoresList = dataManager.GetHighScoresList();
        Debug.Log("count: " + highScoresList.Count);
        highScoresText.text = dataManager.HighScoresToString(highScoresList);
        submissionFields.SetActive(false);
        
        // add the ejected player back to the end of the list so that they can be notified
        if (ejectedPlayer != null)
            highScoresList.Add(ejectedPlayer);
        StartCoroutine(courierNotifications.SendResultsNotification(highScoresList, nameInput.text, messageInput.text));
        StartCoroutine(WaitUntilSent());
    }

    // prevent changing scenes until we've finished doing our POST request
    private IEnumerator WaitUntilSent()
    {
        yield return new WaitUntil(() => !courierNotifications.Sending);
        mainMenuButton.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private string GenerateRecipientId(string name)
    {
        // remove any whitespace from the name
        string id = String.Concat(name.Where(c => !Char.IsWhiteSpace(c)));
        string charPile = "abcdefghijklmnopqrstuvwxyz0123456789";
        int charAmount = Random.Range(5, 15);
        for (int i = 0; i < charAmount; i++)
        {
            id += charPile[Random.Range(0, charPile.Length)];
        }
        return id;
    }
}
