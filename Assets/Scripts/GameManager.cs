using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    public ZombieSpawner zombieSpawner;
    public ItemSpawner itemSpawner;

    private int score = 0;
    public bool IsGameOver {get; private set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameManager findGo = GameObject.FindWithTag("GameController");
        //uIManager.SetScore(score);
    }

    public void AddScore(int add)
    {
        if(IsGameOver)
        {
            return;
        }

        score += add;
        //uIManager.SetScore(score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        uIManager.SetActiveGameOverUI(true);

        itemSpawner.enabled = false;
    }

    public void OnEndGame()
    {
        IsGameOver = true;
        uIManager.SetActiveGameOverUI(true);

    }


}
