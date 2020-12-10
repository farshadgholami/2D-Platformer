using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Vector2 target;

    public void Move()
    {
        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        CameraFollow.Instance.Smoothness = 1;
        obj.SetActive(false);
        obj.transform.position = target;
        obj.SetActive(true);
        yield return null;
        while (CameraFollow.Instance.Smoothness > 0.075f)
        {
            CameraFollow.Instance.Smoothness = Mathf.MoveTowards(CameraFollow.Instance.Smoothness, 0.075f, 5 * Time.deltaTime);
            yield return null;
        }
    }
}
