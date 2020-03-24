using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementLogic : MonoBehaviour
{
    public int numElement;
    public GameObject parent;
    private void OnMouseDown()
    {

        transform.localPosition = parent.GetComponent<CubeLogic>().Shift(numElement);
    }
}
