using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class Location : MonoBehaviour
{
    public static Location Instance { get; set; }

    public TextMeshProUGUI status;

    public float latitude;
    public float longitude;
    public float altitude;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            UnityEngine.Debug.Log("GPS not enabled");
            status.text = "GPS not enabled";
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            UnityEngine.Debug.Log("Timed out");
            status.text = "Timed out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            UnityEngine.Debug.Log("Unable to determine device location");
            status.text = "Unable to determine device location";
            yield break;
        }

        while (true)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;
            yield return new WaitForSeconds(10);
        }
    }
}