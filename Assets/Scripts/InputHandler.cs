using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }
     
    private void CatchMoving()
    {
        var horizontalMoving = Input.GetAxis("Horizontal");
        var vecticalMoving = Input.GetAxis("Vertical");

        InputVector = new Vector2(horizontalMoving, vecticalMoving);
    }

    private void Update() => CatchMoving();
}
