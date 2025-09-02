using UnityEngine;

public class LocalPlayerController : MonoBehaviour
{
    [SerializeField] Transform baseTransform;
    [SerializeField] InputManager inputs;
    Vector2 dir;
    private void Update()
    {
        dir = inputs.inputActions.Player.Move.ReadValue<Vector2>();
        baseTransform.position += rotateCharMove(dir.y,dir.x) * 5f * Time.deltaTime;
    }

    public Vector3 rotateCharMove(float inputY, float inputX)
    {
        Vector3 moveVectorY;
        // y portion of the code
        float theta = Camera.main.transform.localEulerAngles.y;
        float Base = 2 * (Mathf.Sin(theta / 2) * inputY);
        float xDifference = Mathf.Cos(theta / 2) * Base;
        float yDifference = Mathf.Sin(theta / 2) * Base;
        moveVectorY = new Vector3(xDifference, 0, inputY - yDifference);


        Vector3 moveVectorX;

        // x portion of the code
        theta = Camera.main.transform.localEulerAngles.z;
        Base = 2 * ((Mathf.Sin((theta / 2) + (Mathf.PI / 2))) * inputX);
        yDifference = (Mathf.Sin(theta / 2) * Base);
        xDifference = Mathf.Cos(theta / 2) * Base;
        moveVectorX = new Vector3((inputX - xDifference), 0, yDifference);

        Vector3 finalVector = -moveVectorX + moveVectorY;
        return finalVector;
    }
}
