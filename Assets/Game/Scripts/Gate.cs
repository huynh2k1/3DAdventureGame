using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject gateObj;

    public BoxCollider boxCollider;

    public float openDuration = 2f;

    public float openTargetY = -2f;


    IEnumerator OpenGate()
    {
        float currentOpenDuration = 0;
        Vector3 startPos = gateObj.transform.position;
        Vector3 targetPos = startPos + Vector3.up * openTargetY;

        while(currentOpenDuration < openDuration)
        {
            currentOpenDuration += Time.deltaTime;
            gateObj.transform.position = Vector3.Lerp(startPos, targetPos, currentOpenDuration / openDuration);

            yield return null;
        }

        boxCollider.enabled = false;
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenGate());
    }
}
