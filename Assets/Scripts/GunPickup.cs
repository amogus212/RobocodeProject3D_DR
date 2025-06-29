using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GunPickup : MonoBehaviour
{
    public Transform holdPosition;    // Desired position when aiming   :)
    public Transform restPosition;    // Resting position in world    :)
    private Quaternion aimRotation;  // Desired rotation when aiming (usually Quaternion.Euler(0,0,0))   :)
    public Vector3 aimRotationVTREE;  
    public float moveSpeed = 10f;
    public LayerMask raycastMask;      // Set this to include only layers you want to hit (e.g., Player)   :)


    private bool isAiming = false;

    private bool Reloading = false;

    public TextMeshProUGUI BulletShower;
    public int BulletLoaded;
    public int BulletMax;
    public int BulletHave;
    void Update()
    {
        aimRotation = Quaternion.Euler(aimRotationVTREE);   
        bool aimInput = Input.GetButton("Fire2");

        if (aimInput && !isAiming)
            isAiming = true;
        else if (!aimInput && isAiming)
            isAiming = false;

        if (isAiming)
        {
            // Move towards holdPosition
            transform.position = holdPosition.position;
            // Rotate towards aimRotation * holdPosition.rotation (apply rotation offset)
            transform.rotation = holdPosition.rotation * aimRotation;
            
        }
        else
        {
            // Move towards restPosition and rest rotation
            transform.position = restPosition.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, restPosition.rotation, Time.deltaTime * moveSpeed);
        }
        if (Input.GetMouseButtonDown(0) && !Reloading && BulletLoaded > 0)
        {
            BulletLoaded--;
            Vector3 offset =  new Vector3(0, 0, 0);
            if (!isAiming)
            {
                offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            }
            // Optional: Draw the ray in the editor
            Debug.DrawRay(transform.position, (Camera.main.transform.forward + offset) * 100, Color.red, 3);

            // Perform the raycast
            if (Physics.Raycast(transform.position, Camera.main.transform.forward + offset, out RaycastHit hit, 100, raycastMask))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("ZOMBIE hit by raycast!");
                    hit.collider.gameObject.GetComponent<Enemy>().hp -= Random.Range(20, 30);
                    // Add your logic here (e.g., shoot, chase, alert, etc.)
                }
                else
                {
                    Debug.Log("Raycast hit something else: " + hit.transform.name);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !Reloading && BulletLoaded < BulletMax && BulletHave > BulletMax - BulletLoaded)
        {
            StartCoroutine(Reload());
        }
        if (!Reloading)
        {
            BulletShower.text = BulletLoaded + "/" + BulletMax + "/" + BulletHave;
        }
    }
    IEnumerator Reload()
    {
        Reloading = true;
        BulletShower.text = "Reloading";
        yield return new WaitForSeconds(0.1f);
        BulletShower.text = "Reloading.";
        yield return new WaitForSeconds(0.1f);
        BulletShower.text = "Reloading..";
        yield return new WaitForSeconds(0.1f);
        BulletShower.text = "Reloading...";
        yield return new WaitForSeconds(0.1f);
        BulletShower.text = "Reloading....";
        yield return new WaitForSeconds(0.1f);
        BulletShower.text = "Reloading.....";
        yield return new WaitForSeconds(0.1f);
        BulletHave -= BulletMax - BulletLoaded;
        BulletLoaded = BulletMax;
        Reloading = false;
    }
}