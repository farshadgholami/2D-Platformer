using UnityEngine;
using UnityEngine.UI;

public class FastMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private Material _material;
    private float _value;
    
    private void Start()
    {
        _material = GetComponent<Image>().material;
    }

    private void Update()
    {
        _value += speed * Time.deltaTime;
        _material.SetTextureOffset("_MainTex", Vector2.down * _value);
    }
}
