using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float jumpForce = 5f;
    public Transform playerCamera;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;

    private bool Crouching;

    private bool CanDamage = true;

    public int HP = 100;

    public GameObject SmallPLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // We'll handle rotation manually

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (HP < 0)
        {
            Destroy(gameObject);
        }
        SmallPLayer.transform.position = gameObject.transform.position;
        SmallPLayer.transform.rotation = gameObject.transform.rotation;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouching = true;
        }
        else
        {
            Crouching = false;
        }
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (HP <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        SceneManager.LoadScene(1);
    }

    void FixedUpdate()
    {
        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * moveSpeed;
        Vector3 currentVelocity = rb.velocity;

        // Preserve vertical velocity (gravity and jumping)
        velocity.y = currentVelocity.y;

        rb.velocity = velocity;
        if (Crouching)
        {
            transform.localScale = new Vector3(1, 0.4f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if grounded by simple collision with the ground layer or tagged ground
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (CanDamage)
            {
                StartCoroutine(Damage());
            }
        }
    }
    public IEnumerator Damage()
    {
        HP -= 25;
        CanDamage = false;
        yield return new WaitForSeconds(0.2f);
        CanDamage = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}