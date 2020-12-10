using System.Collections;
using UnityEngine;

public class MenuConroller : MonoBehaviour
{
    [SerializeField] private GameObject firstPage;
    [SerializeField] private Gravity playerGravity;
    [SerializeField] private GameObject fastMove;
    
    private bool _isOnFirstPage = true;
    
    private void Update()
    {
        if (_isOnFirstPage && Input.anyKeyDown) GoToLevelSelector();
    }

    private void GoToLevelSelector()
    {
        _isOnFirstPage = false;
        firstPage.SetActive(false);
        playerGravity.enabled = true;
        StartCoroutine(DisableFastMove());
    }

    private IEnumerator DisableFastMove()
    {
        yield return new WaitForSeconds(0.8f);
        fastMove.SetActive(false);
    }
}
