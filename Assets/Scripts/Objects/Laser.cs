using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private bool deactive;
    [SerializeField]
    private bool periodic;
    [SerializeField]
    private float activeDuration;
    [SerializeField]
    private float deactiveDuration;
    [SerializeField]
    private float startDelay;
    private bool ready;
    private int layerMask;
    private LineRenderer line;
    private float timer;
    private bool started;

    // Use this for initialization

    void Start()
    {
        line = GetComponent<LineRenderer>();
        layerMask = LayerMask.GetMask("Player", "Block", "Enemy");
    }
    private void Initialize()
    {
        StartCoroutine(StartDelay());
    }
    // Update is called once per frame
    void Update()
    {
        if (started && periodic)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (ready)
                    timer += deactiveDuration;
                else
                    timer += activeDuration;
                ready = !ready;
            }
        }
        if (startDelay > 0)
        {
            startDelay -= Time.deltaTime;
        }
        else if(!started)
        {
            timer = startDelay + activeDuration;
            started = true;
            ready = true;
        }
        line.positionCount = 0;
        if (!deactive && ready)
            hit();
    }

    private void Lightning(RaycastHit2D hit)
    {
        line.startWidth = line.endWidth = Random.Range(0.05f, 0.2f);
        line.positionCount = (int)Mathf.Ceil(hit.distance) + 1;
        line.SetPosition(0, transform.position);
        for (int i = 1; i < line.positionCount - 1; i++)
        {
            Vector2 pos = transform.position + (transform.up * i) + (transform.right * Random.Range(-0.5f, 0.5f));
            line.SetPosition(i, pos);
        }
        line.SetPosition(line.positionCount - 1, hit.point);
    }
    private void hit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1000, layerMask, 0, 0);
        if (hit.collider == null)
            return;
        if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<CharacterStats>().Health = 0;
        }
        Lightning(hit);
    }
    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        ready = true;
        if (periodic)
            StartCoroutine(Cycle());
    }
    private IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(activeDuration);
            ready = false;
            yield return new WaitForSecondsRealtime(deactiveDuration);
            ready = true;
        }
    }
    private void Activate(bool active)
    {
        deactive = !active;
    }
}
