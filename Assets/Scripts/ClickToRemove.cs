using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToRemove : MonoBehaviour {

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
