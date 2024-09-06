using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TopDown_Template
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class PickUpObject : MonoBehaviour
    {
        #region PickUpObject Method
        public abstract bool PickUp(CharacterInteractable character);
        #endregion

        #region Unity Callback
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
        #endregion
    }
}
