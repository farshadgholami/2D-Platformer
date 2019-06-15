using UnityEngine;
using System.Collections;

public class ParallaxLayer : MonoBehaviour {

    public float parallaxFactor;
    public void Move(Vector2 delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos -= new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor * 0.1f);
        transform.localPosition = newPos;
    }
}
