using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]GameObject obj;

    // Update is called once per frame
    void Update()
    {
        transform.position = obj.transform.position + new Vector3(0,5,-2.8f);
    }
}
