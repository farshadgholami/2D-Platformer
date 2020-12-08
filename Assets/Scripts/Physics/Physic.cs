using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class Physic : MonoBehaviour
{
    public Action HitActionEffects;
    public Action HitAction;
    public event Action<List<RaycastHit2D>> OnCollision;

    [SerializeField]
    private float weight;

    private float friction;
    protected Vector2 force;
    protected Vector2 speed;
    protected Vector2 impactForce;
    protected Vector2 systemSpeed;
    protected Vector2 size;
    protected Vector2 distance;
    protected List<RaycastHit2D> hits = new List<RaycastHit2D>();
    protected List<ImpactProperty> impactProperties;
    protected List<GameObject> impacted_objects_;
    protected float collisionCheckDistance = 0.01f;
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
        impact.Initial();
        self_layer_mask_ = gameObject.layer;
        collider2d = GetComponent<BoxCollider2D>();
        size = transform.localScale * collider2d.size;
        layerMask += LayerMask.GetMask("Block" , "Enemy");
        CalculateRayCastPoints();
        impactProperties = new List<ImpactProperty>();
        impacted_objects_ = new List<GameObject>();
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
        impactProperties.Clear();
        impacted_objects_.Clear();
        distance = (force + speed) / weight * Time.deltaTime;
        Move(distance);
    }

    public void Move(Vector2 distance)
    {
        if (distance.x > 0)
            MovementCheckRight(distance.x);
        else if (distance.x < 0) MovementCheckLeft(-distance.x);
        if (distance.y > 0)
            MovementCheckUp(distance.y);
        else if (distance.y < 0) MovementCheckDown(-distance.y);
    }
    
    protected virtual void MovementCheckRight(float distance)
    {
        MoveToDirection(raycastPointsX, Vector2.right, distance, 0, layerMask);
    }
    protected virtual void MovementCheckLeft(float distance)
    {
        MoveToDirection(raycastPointsX, Vector2.left, distance, 0, layerMask);
    }
    protected virtual void MovementCheckUp(float distance)
    {
        MoveToDirection(raycastPointsY, Vector2.up, distance, 0, layerMask);
    }
    
    protected virtual void MovementCheckDown(float distance)
    {
        MoveToDirection(raycastPointsY, Vector2.down, distance, 0, layerMask);
    }

    protected virtual void MoveToDirection(Vector2[] raycastPoints, Vector2 direction, float distance, float threshold, int layerMask)
    {
        var nearestHitPoints = GetNearestHitPoints(raycastPoints, direction, distance, threshold, layerMask).ToList();

        UpdateImpactProperties(nearestHitPoints, direction);
        impact.AddRange(nearestHitPoints, direction);
        
        distance = nearestHitPoints.Count > 0 ? Mathf.Min(GetMinDistanceCollision(nearestHitPoints) - threshold, distance) : distance;
        ApplyMovement(direction * distance);
    }

    private float GetMinDistanceCollision(List<RaycastHit2D> hitObjects)
    {
        var minDistance = float.MaxValue;
        foreach (var hitObject in hitObjects) minDistance = Mathf.Min(minDistance, hitObject.distance);
        return minDistance;
    }
    
    private List<RaycastHit2D> GetNearestHitPoints(Vector2[] raycastPoints, Vector2 direction, float distance, float threshold, int layerMask)
    {
        hits.Clear();
        distance += threshold;
        foreach (var raycastPoint in raycastPoints)
        {
            hitPoint = Physics2D.Raycast((Vector2) transform.position + (raycastPoint + direction * threshold) * Mathf.Sign(direction.x + direction.y), direction,
                distance, layerMask, 0, 0);
            
            if (!IsRaycastHit(hitPoint, distance)) continue;
            
            distance = UpdateHits(hitPoint, distance);
        }
        return hits;
    }

    private float UpdateHits(RaycastHit2D hitPoint, float distance)
    {
        if (distance > hitPoint.distance)
        {
            distance = hitPoint.distance;
            hits.Clear();
        }
        hits.Add(hitPoint);
        return distance;
    }
    
    private bool IsRaycastHit(RaycastHit2D hitPoint, float distance)
    {
        return hitPoint.collider && !hitPoint.collider.Equals(collider2d) && hitPoint.distance <= distance;
    }
    
    protected virtual void ApplyMovement(Vector2 distance)
    {
        transform.position += (Vector3)distance;
    }

    protected virtual void CheckCollisionEnter()
    {
        if (!impact.Left) CheckImpact(raycastPointsX, Vector2.left, collisionCheckDistance, layerMask);
        if (!impact.Right) CheckImpact(raycastPointsX, Vector2.right, collisionCheckDistance, layerMask);
        if (!impact.Up) CheckImpact(raycastPointsY, Vector2.up, collisionCheckDistance, layerMask);
        if (!impact.Down) CheckImpact(raycastPointsY, Vector2.down, collisionCheckDistance, layerMask);
        
        NotifyOnCollision();
    }

    protected void NotifyOnCollision()
    {
        if (impact.Left) OnCollision?.Invoke(impact.LeftHits);
        if (impact.Right) OnCollision?.Invoke(impact.RightHits);
        if (impact.Up) OnCollision?.Invoke(impact.UpHits);
        if (impact.Down) OnCollision?.Invoke(impact.DownHits);
    }

    public List<RaycastHit2D> CheckImpact(Vector2[] raycastPoints, Vector2 direction, float distance, int layerMask)
    {
        hits.Clear();
        foreach (var raycastPoint in raycastPoints)
        {
            hitPoint = Physics2D.Raycast((Vector2) transform.position + raycastPoint * Mathf.Sign(direction.x + direction.y), direction,
                distance, layerMask, 0, 0);
            
            if (!IsRaycastHit(hitPoint, distance)) continue;
            if (hits.Contains(hitPoint)) continue;
            if (hits.Any(a => a.collider.Equals(hitPoint.collider))) continue;
            hits.Add(hitPoint);
        }
        
        UpdateImpactProperties(hits, direction);
        impact.AddRange(hits, direction);
        return hits;
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
    protected virtual void UpdateImpactProperties(List<RaycastHit2D> hits , Vector2 side)
    {
        foreach (RaycastHit2D hit in hits)
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
    public Vector2 Speed { get { return speed; }
        set => speed = value;
    }
    public Vector2 ImpactForce { get { return impactForce; } }
    public Vector2 Size { get { return size; } }
    public float Weight { get { return weight; } }
    public float Friction { get { return friction; } }
    public BoxCollider2D Collider2D { get { return collider2d; } }
    public List<ImpactProperty> ImpactProperties { get { return impactProperties; } }
    public int Layer { get { return layerMask; } set { layerMask = value; } }

    public Vector2 DeltaPosition => distance;

    public struct Impact
    {
        public bool Up => UpHits.Count > 0;
        public bool Down => DownHits.Count > 0;
        public bool Right => RightHits.Count > 0;
        public bool Left => LeftHits.Count > 0;
        
        public List<RaycastHit2D> UpHits;
        public List<RaycastHit2D> DownHits;
        public List<RaycastHit2D> RightHits;
        public List<RaycastHit2D> LeftHits;

        public void Initial()
        {
            UpHits = new List<RaycastHit2D>();
            DownHits = new List<RaycastHit2D>();
            RightHits = new List<RaycastHit2D>();
            LeftHits = new List<RaycastHit2D>();
        }

        public void AddRange(List<RaycastHit2D> hits, Vector2 direction)
        {
            GetHits(direction).AddRange(hits);
        }

        public List<RaycastHit2D> GetHits(Vector2 direction)
        {
            if (direction.Equals(Vector2.up))
                return UpHits;
            if (direction.Equals(Vector2.down))
                return DownHits;
            if (direction.Equals(Vector2.right))
                return RightHits;
            return LeftHits;
        }

        public void Reset()
        {
            UpHits.Clear();
            DownHits.Clear();
            RightHits.Clear();
            LeftHits.Clear();
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