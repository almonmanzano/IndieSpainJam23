using UnityEngine;

public class ConfineCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
