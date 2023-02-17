using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public GameObject Coin;
    public GameObject CoinManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CoinManagerScript.CoinCounter++;
            Destroy(Coin);
        }
    }
}