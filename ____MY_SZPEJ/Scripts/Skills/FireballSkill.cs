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

        Vector3 mouseWorldPos = GetMouseWorldPosition(playerTransform);
        Vector3 direction = mouseWorldPos - playerTransform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            Debug.LogWarning("Fireball: nieprawidłowy kierunek (zero)!");
            return;
        }

        int baseSkillDamage = PlayerStatsManager.Instance != null
            ? PlayerStatsManager.Instance.SkillBaseDamage
            : 10;

        float skillMultiplier = PlayerStatsManager.Instance != null
            ? PlayerStatsManager.Instance.SkillBaseDamageMultiplier
            : 1f;

        float rawDamage = (baseSkillDamage + data.skillBaseDamage) * skillMultiplier;
        int totalDamage = Mathf.RoundToInt(rawDamage * (data.damageScalePercent / 100f));


        GameObject go = GameObject.Instantiate(
            data.ProjectilePrefab,
            playerTransform.position + Vector3.up * 1f,
            Quaternion.LookRotation(direction)
        );

        Projectile fireball = go.GetComponent<Projectile>();
        fireball.Init(direction, 10f, 3f, totalDamage, AttackSource.Player);
    }


    public float GetCooldown() => data.cooldown;
    public SkillSO GetData() => data;
    public int GetResourceCost() => data.resourceCost;

    private Vector3 GetMouseWorldPosition(Transform playerTransform)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        Debug.LogWarning("Fireball: nie znaleziono punktu na warstwie Ground. Używam fallback.");
        return playerTransform.position + playerTransform.forward * 5f;
    }
}
