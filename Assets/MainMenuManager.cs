using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_InputField deviceNameInput;
    public GameObject mainTitleUi;
    public TextMeshProUGUI deviceName;
    public GameObject settingsPanel;
    public TextMeshProUGUI qualityText;
    public TextMeshProUGUI messageModeButtonText;
    public TextMeshProUGUI messageModeInfo;
    public GameObject highScoresPanel;
    public TextMeshProUGUI highScoresText;
    public GameObject howToPlayPanel;
    private DataManager dataManager;

    
    int currentQual;
    int numQualLevels;

    // Start is called before the first frame update
    void Start()
    {
        dataManager = (DataManager)FindObjectOfType(typeof(DataManager));
        deviceNameInput.text = dataManager.GetDeviceName();
        deviceName.text = dataManager.GetDeviceName();
        UpdateMessageMode();
        currentQual = QualitySettings.GetQualityLevel();
        numQualLevels = QualitySettings.names.Length;
        qualityText.text = "Graphics Quality: " + (currentQual + 1) + " out of " + (numQualLevels);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ClearHighScores()
    {
        dataManager.ClearHighScores();
    }

    public void ScreenSwitcher(int screenDest)
    {
        if (settingsPanel.activeSelf)
        {
            if (deviceNameInput.text != "")
                dataManager.SetDeviceName(deviceNameInput.text);
            deviceName.text = dataManager.GetDeviceName();
        }
        
        mainTitleUi.SetActive(false);
        settingsPanel.SetActive(false);
        highScoresPanel.SetActive(false);
        howToPlayPanel.SetActive(false);

        switch (screenDest)
        {
            case 0:
                mainTitleUi.SetActive(true);
                break;
            case 1:
                settingsPanel.SetActive(true);
                break;
            case 2:
                highScoresPanel.SetActive(true);
                highScoresText.text = dataManager.HighScoresToString(dataManager.GetHighScoresList());
                break;
            case 3:
                howToPlayPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void MessageModeToggle()
    {
        if (dataManager.GetMessageMode() == 1)
            dataManager.SetMessageMode(0);
        else
            dataManager.SetMessageMode(1);
        UpdateMessageMode();
    }

    private void UpdateMessageMode()
    {
        if (dataManager.GetMessageMode() == 0)
        {
            messageModeButtonText.text = "Unique Recipients Only";
            messageModeInfo.text = "If an email address or phone number is associated " +
                                    "with multiple players on the leaderboard, only the " +
                                    "first instance of the address/number will have a message sent to it.";
        }
        else
        {
            messageModeButtonText.text = "All Recipients";
            messageModeInfo.text = "Emails and text messages will be sent to every player " +
                                    "with an associated email or phone, regardless of how many " +
                                    "share the same email address or phone number.";
        }
    }

    public void ChangeQuality(bool increase)
    {
        if (increase)
        {
            QualitySettings.IncreaseLevel(true);
        }
        else
        {
            QualitySettings.DecreaseLevel(true);
        }
        currentQual = currentQual = QualitySettings.GetQualityLevel();
        qualityText.text = "Graphics Quality: " + (currentQual + 1) + " out of " + (numQualLevels);
    }
}
