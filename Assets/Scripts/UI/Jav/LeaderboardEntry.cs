using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class LeaderboardEntry
{
    public string user;
    public int score;
    public string time;

    public LeaderboardEntry(string user, int score, string time)
    {
        this.user = user;
        this.score = score;
        this.time = time;
    }
}

