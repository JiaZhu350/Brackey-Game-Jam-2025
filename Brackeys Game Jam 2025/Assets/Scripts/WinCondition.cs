using UnityEngine;
using UnityEngine.InputSystem;

public class WinCondition : MonoBehaviour
{

    [SerializeField] private Player _player;
    private bool _canReadyToCashOut = false;
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && _canReadyToCashOut)
        {
            _player.PlayerWin();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canReadyToCashOut = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _canReadyToCashOut = false;
        }
    }
}
