using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform plateCounterPrefab;
    [SerializeField] private float offsetBetweenPlates = .1f;

    private List<GameObject> plates;

    private void Awake()
    {
        plates = new List<GameObject>();
    }
    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateGrabbed += PlateCounter_OnPlateGrabbed;
    }

    private void PlateCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform spawned = Instantiate(plateCounterPrefab, plateCounter.GetSpawnPoint());
        spawned.localPosition = new Vector3(0, plates.Count * offsetBetweenPlates, 0);
        plates.Add(spawned.gameObject);
    }

    private void PlateCounter_OnPlateGrabbed(object sender, EventArgs e)
    {
        GameObject plate = plates[plates.Count - 1];
        plates.Remove(plate);
        Destroy(plate);
    }
}
