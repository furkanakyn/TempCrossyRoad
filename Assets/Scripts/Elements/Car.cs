using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public List<GameObject> carModels;
    public float rotationSpeed = 600f;
    public bool isCarDirectionLeft;

    internal void StartCar(int row, bool toLeft, float carTravelDuration)
    {
        if (carModels == null || carModels.Count == 0)
        {
            Debug.LogError("carModels listesi boþ veya atanmýþ deðil.");
            return;
        }
        foreach (GameObject carModels in carModels)
        {
            carModels.SetActive(false);
        }
        if (carModels == null || carModels.Count == 0)
        {
            Debug.LogError("carModels listesi boþ veya atanmýþ deðil.");
            return;
        }
        carModels[UnityEngine.Random.Range(0, carModels.Count)].SetActive(true);

        if (toLeft)
        {
            transform.position = new Vector3(29f, 0, row);
            transform.DOMoveX(0, carTravelDuration).SetEase(Ease.Linear).OnComplete(DestsoryCar);
            transform.Rotate(0, 180, 0);
        }
        else
        {
            transform.position = new Vector3(0, 0, row);
            transform.DOMoveX(29f, carTravelDuration).SetEase(Ease.Linear).OnComplete(DestsoryCar);
        }
        isCarDirectionLeft = toLeft;

    }
    void DestsoryCar()
    {
        transform.DOKill();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }

    void RotateWheels()
    {
        foreach (Transform wheel in transform.GetComponentsInChildren<Transform>())
        {
            if (wheel.gameObject == null)
            {
                return;
            }
            if (wheel.gameObject.layer == LayerMask.NameToLayer("RightWheel"))
            {
                wheel.localRotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
            }
            else if (wheel.gameObject.layer == LayerMask.NameToLayer("LeftWheel"))
            {
                wheel.localRotation *= Quaternion.Euler(-rotationSpeed * Time.deltaTime, 0, 0);
            }

        }
    }
    private void Update()
    {
        RotateWheels();
    }

}
