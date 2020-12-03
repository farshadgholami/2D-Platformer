using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Physic : MonoBehaviour
{
    public Action HitActionEffects;
    public Action HitAction;
    public event Action<Collider2D> OnCollision;

    [SerializeField]
    private float weight;

    private float collisionCheckDistance = 0.01f;
    private float friction;
    protected Vector2 force;
    protected Vector2 speed;
    protected Vector2 impactForce;
    protected Vector2 systemSpeed;
    protected Vector2 size;
    protected Vector2 distance;
    protected List<RaycastHit2D> h_raycast_list_;
    protected List<RaycastHit2D> v_raycast_list_;
    protected List<ImpactProperty> impactProperties;
    protected List<GameObject> impacted_objects_;


    protected Vector2[] raycastPointsX;
    protected Vector2[] raycastPointsY;
    protected BoxCollider2D collider2d;

    protected int layerMask;
    protected Impact impact;
    protected int self_layer_mask_;
    protected RaycastHit2D hitPoint;

    //Save
    protected Vector2 savedPostion;
    protected Vector2 savedForce;

    private void Awake()
    {
        GameManager.SaveSceneAction += Save;
        GameManager.LoadSceneAction += Load;
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Function();
    }
    protected virtual void Init()
    {
        self_layer_mask_ = gameObject.layer;
        collider2d = GetComponent<BoxCollider2D>();
        size = transform.localScale * collider2d.size;
        layerMask += LayerMask.GetMask("Block" , "Enemy");
        CalculateRayCastPoints();
        impactProperties = new List<ImpactProperty>();
        impacted_objects_ = new List<GameObject>();
        v_raycast_list_ = new List<RaycastHit2D>();
        h_raycast_list_ = new List<RaycastHit2D>();
    }
    protected virtual void Function()
    {
        gameObject.layer = LayerMask.NameToLayer("Void");
        CapGravitySpeed();
        CalculateMovment();
        CheckCollisionEnter();
        CalculateHit();
        HitActionFunction();
        ResetCalculate();
        gameObject.layer = self_layer_mask_;
    }
    protected void CalculateRayCastPoints()
    {
        // calculate horizontal raycast point
        float cut = GameManager.MinSize;
        raycastPointsX = new Vector2[Mathf.CeilToInt(size.y / cut) + 1];
        raycastPointsX[0] = new Vector2(size.x / 2 , (-size.y / 2) + 0.01f);
        raycastPointsX[raycastPointsX.Length - 1] = new Vector2(size.x / 2 , (size.y / 2) - 0.01f);
        for (int i = 1 ; i < raycastPointsX.Length - 1 ; i++)
        {
            raycastPointsX[i] = raycastPointsX[i - 1] + Vector2.up * cut;
        }
        // calculate vertical raycast point
        raycastPointsY = new Vector2[Mathf.CeilToInt(size.x / cut) + 1];
        raycastPointsY[0] = new Vector2((-size.x / 2) + 0.01f , size.y / 2);
        raycastPointsY[raycastPointsY.Length - 1] = new Vector2(size.x / 2 - 0.01f , size.y / 2);
        for (int i = 1 ; i < raycastPointsY.Length - 1 ; i++)
        {
            raycastPointsY[i] = raycastPointsY[i - 1] + Vector2.right * cut;
        }
    }
    //Protected Functions
    protected virtual void CalculateMovment()
    {
        h_raycast_list_.Clear();
        v_raycast_list_.Clear();
        impactProperties.Clear();
        impacted_objects_.Clear();
        distance = ((force + speed) / weight) * Time.deltaTime;
        if (distance.x > 0)
        {
            MovementCheckRight();
        }
        else if (distance.x < 0)
        {
            MovementCheckLeft();
        }
        if (distance.y > 0)
        {
            MovementCheckUp();
        }
        else if (distance.y < 0)
        {
            MovementCheckDown();
        }
    }
    protected virtual void MovementCheckRight()
    {
        float leastDistance = distance.x;
        for (int i = 0 ; i < raycastPointsX.Length ; i++)
        {
            hitPoint = Physics2D.Raycast((Vector2)transform.position + raycastPointsX[i] , Vector2.right , leastDistance , layerMask , 0 , 0);
            if (hitPoint.collider != null && hitPoint.distance <= leastDistance)
            {
                impact.RightCollider = hitPoint.collider;
                leastDistance = hitPoint.distance;
                h_raycast_list_.Add(hitPoint);
            }
        }
        h_raycast_list_.RemoveAll(delegate (RaycastHit2D ray)
        {
            return ray.distance > leastDistance;
        });
        UpdateImpactProperties(h_raycast_list_ , Vector2.right);
        ApplyMovement(Vector2.right * leastDistance);
    }
    protected virtual void MovementCheckLeft()
    {
        float leastDistance = -distance.x;
        for (int i = 0 ; i < raycastPointsX.Length ; i++)
        {
            hitPoint = Physics2D.Raycast((Vector2)transform.position - raycastPointsX[i] , Vector2.left , leastDistance , layerMask , 0 , 0);
            if (hitPoint.collider != null && !hitPoint.collider.Equals(collider2d) && hitPoint.distance <= leastDistance)
            {
                impact.LeftCollider = hitPoint.collider;
                leastDistance = hitPoint.distance;
                h_raycast_list_.Add(hitPoint);
            }
        }
        h_raycast_list_.RemoveAll(delegate (RaycastHit2D ray)
        {
            return ray.distance > leastDistance;
        });
        UpdateImpactProperties(h_raycast_list_ , Vector2.left);
        ApplyMovement(Vector2.left * leastDistance);
    }
    protected virtual void MovementCheckUp()
    {
        float leastDistance = distance.y;
        for (int i = 0 ; i < raycastPointsY.Length ; i++)
        {
            hitPoint = Physics2D.Raycast((Vector2)transform.position + raycastPointsY[i] , Vector2.up , leastDistance , layerMask , 0 , 0);
            if (hitPoint.collider != null && !hitPoint.collider.Equals(collider2d) && hitPoint.distance <= leastDistance)
            {
                impact.UpCollider = hitPoint.collider;
                leastDistance = hitPoint.distance;
                v_raycast_list_.Add(hitPoint);
            }
        }
        v_raycast_list_.RemoveAll(delegate (RaycastHit2D ray)
        {
            return ray.distance > leastDistance;
        });
        UpdateImpactProperties(v_raycast_list_ , Vector2.up);
        ApplyMovement(Vector2.up * leastDistance);
    }
    protected virtual void MovementCheckDown()
    {
        float leastDistance = -distance.y;
        for (int i = 0 ; i < raycastPointsY.Length ; i++)
        {
            RaycastHit2D[] points = Physics2D.RaycastAll((Vector2)transform.position - raycastPointsY[i] , Vector2.down , leastDistance , layerMask , 0 , 0);
            foreach (RaycastHit2D hitPoint in points)
            {
                if (hitPoint.collider != null && !hitPoint.collider.Equals(collider2d) && hitPoint.distance <= leastDistance)
                {
                    impact.DownCollider = hitPoint.collider;
                    leastDistance = hitPoint.distance;
                    v_raycast_list_.Add(hitPoint);
                }
            }
        }
        v_raycast_list_.RemoveAll(delegate (RaycastHit2D ray)
        {
            return ray.distance > leastDistance;
        });
        UpdateImpactProperties(v_raycast_list_ , Vector2.down);
        ApplyMovement(Vector2.down * leastDistance);
    }
    protected virtual void ApplyMovement(Vector2 distance)
    {
        transform.position += (Vector3)distance;
    }

    protected void CheckCollisionEnter()
    {
        if (!impact.Left) impact.LeftCollider = CheckImpact(raycastPointsX, Vector2.left, collisionCheckDistance, layerMask);
        if (!impact.Right) impact.RightCollider = CheckImpact(raycastPointsX, Vector2.right, collisionCheckDistance, layerMask);
        if (!impact.Up) impact.UpCollider = CheckImpact(raycastPointsY, Vector2.up, collisionCheckDistance, layerMask);
        if (!impact.Down) impact.DownCollider = CheckImpact(raycastPointsY, Vector2.down, collisionCheckDistance, layerMask);
        
        NotifyOnCollision();
    }

    private void NotifyOnCollision()
    {
        if (impact.Left) OnCollision?.Invoke(impact.LeftCollider);
        if (impact.Right) OnCollision?.Invoke(impact.RightCollider);
        if (impact.Up) OnCollision?.Invoke(impact.UpCollider);
        if (impact.Down) OnCollision?.Invoke(impact.DownCollider);
    }
    
    private Collider2D CheckImpact(Vector2[] raycastPoints, Vector2 direction, float distance, int layerMask)
    {
        List<RaycastHit2D> raycastHitList = null;
        foreach (var raycastPoint in raycastPoints)
        {
            hitPoint = Physics2D.Raycast((Vector2) transform.position + raycastPoint * Mathf.Sign(direction.x + direction.y), direction,
                distance, layerMask, 0, 0);
            
            if (!IsRaycastHit(hitPoint, distance)) continue;
            
            if (raycastHitList == null) raycastHitList = new List<RaycastHit2D>();
            raycastHitList.Add(hitPoint);
            distance = hitPoint.distance;
        }

        if (raycastHitList == null) return null;
        
        raycastHitList.RemoveAll(delegate (RaycastHit2D ray) { return ray.distance > distance; });
        UpdateImpactProperties(raycastHitList, direction);
        return raycastHitList[0].collider;
    }
    
    private bool IsRaycastHit(RaycastHit2D hitPoint, float distance)
    {
        return hitPoint.collider != null && !hitPoint.collider.Equals(collider2d) && hitPoint.distance <= distance;
    }
    
    protected virtual void CalculateHit()
    {
        if (impact.Right || impact.Left)
            force.x = 0;
        if (impact.Up || impact.Down)
            force.y = 0;
    }
    protected virtual void ResetCalculate()
    {
        speed = Vector2.zero;
        impactForce = Vector2.zero;
        impact.Reset();
        systemSpeed = Vector2.zero;
    }
    protected virtual void UpdateImpactProperties(List<RaycastHit2D> raycastList , Vector2 side)
    {
        foreach (RaycastHit2D hit in raycastList)
        {
            var impact_object = hit.collider.gameObject;
            if (!impacted_objects_.Contains(impact_object))
            {
                var impactEffects = impact_object.GetComponents<ImpactEffect>();
                foreach (ImpactEffect effect in impactEffects)
                {
                    impactProperties.Add(new ImpactProperty(effect , side));
                }
                impacted_objects_.Add(impact_object);
            }
        }
    }
    protected void CapGravitySpeed()
    {
        if (force.y < 0)
        {
            force.y = Mathf.Max(force.y , -(GameManager.instance.pMaxGravitySpeed) * weight);
        }
    }
    protected virtual void HitActionFunction()
    {
        if (impact.HasImpact())
        {
            HitAction?.Invoke();
        }
        if (impactProperties.Count > 0)
        {
            HitActionEffects?.Invoke();
        }
    }
    protected virtual void Save()
    {
        savedPostion = transform.position;
        savedForce = force;
    }
    protected virtual void Load()
    {
        ResetForce();
        transform.position = savedPostion;
        force = savedForce;
    }
    //Public Functions
    public virtual void AddForce(Vector2 force)
    {
        this.force += force;
    }
    public virtual void AddSpeed(Vector2 speed)
    {
        this.speed += speed;
    }
    public virtual void AddImpactForce(Vector2 impactForce)
    {
        this.impactForce += impactForce;
    }
    public void ResetForce()
    {
        force = Vector2.zero;
    }

    public bool HasImpact(Vector2 direction)
    {
        return impact.HasImpact(direction);
    }

    //Public Get Attributes

    public Impact ImpactSide => impact;
    public Vector2 Force { get { return force; } }
    public Vector2 Speed { get { return speed; } }
    public Vector2 ImpactForce { get { return impactForce; } }
    public Vector2 Size { get { return size; } }
    public float Weight { get { return weight; } }
    public float Friction { get { return friction; } }
    public BoxCollider2D Collider2D { get { return collider2d; } }
    public List<ImpactProperty> ImpactProperties { get { return impactProperties; } }
    public int Layer { get { return layerMask; } set { layerMask = value; } }

    public struct Impact
    {
        public bool Up => UpCollider;
        public bool Down => DownCollider;
        public bool Right => RightCollider;
        public bool Left => LeftCollider;
        
        public Collider2D UpCollider;
        public Collider2D DownCollider;
        public Collider2D RightCollider;
        public Collider2D LeftCollider;

        public void Reset()
        {
            UpCollider = null;
            DownCollider = null;
            RightCollider = null;
            LeftCollider = null;
        }

        public bool HasImpact()
        {
            return Up || Down || Right || Left;
        }

        public bool HasImpact(Vector2 direction)
        {
            if (direction.Equals(Vector2.left) && Left) return true;
            if (direction.Equals(Vector2.right) && Right) return true;
            if (direction.Equals(Vector2.up) && Up) return true;
            if (direction.Equals(Vector2.down) && Down) return true;
            return false;
        }
    }
}



