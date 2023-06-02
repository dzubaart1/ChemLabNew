using System;
using UnityEngine;

public class ChemicGlassCanvasCntrl : MonoBehaviour
{
    private Transform obj1;
    public Transform target;
    public Vector3 newDir;

    private void Start()
    {
        obj1 = gameObject.transform;
    }

    private void Update()
    {
        newDir = Vector3.RotateTowards(obj1.forward, target.position - obj1.position, Time.deltaTime * 1f, 5);
        obj1.rotation = Quaternion.LookRotation(newDir);
        obj1.eulerAngles = new Vector3(0, obj1.eulerAngles.y, 0);

    }
}
