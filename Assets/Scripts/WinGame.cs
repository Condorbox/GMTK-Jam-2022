using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    [SerializeField] private string winLevel;
    [SerializeField] private CameraManager cameraManager;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //cameraManager.ChangeCamera();
            SceneManager.LoadScene(winLevel);
        }
    }
}
