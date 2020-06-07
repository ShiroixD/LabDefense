using UnityEngine;
using System.Collections;

public class WFX_Demo_DeleteAfterDelay : MonoBehaviour
{
	public float duration = 1.0f;
	public float currentTime = 0;

	void Start()
	{
		currentTime = duration;
	}

	void Update ()
	{
		if (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
		}

		if (currentTime < 0f)
		{
			gameObject.SetActive(false);
		}
	}
}
