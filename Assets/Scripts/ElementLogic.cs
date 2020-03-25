using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementLogic : MonoBehaviour
{
    public int numElement;
    public GameObject parent;

    public void SetPos()
    {
        transform.localPosition = parent.GetComponent<CubeLogic>().getStart(numElement);
    }

        private void OnMouseDown()
    {

        transform.localPosition = parent.GetComponent<CubeLogic>().Shift(numElement);
    }
}
