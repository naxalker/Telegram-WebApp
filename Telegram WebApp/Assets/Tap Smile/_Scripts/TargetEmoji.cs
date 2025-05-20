using UnityEngine;

public class TargetEmoji : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.AddScore(1);
            GameManager.Instance.MoveTarget();
        }
    }
}
