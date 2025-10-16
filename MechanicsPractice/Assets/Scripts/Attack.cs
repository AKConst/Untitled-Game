using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            Debug.Log("Player parry");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if(other.gameObject.layer == 6)
        {
            Debug.Log("enemy hit");
            Destroy(other.gameObject);
        }
    }
}
