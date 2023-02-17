using TMPro;
using UnityEngine;

public class CoinManagerScript : MonoBehaviour
{
    static public int CoinCounter;
    public GameObject Textbox;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("report", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (CoinCounter < 0) { CoinCounter = 0; }
        Textbox.GetComponent<TextMeshProUGUI>().text = "Coins: " + CoinCounter.ToString();
    }

    void report()
    {
        
    }
}