using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] int healAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().RestoreHealth(healAmount);

            this.gameObject.SetActive(false);
        }
    }
}
