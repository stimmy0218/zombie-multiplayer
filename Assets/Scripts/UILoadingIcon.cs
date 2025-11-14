using UnityEngine;

public class UILoadingIcon : MonoBehaviour
{
    public float speed = 500f;

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}