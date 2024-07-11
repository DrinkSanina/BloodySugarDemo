using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMove : MonoBehaviour
{
    public float dashSpeed = 300.0f;
    public float dashTime = 0.25f;

    public IEnumerator Dash(CharacterController controller, Vector3 direction)
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
