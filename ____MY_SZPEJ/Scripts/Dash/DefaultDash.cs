using UnityEngine;

public class DefaultDash : IDash
{
    private DashSO data;
    private DashHandler handler;

    public DefaultDash(DashSO data, DashHandler handler)
    {
        this.data = data;
        this.handler = handler;
    }

    public void Execute(Transform user)
    {
        Debug.Log($"Dash: {data.dashName}");

        Vector3 targetPoint = GetDashTarget(user);

        if (data.canPhaseThroughWalls)
        {
            
            user.position = targetPoint;
        }
        else
        {
            
            user.GetComponent<Rigidbody>()?.MovePosition(targetPoint);
        }
    }

    private Vector3 GetDashTarget(Transform user)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Vector3 dir = (hit.point - user.position).normalized;
            Vector3 finalPos = user.position + dir * data.maxDistance;

            if (!data.canPhaseThroughWalls)
            {
               
                if (Physics.Raycast(user.position, dir, out RaycastHit wallHit, data.maxDistance))
                {
                    finalPos = wallHit.point;
                }
            }

            finalPos.y = user.position.y; 
            return finalPos;
        }

        return user.position;
    }

    public DashSO GetData() => data;
}
