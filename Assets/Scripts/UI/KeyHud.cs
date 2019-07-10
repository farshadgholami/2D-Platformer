using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHud : MonoBehaviour
{
    [SerializeField]
    private Text red_text_;
    [SerializeField]
    private Text yellow_text_;
    [SerializeField]
    private Text blue_text_;

    private PlayerStats stats_;


    // Start is called before the first frame update
    void Start()
    {
        stats_ = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        red_text_.text = "";
        yellow_text_.text = "";
        blue_text_.text = "";

        int temp = Mathf.Min(stats_.RedKey,99);
        if (temp < 10)
            red_text_.text = "0";
        red_text_.text += temp.ToString();
        temp = Mathf.Min(stats_.YellowKey , 99);
        if (temp < 10)
            yellow_text_.text = "0";
        yellow_text_.text += temp.ToString();
        temp = Mathf.Min(stats_.BlueKey , 99);
        if (temp < 10)
            blue_text_.text = "0";
        blue_text_.text += temp.ToString();
    }
}
