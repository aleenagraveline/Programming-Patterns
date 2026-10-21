using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction hitAction;
    private InputAction lookAction;

    private Camera cam;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private float timer = 1;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private int health;
    [SerializeField] private int points;

    public static event Action<int> OnTakeDamage;
    public static event Action<int> OnGainPoints;
    public static event Action OnDeath;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        hitAction = playerInput.actions["Hit"];
        lookAction = playerInput.actions["Look"];

        cam = FindAnyObjectByType<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseRotation = lookAction.ReadValue<Vector2>();

        rotationY += mouseRotation.x * mouseSensitivity * Time.deltaTime;
        rotationX -= mouseRotation.y * mouseSensitivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -45f, 45f);

        cam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        if (hitAction.WasPressedThisFrame())
        {
            Hit();
        }

        timer -= Time.deltaTime;
        if (health == 0)
        {
            Debug.Log("You lose");
            OnDeath?.Invoke();
        }
        else if(timer <= 0)
        {
            GainPoints(1);
            timer = 1;
        }
    }

    void Hit()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            Debug.Log("Hit: " + hit.collider.name);
            if(hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>().Die();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && this.CompareTag("Player"))
        {
            Debug.Log("Damage Player");
            health--;
            OnTakeDamage?.Invoke(health);
            other.gameObject.GetComponent<EnemyController>().Clear();
        }
    }

    public void GainPoints(int pointGain)
    {
        points += pointGain;
        OnGainPoints?.Invoke(points);
    }

    public int GetHealth()
    {
        return health;
    }
}
