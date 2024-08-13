using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int level = 1;
    private int fight = 1;
    public static GameManager instance;
    public Player player;
    public List<Player> players;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateNewPlayer(); //TODO: Option to load or make new player
        PlayGame();
        Debug.Log("Game Over");
    }

    public void CreateNewPlayer()
    {
        //Copy the player in id slot 0 from the playerInfo json file and then save it back as a new player with the next available id
        players = LoadPlayers();
        //TODO: Customizable name
        string newName = "Bob";
        player = new Player(players[0], players.Count, newName); //Set the player id to the next available id
        player.Initialize();
        //Save back to the json file
        players.Add(player);
        SavePlayers(players);
    }

    private Player getPlayer(int player_id)
    {
        List<Player> players = LoadPlayers();
        Player player = players.Find(p => p.player_id == player_id);
        return player; //Note to self: This is the actual player entity from the list. It doesn't need to be saved back to it.
    }

    public void PlayGame()
    {
        do
        {
            player.currentLevel = level;
            player.currentFight = fight; //Set the player's current level and fight for saving
            SavePlayers();
            Debug.Log("Level " + level + " Fight " + fight);
            FightStates fightResult = TurnManager.instance.MainTurnTracker(player, level, fight);
            if (fightResult == FightStates.Win)
            {
                //TODO: Give player loot. Maybe health potions?
                //Give player xp
                player.totalExperience += LevelManager.instance.getFightXp(level, fight);
                Debug.Log("Player has won the fight");
                Debug.Log("Player has " + player.totalExperience + " experience");
                Debug.Log("Player is level " + player.level);
                //Increase fight number unless it is the last fight in the level
                //In that case increase the level
                //If the last level is beaten, the player wins
                if (fight < LevelManager.instance.GetLevel(level).fights.Count)
                {
                    fight++;
                }
                else if (level < LevelManager.instance.levelData.levels.Count)
                {
                    level++;
                    fight = 1;
                }
                else
                {
                    //End of the game?
                    Debug.Log("Player has won the game");
                    return;
                }
            }
            else //The method can only return win or lose, never continue
            {
                Debug.Log("Player has lost the fight");
                return;
            }
        } while (true); //While true because it will return when the game is over
    }

    public List<Player> LoadPlayers()
    {
        string json = Resources.Load<TextAsset>("playerInfo").text;
        List<Player> players = JsonUtility.FromJson<PlayerData>(json).players;
        return players;
    }

    public void SavePlayers()
    {
        PlayerData pd = new PlayerData();
        pd.players = players;
        string json = JsonUtility.ToJson(pd, true);
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/playerInfo.json", json);
    }
}
