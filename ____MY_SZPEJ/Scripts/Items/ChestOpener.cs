using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChestOpener : MonoBehaviour
{
    [Header("Drop ustawienia")]
    [SerializeField] private int minDropAmount = 3;
    [SerializeField] private int maxDropAmount = 6;

    [Header("Wymagana odleg³oœæ od gracza")]
    [SerializeField] private float interactionRange = 3f;

    [Header("Prefab efektu otwarcia (opcjonalnie)")]
    [SerializeField] private GameObject openEffectPrefab;

    private bool isOpened = false;

    private void Update()
    {
        if (isOpened) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsPlayerLookingAtMe(out GameObject player))
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= interactionRange)
                {
                    OpenChest(player.transform.position);
                }
                else
                {
                    Debug.Log("Za daleko od skrzyni!");
                }
            }
        }
    }

    private bool IsPlayerLookingAtMe(out GameObject player)
    {
        player = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                return true;
            }
        }

        return false;
    }

    private void OpenChest(Vector3 playerPosition)
    {
        isOpened = true;

        if (ItemDropManager.Instance != null)
        {
            ItemDropManager.Instance.DropItems(transform.position, minDropAmount, maxDropAmount);
        }
        else
        {
            Debug.LogWarning("ItemDropManager.Instance nie istnieje!");
        }

        if (openEffectPrefab != null)
        {
            Instantiate(openEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
#endif

}
