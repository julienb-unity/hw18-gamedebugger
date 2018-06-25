using UnityEngine;

public class CubeController : MonoBehaviour
{
	public float Speed = 7;

	void Update () 
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.localPosition += Time.deltaTime * Speed * new Vector3(-1, 0, 0);
		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.localPosition += Time.deltaTime * Speed * new Vector3(1, 0, 0);
		}
	}
}
