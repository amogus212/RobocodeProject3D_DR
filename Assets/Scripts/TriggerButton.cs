using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    //0 - Door
    //1 - Gun
    public int mode;
    public GameObject Door;
    public GameObject Player;
    public float Range;
    public float MoveMult;
    public GameObject Gun;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < Range)
            {
                if (mode == 0)
                {
                    StartCoroutine(LowerDoor());
                }else if (mode == 1)
                {
                    Gun.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }
    }
    IEnumerator LowerDoor()
    {
        yield return new WaitForSeconds(0.5f);
        int repeats = 500;
        while (repeats > 0)
        {
            repeats--;
            Door.transform.position = Door.transform.position - new Vector3(0, 0.1f * MoveMult, 0);
            yield return new WaitForSeconds(0.0001f);
        }
    }
}
