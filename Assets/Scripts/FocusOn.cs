using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusOn : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 targetPos = target.transform.position;
        pos.x = targetPos.x;
        pos.y = targetPos.y;
        transform.position = pos;
    }
}
