using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltRollingAnimator : MonoBehaviour
{
    private Renderer rend;
    private float offset = 0.0f;
    public static float offsetSpeed = 0.01f;

	void Start ()
    {
        rend = GetComponent<Renderer>();
	}
	
	void FixedUpdate ()
    {
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0.0f));
        offset -= offsetSpeed;
    }
}
