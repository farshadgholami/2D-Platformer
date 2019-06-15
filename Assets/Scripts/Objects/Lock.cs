using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField]
    private KeyColor color;
    private LockState state;
    private Animator animator;
    private LockState savedState;
    private float savedAnimatorTime;
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
        GameManager.LateLoadSceneAction += GraphicFix;
        GameManager.LateStartAction += GraphicFix;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OpeningCompelete()
    {
        gameObject.layer = LayerMask.NameToLayer("Void");
        state = LockState.Open;
        int mask = LayerMask.GetMask("Block");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1, mask, 0, 0);
        if (hit.collider && hit.collider.CompareTag("Lock"))
        {
            hit.collider.GetComponent<Lock>().GraphicFix();
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, 1, mask, 0, 0);
        if (hit.collider && hit.collider.CompareTag("Lock"))
        {
            hit.collider.GetComponent<Lock>().GraphicFix();
        }
        hit = Physics2D.Raycast(transform.position, Vector2.right, 1, mask, 0, 0);
        if (hit.collider && hit.collider.CompareTag("Lock"))
        {
            hit.collider.GetComponent<Lock>().GraphicFix();
        }
        hit = Physics2D.Raycast(transform.position, Vector2.left, 1, mask, 0, 0);
        if (hit.collider && hit.collider.CompareTag("Lock"))
        {
            hit.collider.GetComponent<Lock>().GraphicFix();
        }
    }
    public void GraphicFix()
    {
        gameObject.layer = LayerMask.NameToLayer("Void");
        int mask = LayerMask.GetMask("Block");
        bool top = Physics2D.Raycast(transform.position, Vector2.up, 1, mask, 0, 0).collider;
        bool down = Physics2D.Raycast(transform.position, Vector2.down, 1, mask, 0, 0).collider;
        bool right = Physics2D.Raycast(transform.position, Vector2.right, 1, mask, 0, 0).collider;
        bool left = Physics2D.Raycast(transform.position, Vector2.left, 1, mask, 0, 0).collider;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(!top);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(!down);
        transform.GetChild(0).GetChild(2).gameObject.SetActive(!right);
        transform.GetChild(0).GetChild(3).gameObject.SetActive(!left);
        transform.GetChild(2).GetChild(0).gameObject.SetActive(right && !down);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(left && !down);
        transform.GetChild(2).GetChild(2).gameObject.SetActive(down && !left);
        transform.GetChild(2).GetChild(3).gameObject.SetActive(top && !left);
        transform.GetChild(2).GetChild(4).gameObject.SetActive(down && !right);
        transform.GetChild(2).GetChild(5).gameObject.SetActive(top && !right);
        transform.GetChild(2).GetChild(6).gameObject.SetActive(left && !top);
        transform.GetChild(2).GetChild(7).gameObject.SetActive(right && !top);
        if (state != LockState.Open)
        {
            gameObject.layer = LayerMask.NameToLayer("Block");
        }
    }
    public void Open()
    {
        if (state == LockState.Close)
        {
            state = LockState.Opening;
            animator.Play("Opening");
        }
    }
    private void Save()
    {
        savedState = state;
        savedAnimatorTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    private void Load()
    {
        state = savedState;
        if (state == LockState.Close)
        {
            gameObject.layer = LayerMask.NameToLayer("Block");
            animator.Play("Close");
        }
        else if (state == LockState.Opening)
        {
            gameObject.layer = LayerMask.NameToLayer("Block");
            animator.Play("Opening",0,savedAnimatorTime);
        }
        else if (state == LockState.Open)
        {
            gameObject.layer = LayerMask.NameToLayer("Void");
            animator.Play("Open");
        }
    }
    public LockState State { get { return state; } }
    public KeyColor Color { get { return color; } }
}

public enum LockState { Close, Opening, Open }
