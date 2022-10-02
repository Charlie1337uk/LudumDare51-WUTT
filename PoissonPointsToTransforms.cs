using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonPointsToTransforms : MonoBehaviour
{
    public poissonTest _poissonTest;
    public List<GameObject> PlaySpaces;
    public GameObject HideCentre;
    public float Divisor;
    void Start()
    {
        _poissonTest = GetComponent<poissonTest>();
        FillPlaySpaces();
    }
    public void FillPlaySpaces()
    {
        foreach (var spot in _poissonTest.points)
        {
            GameObject newGO = new GameObject();
            PlaySpaces.Add(newGO);
            newGO.transform.parent = HideCentre.transform;
            newGO.transform.position = HideCentre.transform.position;
            newGO.transform.Translate(new Vector3(spot.x*Divisor,0,spot.y*Divisor));
        }
    }
    
}
