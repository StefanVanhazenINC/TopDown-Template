using System.Collections;
using System.Collections.Generic;
using TMPro;
using TopDownController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterInteractable : MonoBehaviour
{
    //для интеракции с предметами 

    [SerializeField] private float _radiusSearch;//радиус поиска в круг
    [SerializeField] private float _radiusSelector;// радиус под мышью 

    [SerializeField] private CharacterWeaponHolder _weaponHolder;
    [SerializeField] private HealthSystem _playerHealth;

    [Header("InteractableLayer")]
    [SerializeField] private LayerMask _interactabLayer;

    private bool _assistSearch = false;
    private Vector2 _mousePosition;
    private IInteractable _interactableObject;
    private Collider2D[] _weaponContainerColl = new Collider2D[10];
    private InputController _inputController;


    public UnityEvent<bool, Vector2> OnFindObject;


    public CharacterWeaponHolder WeaponHolder { get => _weaponHolder; }
    public HealthSystem PlayerHealth { get => _playerHealth; }


    private void Awake()
    {
        _inputController = GetComponent<InputController>();
        _inputController.OnInteractionEvent.AddListener(Interact);
        _inputController.LookEvent.AddListener(SetMousePosition);
        _inputController.SwitchDeviceInput.AddListener(EnableAssistSearch);
    }
  
    private void SetMousePosition(Vector2 mousePosition) 
    {
        _mousePosition = mousePosition;
    }
    private void Update()
    {
        Search();
    }
    private void Search() 
    {
        _weaponContainerColl = Physics2D.OverlapCircleAll(transform.position, _radiusSearch, _interactabLayer);
        Collider2D tempTarget;
        if (!_assistSearch)
        {
            tempTarget = Physics2D.OverlapCircle(_mousePosition, _radiusSelector, _interactabLayer);
        }
        else 
        {
            Vector2 direction = (_mousePosition - (Vector2)transform.position  );
            tempTarget = Physics2D.CircleCast(transform.position,_radiusSelector, direction , _radiusSearch,_interactabLayer).collider;
        }

        if (tempTarget != null)
        {
            if (_interactableObject != null)
            {
                _interactableObject.HoverObject(false, this);
            }
            for (int i = 0; i < _weaponContainerColl.Length; i++)
            {
                if (_weaponContainerColl[i] == tempTarget /*&& _lastObject != tempTarget*/)
                {
                    //_lastObject  = _weaponContainerColl[i];
                    _interactableObject = _weaponContainerColl[i].GetComponent<IInteractable>();
                    bool t_hoverEnable = _interactableObject.HoverObject(true, this);
                    OnFindObject?.Invoke(t_hoverEnable, _interactableObject.GetTransform.position);
                    return;
                }
                if (_interactableObject != null)
                {
                    _interactableObject.HoverObject(false, this);
                }
                _interactableObject = null;
            }
        }
        else 
        {
            //_lastObject = null;
            if (_interactableObject != null)
            {
                _interactableObject.HoverObject(false,this);
            }
            _interactableObject = null;
        }
       
        OnFindObject?.Invoke(false,Vector2.zero);
    }
    public void Interact(InputAction.CallbackContext ctx) 
    {
        if (_interactableObject != null) 
        {
            _interactableObject.Interact(this);
        }
    }
    public void Interact()
    {
        if (_interactableObject != null)
        {
            _interactableObject.Interact(this);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radiusSearch);
        Gizmos.DrawWireSphere(_mousePosition, _radiusSelector);
        if (_assistSearch) 
        {
            Vector2 direction = (_mousePosition - (Vector2)transform.position);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + (direction * _radiusSearch));
        }
    }

    public void EnableAssistSearch(bool value) 
    {
        
        _assistSearch = value;


    }

}
