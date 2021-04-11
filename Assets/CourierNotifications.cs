using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CourierNotifications : MonoBehaviour
{
    private bool sending = false;
    public bool Sending { get => sending; }

    private DataManager dataManager;

    void Start()
    {
        dataManager = GetComponent<DataManager>();
    }

    public IEnumerator SendResultsNotification(List<PlayerEntry> players, string beatBy, string beatMessage)
    {
        sending = true;

        List<string> emailsSentToAlready = new List<string>();
        List<string> phonesTextedAlready = new List<string>();

        // go through and send the notifications to all these players (except the ones who haven't changed rank)
        // they can all be treated the same here, Courier will handle them differently based on their data
        for (int i = 0; i < players.Count; i++)
        {
            // only send to those who have moved in the leaderboard
            if (players[i].playerData.currentPlace != players[i].playerData.previousPlace)
            {
                string email;
                string phone;
                if (dataManager.GetMessageMode() == 0) //unique mode
                {
                    // don't spam people with emails/texts if they're on the leaderboard multiple times    
                    if (emailsSentToAlready.Contains(players[i].playerProfile.email))
                    {
                        email = "";
                    }
                    else
                    {
                        email = players[i].playerProfile.email;
                        emailsSentToAlready.Add(email);
                    }
                    if (phonesTextedAlready.Contains(players[i].playerProfile.phone_number))
                    {
                        phone = "";
                    }
                    else
                    {
                        phone = players[i].playerProfile.phone_number;
                        phonesTextedAlready.Add(phone);
                    }
                }
                else
                {
                    // just send it all to everyone
                    email = players[i].playerProfile.email;
                    phone = players[i].playerProfile.phone_number;
                }

                WWWForm form = new WWWForm();
                form.AddField("event", "9GRDXE7RJZ42MSM12A87H4X3JCDC");
                form.AddField("recipient", players[i].recipient_id);
                form.AddField("override", "{}");
                form.AddField("data", "{\"playerName\":\"" + players[i].playerData.playerName + "\"," +
                                        "\"score\":" + players[i].playerData.score + "," +
                                        "\"currentPlace\":" + players[i].playerData.currentPlace + "," +
                                        "\"previousPlace\":" + players[i].playerData.previousPlace + "," + 
                                        "\"beatBy\":\"" + beatBy + "\"," +
                                        "\"beatMessage\":\"" + beatMessage + "\"," +
                                        "\"deviceName\":\"" + dataManager.GetDeviceName() + "\"}");
                form.AddField("profile", "{\"email\":\"" + email + "\"," + 
                                            "\"phone_number\":\"" + phone + "\"}");

                using (UnityWebRequest www = UnityWebRequest.Post("https://api.courier.com/send", form))
                {
                    www.SetRequestHeader("Authorization", "Bearer " + Auth.CourierKey);
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log("Form upload complete!");
                    }
                }
            }
        }
        sending = false;
    }
}
