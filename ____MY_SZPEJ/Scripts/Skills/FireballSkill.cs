using UnityEngine;

public class FireballSkill : ISkill
{
    private SkillSO data;

    public FireballSkill(SkillSO skillData)
    {
        data = skillData;
    }

    public void Activate(Transform playerTransform)
    {
        if (data.ProjectilePrefab == null)
        {
            Debug.LogWarning("Brak przypisanego prefab skilla!");
            return;
        }

        UnitSO_Container unitContainer = playerTransform.GetComponent<UnitSO_Container>();
        if (unitContainer == null)
        {
            Debug.LogWarning("Brak UnitSO_Container na obiekcie gracza!");
            return;
        }

        UnitSO playerUnit = unitContainer.GetUnitSO();
        int damage = playerUnit != null ? playerUnit.baseDamage : 10;

        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3 direction = mouseWorldPos - playerTransform.position;
        direction.y = 0f;

        GameObject go = GameObject.Instantiate(data.ProjectilePrefab, playerTransform.position + Vector3.up * 1f, Quaternion.identity);
        Projectile fireball = go.GetComponent<Projectile>();
        fireball.Init(direction, 10f, 3f, damage, AttackSource.Player);
    }

    public float GetCooldown() => data.cooldown;
    public SkillSO GetData() => data;
    public int GetResourceCost() => data.resourceCost;

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}

