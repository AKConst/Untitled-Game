using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
  
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)  
        {
            Debug.Log("enemy parry");
            Destroy(other.gameObject);// Destroy player's attack
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 7)  // Player layer
        {
            Debug.Log("player hit");
            Destroy(other.gameObject);
            GameManagerScript.instance.ChangeLevel(4);// Destroy player
        }
    }
}
