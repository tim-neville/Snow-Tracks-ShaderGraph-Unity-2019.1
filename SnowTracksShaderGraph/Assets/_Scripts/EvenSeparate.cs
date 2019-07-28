using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenSeparate : MonoBehaviour
{
    public GameObject prefab;
    public int numToCreate = 1;
    private Transform[] items;
    public Vector3 spacing = Vector3.zero;

    public Vector3 startLocation = Vector3.zero;
    public Vector3 rotation = Vector3.zero;

    void Start()
    {
        items = new Transform[numToCreate];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Instantiate(prefab, startLocation, Quaternion.Euler(rotation), this.transform).transform;
        }
    }

    void Update()
    {
        items[0].position = startLocation;

        for (int i = 1; i < items.Length; i++)
        {
            items[i].position = items[i - 1].position + spacing;
        }
    }
}
