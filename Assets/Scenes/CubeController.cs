using UnityEngine;

public class CubeController : MonoBehaviour
{
	public float Speed = 7;

	void Update () 
	{
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			transform.localPosition += Time.deltaTime * Speed * -Vector3.right;
		}
		
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			transform.localPosition += Time.deltaTime * Speed * Vector3.right;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			transform.localPosition += Time.deltaTime * Speed * Vector3.up;
		}

		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			transform.localPosition += Time.deltaTime * Speed * -Vector3.up;
		}
	}
}
