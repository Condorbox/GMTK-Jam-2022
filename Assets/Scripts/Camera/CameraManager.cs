using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private Transform target;
    [SerializeField] private float offset;
    [SerializeField] private float timeToHide = 2f;
    private float counter;
    private bool activated = false;

    private void Start()
    {
        HideActionCamera();

    }

    private void Update()
    {
        if (activated)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                HideActionCamera();
            }
        }
    }

    public void ChangeCamera()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 offsetVector = Quaternion.Euler(0, 90, 0) * dir;
        Vector3 cameraCharacterHeight = Vector3.up * 10.7f;
        Vector3 actionCameraPosition = transform.position + cameraCharacterHeight + offsetVector;
        ScreenSake.Instance.Shake(10f);
        actionCameraGameObject.transform.position = actionCameraPosition;
        actionCameraGameObject.transform.LookAt(target.position + cameraCharacterHeight);
        ShowActionCamera();
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
        activated = true;
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
        counter = timeToHide;
        activated = false;
    }
}
