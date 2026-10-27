using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text waveInfoText;
    public Text scoreText;

    public GameObject gameOverUI;

    private void OnEnable()
    {
        //SetAmmoText(0, 0);
        SetActiveGameOverUI(false);
    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo} / {remainAmmo}";
    }

    public void SetWaveText(int wave, int count)
    {
        waveInfoText.text = $"Wave: {wave}\nEnemy Left: {count}";
    }
    public void SetScoreText(int score)
    {
        Debug.Log("ddddd" + score);
        scoreText.text = $"Score: {score}";
    }

    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
}
