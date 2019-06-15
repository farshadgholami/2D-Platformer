using UnityEngine;
using System.Collections;

public class ParallaxCamera : MonoBehaviour {

    public delegate void ParallaxCameraDelegate(Vector2 deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private float oldPosition;
    private float oldPositionY;
    void Start()
    {
        oldPosition = transform.position.x;
        oldPositionY = transform.position.y;
    }
    void Update()
    {
        if (transform.position.x != oldPosition)
        {
            if (onCameraTranslate != null)
            {
                Vector2 delta = new Vector2(oldPosition - transform.position.x,0);
                onCameraTranslate(delta);
            }
            oldPosition = transform.position.x;
        }
        if (transform.position.y != oldPositionY)
        {
            if (onCameraTranslate != null)
            {
                Vector2 delta = new Vector2(0,oldPositionY - transform.position.y);
                
                onCameraTranslate(delta);
            }
            oldPositionY = transform.position.y;
        }
    }
}
