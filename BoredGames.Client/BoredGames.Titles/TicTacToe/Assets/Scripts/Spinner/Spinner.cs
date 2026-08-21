using Unity.Burst.Intrinsics;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(this.transform.position, Vector3.forward, _speed);
    }
}
