using UnityEngine;
using System;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider2D;

    

    void Start()
    {
        if(_collider2D == null)
        {
            _collider2D = GetComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    // Use this function to activate or deactivate the collider. Since its "public", other scripts can use it.
    public void ToggleCollider()
    {
        _collider2D.enabled = !_collider2D.enabled;
    }

    // Use the "ActivateCollider" and "DeactivateCollider" in case "ToggleCollider" malfunctions or specific cases
    public void ActivateCollider()
    {
        _collider2D.enabled = true;
    }

    public void DeactivateCollider()
    {
        _collider2D.enabled = false;
    }

    // COROUTINES
    
}
