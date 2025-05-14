using UnityEngine;

public class ChainHookAttack : IAutoAttack
{
    private AutoAttackSO data;
    private AutoAttackHandler handler;

    public ChainHookAttack(AutoAttackSO data, AutoAttackHandler handler)
    {
        this.data = data;
        this.handler = handler;
    }

    public void Execute(Transform user)
    {
        Debug.Log($"[ChainHook] Gracz wystrzeliwuje hak za {handler.GetCalculatedDamage()} obrażeń.");

        if (data.projectilePrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                Vector3 direction = (hit.point - user.position).normalized;
                direction.y = 0f;

                Vector3 spawnPos = user.position + direction * 1f + Vector3.up * 1f;
                Quaternion rot = Quaternion.LookRotation(direction);

                GameObject hookObj = GameObject.Instantiate(data.projectilePrefab, spawnPos, rot);
                HookProjectile hook = hookObj.GetComponent<HookProjectile>();
                if (hook != null)
                {
                    hook.Init(direction, data.projectileSpeed, data.projectileDuration, handler.GetCalculatedDamage(), user, data.pullSpeed);
                }
            }
        }
        else
        {
            Debug.LogWarning("[ChainHook] Brak przypiętego prefab'u haka!");
        }
    }

    public AutoAttackSO GetData() => data;
}
