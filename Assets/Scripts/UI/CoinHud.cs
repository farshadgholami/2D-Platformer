using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinHud : MonoBehaviour
{
    [SerializeField]
    private Text current_coins_;
    [SerializeField]
    private Text max_coins_;

    private PlayerStats stats_;
    public static CoinHud sSingletone;

    private int max_coin_count_;
    public int pMaxCoinCount { get { return max_coin_count_; } set { max_coin_count_ = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if (sSingletone == null)
            sSingletone = this;
        else
            Destroy(gameObject);

        stats_ = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
        current_coins_.text = "";
        int temp = stats_.Points;
        if ( temp < 100)
            current_coins_.text = "0";
        if (temp < 10)
            current_coins_.text += "0";

        current_coins_.text += temp.ToString();
    }

    public void AddMaxCoin(int count)
    {
        max_coin_count_ += count;

        max_coins_.text = "";
        if (max_coin_count_ < 100)
            max_coins_.text = "0";
        if (max_coin_count_ < 10)
            max_coins_.text += "0";

        max_coins_.text += max_coin_count_.ToString();
    }
}
