using System;

[Serializable]
public class PlayerData
{
    public string playerName;
    public int score;
    public int currentPlace;
    public int previousPlace;
}

[Serializable]
public class PlayerProfile
{
    public string email;
    public string phone_number;
}

[Serializable]
public class PlayerEntry
{
    public string recipient_id;
    public PlayerData playerData;
    public PlayerProfile playerProfile;
}