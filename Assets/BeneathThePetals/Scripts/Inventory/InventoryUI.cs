using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera inventoryCamera;

    [SerializeField] private GameObject MainUIObj;
    [SerializeField] private GameObject InventoryUIObj;

    [Header("Text UI")]
    [SerializeField] private TMP_Text objName;
    [SerializeField] private TMP_Text objInfo;

    [Header("Inventory Settings")]
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float selectedRotationSpeed = 50f;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> defaultRotations = new List<Quaternion>();
    private bool isRotating = false;
    private int currentSelected = 0;

    void Start()
    {
        mainCamera.enabled = true;
        inventoryCamera.enabled = false;

        ArrangeObjectsInCircle();
        InventoryUIObj.SetActive(false);

        foreach (var obj in objects)
        {
            defaultRotations.Add(obj.transform.rotation);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleCamera();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isRotating)
        {
            StartCoroutine(RotateInventory(-1)); // Move to the right in the circle
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isRotating)
        {
            StartCoroutine(RotateInventory(1)); // Move to the left in the circle
        }

        if (objects.Length > 0 && !isRotating)
        {
            Vector3 rotation = objects[currentSelected].transform.rotation.eulerAngles;
            rotation.y += selectedRotationSpeed * Time.deltaTime;
            objects[currentSelected].transform.rotation = Quaternion.Euler(rotation);
        }
    }

    void ToggleCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        inventoryCamera.enabled = !inventoryCamera.enabled;

        if (MainUIObj.activeInHierarchy)
        {
            MainUIObj.SetActive(false);
            InventoryUIObj.SetActive(true);
        }
        else
        {
            MainUIObj.SetActive(true);
            InventoryUIObj.SetActive(false);
        }
    } 

    void ArrangeObjectsInCircle()
    {
        positions.Clear();
        int numberOfObjects = objects.Length;
        float angleStep = 360f / numberOfObjects;

        Vector3 firstPosition = pivot.position - pivot.forward * radius;
        positions.Add(firstPosition);
        objects[0].transform.position = firstPosition;
        ChangeUIText(objects[0]);

        for (int i = 1; i < numberOfObjects; i++)
        {
            float angle = i * angleStep;

            Vector3 position = Quaternion.Euler(0, angle, 0) * (firstPosition - pivot.position) + pivot.position;
            positions.Add(position);
            objects[i].transform.position = position;
        }
    }

    IEnumerator RotateInventory(int direction)
    {
        isRotating = true;
        List<Vector3> newPositions = new List<Vector3>();

        for (int i = 0; i < objects.Length; i++)
        {
            int newIndex = (i + direction + objects.Length) % objects.Length;
            newPositions.Add(positions[newIndex]);
        }

        ShowInfoUI(false);

        objects[currentSelected].transform.rotation = defaultRotations[currentSelected];

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].transform.position = Vector3.Lerp(objects[i].transform.position, newPositions[i], t);
            }
            yield return null;
        }
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = newPositions[i];
        }

        positions = newPositions;
        isRotating = false;

        currentSelected = (currentSelected + -direction + objects.Length) % objects.Length;
        ChangeUIText(objects[currentSelected]);
        ShowInfoUI(true);

    }

    void ChangeUIText(GameObject storyclue)
    {
        objName.text = storyclue.GetComponent<StoryClueInfo>().ReturnName();
        objInfo.text = storyclue.GetComponent<StoryClueInfo>().ReturnTextInfo();
    }

    void ShowInfoUI(bool show)
    {
        objName.enabled = show;
        objInfo.enabled = show;
    }
}
