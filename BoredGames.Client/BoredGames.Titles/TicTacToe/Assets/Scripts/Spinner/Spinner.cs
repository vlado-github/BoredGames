using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(this.transform.position, Vector3.forward, 0.5f);
    }
}
