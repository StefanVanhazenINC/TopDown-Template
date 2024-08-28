using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class PickUpObject: MonoBehaviour
{

    public abstract bool PickUp(CharacterInteractable character);


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterInteractable character))
        {
            if (PickUp(character))
            {
                Destroy(gameObject);
            }
        }
    }
}
