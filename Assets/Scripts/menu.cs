using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
   

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
           SceneManager.LoadScene(0);
        }
    }
}