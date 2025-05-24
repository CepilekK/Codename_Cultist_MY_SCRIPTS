using UnityEngine;
using UnityEngine.AI;

public class PhaseDash : IDash
{
    private DashSO data;
    private DashHandler handler;

    public PhaseDash(DashSO data, DashHandler handler)
    {
        this.data = data;
        this.handler = handler;
    }

    public void Execute(Transform user)
    {
        Vector3 target = GetDashTarget(user);
      //  Debug.Log("Dashing to: " + target);
        user.position = target;
    }

    private Vector3 GetDashTarget(Transform user)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            Vector3 direction = (hit.point - user.position).normalized;
            Vector3 desired = user.position + direction * data.maxDistance;

            Debug.DrawLine(user.position, desired, Color.cyan, 2f);

            if (data.canPhaseThroughWalls)
            {
                return desired;
            }

           
            if (NavMesh.SamplePosition(desired, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            {
                return navHit.position;
            }
        }

        return user.position;
    }



    public DashSO GetData() => data;
}
