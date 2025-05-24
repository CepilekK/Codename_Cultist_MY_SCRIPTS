using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject skillTreeCamera;
    [SerializeField] private GameObject playerHUD;

    private bool isOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Do przerobienia potem
        {
            ToggleSkillTree();
        }
    }

    public void ToggleSkillTree()
    {
        isOpen = !isOpen;
        skillTreeCamera.SetActive(isOpen);
        playerHUD.SetActive(!isOpen); // ukryj UI gracza gdy w drzewku
        Time.timeScale = isOpen ? 0 : 1; // pauza gry 
    }
}
