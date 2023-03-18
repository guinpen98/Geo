using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISystem : MonoBehaviour
{
    public TextMeshProUGUI latitude;
    public TextMeshProUGUI longitude;
    public TextMeshProUGUI altitude;


    void Update()
    {
        latitude.text = $"Latitude:{Location.Instance.latitude}\nLongitude:{Location.Instance.longitude}\nAltitude:{Location.Instance.altitude}";
    }
}
