using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerashake : MonoBehaviour
{
    public Vector3 OriginalPos;

    // Start is called before the first frame update
    void Start()
    {
        OriginalPos = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;


            transform.localPosition = new Vector3(x, y, OriginalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = OriginalPos;
    }
}
