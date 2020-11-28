using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class Toolkit : MonoBehaviour
{
    ///  Vector 2 Transpose
    public static bool CheckMove(Vector2 originPosition, Vector2 vecSize, Vector2 direction, float distance, int layerNumber, Collider2D colider, out List<RaycastHit2D> hitPoints)
    {
        float leastDistance = Mathf.Infinity;

        float threshold = 0.01f;
        bool hit = false;
        hitPoints = new List<RaycastHit2D>();
        Vector2 ceilingSize = new Vector2(vecSize.x / Mathf.Ceil(vecSize.x), vecSize.y / Mathf.Ceil(vecSize.y));
        Vector2 rayOrigin = originPosition + vecSize * (direction.x + direction.y) / 2;
        Vector2 multiplier = Transpose2(direction) * ceilingSize;
        float loopSize = Mathf.Abs(direction.x) * Mathf.Ceil(vecSize.y) + Mathf.Abs(direction.y) * Mathf.Ceil(vecSize.x);
        for (int i = 0; i <= loopSize; i++)
        {
            float k = 0;
            // first point threshold
            if (i == 0)
                k = threshold;
            // last point threshold
            if (i == loopSize)
                k = -threshold;
            RaycastHit2D[] points = Physics2D.RaycastAll(rayOrigin - multiplier * (i + k), direction, distance, layerNumber, 0, 0);
            foreach (RaycastHit2D hitPoint in points)
            {
                if (hitPoint.collider != null && !hitPoint.collider.Equals(colider) && hitPoint.distance <= leastDistance)
                {
                    hit = true;
                    leastDistance = hitPoint.distance;
                    hitPoints.Add(hitPoint);
                }
            }
        }
        hitPoints.RemoveAll(delegate (RaycastHit2D ray)
        {
            return ray.distance > leastDistance;
        });

        return hit;
    }
    public static Vector2 Transpose2(Vector2 vec)
    {
        return new Vector2(vec.y, vec.x);
    }
    public static Vector2 HitSide(RaycastHit2D hit)
    {
        Vector2 side = Vector2.zero;
        Vector2 direction = hit.point - (Vector2)hit.transform.position;
        float angle = Vector2.SignedAngle(direction, Vector2.up);
        if (angle >= -45 && angle <= 45)
        {
            side = Vector2.up;
        }
        else if (angle > 45 && angle <= 135)
        {
            side = Vector2.right;
        }
        else if ((angle > 135 && angle <= 180) || (angle >= -180 && angle <= -135))
        {
            side = Vector2.down;
        }
        else if (angle > -135 && angle < -45)
        {
            side = Vector2.left;
        }
        return side;
    }
    public static bool HitUpCheck(RaycastHit2D hit)
    {
        Vector2 size = hit.collider.gameObject.GetComponent<BoxCollider2D>().size * hit.transform.localScale;
        if ((hit.point - ((Vector2)hit.transform.position + (size * 0.5f))) * Vector2.up == Vector2.zero)
        {
            return true;
        }
        return false;
    }
    public static string VectorSerialize(Vector2 vector)
    {
        return vector.x + "," + vector.y;
    }
    public static Vector2 DeserializeVector(string s)
    {
        string[] parts = s.Split(',');
        return new Vector2(float.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat));
    }
    public static bool IsVisible(Vector2 origin, Vector2 destenition, int layerMask, Collider2D colider)
    {
        Vector2 directiohn = destenition - origin;
        RaycastHit2D hit = Physics2D.Raycast(origin, directiohn.normalized, directiohn.magnitude, layerMask, 0, 0);
        if (hit.collider && colider)
        {
            if (hit.collider.Equals(colider))
                return true;
            else
                return false;

        }
        return false;
    }
    public static int SideToNumber(Vector2 side)
    {
        if (side == Vector2.up)
            return 0;
        else if (side == Vector2.right)
            return 1;
        else if (side == Vector2.down)
            return 2;
        else if (side == Vector2.left)
            return 3;
        else
            Debug.Log("Something is Wrong");

        return -1;
    }
    public static Vector2 NumberToSide(int number)
    {
        switch (number)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.down;
            case 3:
                return Vector2.left;
            default:
                Debug.Log("Somthing is Wrong");
                return Vector2.zero;
        }
    }
    public static Vector2 BoolVector(bool x, bool y)
    {
        Vector2 boolVector = Vector2.zero;
        if (x)
        {
            boolVector.x = 1;
        }
        if (y)
        {
            boolVector.y = 1;
        }
        return boolVector;
    }
    public static Vector2 Side(Vector2 direction)
    {
        if (Mathf.Abs(direction.y) >= Mathf.Abs(direction.x))
            return (direction * Vector2.up).normalized;
        else
            return (direction * Vector2.right).normalized;
    }
    public static Vector2 Vector2Abs(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }
    public static Vector2 SideToVector(Side side)
    {
        switch (side)
        {
            case global::Side.Right:
                return Vector2.right;
            case global::Side.Left:
                return Vector2.left;
            case global::Side.Up:
                return Vector2.up;
            case global::Side.Down:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }
    public static Side VectorToSide(Vector2 vector)
    {
        if (vector == Vector2.right)
            return global::Side.Right;
        else if (vector == Vector2.left)
            return global::Side.Left;
        else if (vector == Vector2.up)
            return global::Side.Up;
        else
            return global::Side.Down;
    }
}

public enum Side { Up, Right, Down, Left }
public class HitDistanceCompare : IComparer<RaycastHit2D>
{
    public int Compare(RaycastHit2D x, RaycastHit2D y)
    {
        if (x.distance == y.distance)
            return 0;
        if (x.distance < y.distance)
            return -1;
        else return 1;
    }
}
