using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private SpikeSpawn[] spikeSpawners;
	[SerializeField]
	private Player player;
	private UIController uiController;
	private RandomColor randomColor;
    private int currentSpawn = 0;
	private int currentScore = 0;

    private void Awake()
    {
		uiController = GetComponent<UIController>();
		randomColor = GetComponent<RandomColor>();
    }
	private IEnumerator Start()
	{
		while (true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				player.GameStart();
				uiController.GameStart();
				yield break;
			}

			yield return null;
		}
	}

	public void CollisionWithWall()
	{
		UpdateSpikes();

		currentScore++;
		uiController.UpdateScore(currentScore);

		randomColor.OnChange();
	}
	public void GameOver()
    {
		StartCoroutine(nameof(GameOverProcess));
    }
	private void UpdateSpikes()
	{

		spikeSpawners[currentSpawn].ActivateAll();

		currentSpawn = (currentSpawn + 1) % spikeSpawners.Length;

		spikeSpawners[currentSpawn].DeactivateAll();
	}
	public int Scores()
    {
		return currentScore;
    }

	private IEnumerator GameOverProcess()
	{
		if (currentScore > PlayerPrefs.GetInt("HIGHSCORE"))
		{
			PlayerPrefs.SetInt("HIGHSCORE", currentScore);
		}

		uiController.GameOver();

		while (true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}

			yield return null;
		}
	}
}
