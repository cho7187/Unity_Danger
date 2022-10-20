using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private GameController gameController;
	[SerializeField]
	private GameObject playerDieEffect;
	[SerializeField]
	private float moveSpeed = 5;
	[SerializeField]
	private float jumpForce = 15;
	[SerializeField]
	PlayerTrailSpawn playerTrailSpawner;
	private AudioSource audioSource;
	private Rigidbody2D rb2D;  

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		rb2D = GetComponent<Rigidbody2D>();
		rb2D.isKinematic = true;
	}
	private IEnumerator Start()
	{
		float originY = transform.position.y;
		float deltaY = 0.5f;
		float moveSpeedY = 2;

		while (true)
		{
			float y = originY + deltaY * Mathf.Sin(Time.time * moveSpeedY);
			transform.position = new Vector2(transform.position.x, y);

			yield return null;
		}
	}

	public void GameStart()
	{
		rb2D.isKinematic = false;
		rb2D.velocity = new Vector2(moveSpeed, jumpForce);

		StopCoroutine(nameof(Start));
		StartCoroutine(nameof(UpdateInput));
	}

	private IEnumerator UpdateInput()
	{
		while (true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				JumpTo();

				playerTrailSpawner.OnSpawns();
			}

			yield return null;
		}
	}
	private void ReverseXDir()
	{
		float x = -Mathf.Sign(rb2D.velocity.x);
		rb2D.velocity = new Vector2(x * moveSpeed, rb2D.velocity.y);
	}

	private void JumpTo()
	{
		rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			ReverseXDir();

			gameController.CollisionWithWall();

			audioSource.Play();
		}
		else if (collision.CompareTag("Spike"))
		{

			Instantiate(playerDieEffect, transform.position, Quaternion.identity);

			gameController.GameOver();

			gameObject.SetActive(false);
		}

	}
}
