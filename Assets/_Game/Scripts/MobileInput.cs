using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static bool moveLeft;
    public static bool moveRight;
    public static bool jumpPressed;
    public static bool attackPressed;
    public static bool parryPressed;

    public void LeftDown() { moveLeft = true; }
    public void LeftUp() { moveLeft = false; }

    public void RightDown() { moveRight = true; }
    public void RightUp() { moveRight = false; }

    public void Jump() { jumpPressed = true; }

    public void Attack() { attackPressed = true; }

    public void Parry() { parryPressed = true; }
}