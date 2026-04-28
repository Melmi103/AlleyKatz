using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private bool isHoldingShift;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

   void MoveLeft()
    {
        if (isHoldingShift) transform.Translate(Vector2.left * Time.deltaTime * 8);
        else
        transform.Translate(Vector2.left * Time.deltaTime * 4);
    }
    void MoveRight()
    {
        if (isHoldingShift) transform.Translate(Vector2.right * Time.deltaTime * 8);
        else
        transform.Translate(Vector2.right * Time.deltaTime * 4);
    }
    void MoveForward()
    {
        if (isHoldingShift) transform.Translate(Vector2.up * Time.deltaTime * 8);
        else
        transform.Translate(Vector2.up * Time.deltaTime * 4);
    }
    void MoveBackward()
    {
        if (isHoldingShift) transform.Translate(Vector2.down * Time.deltaTime * 8);
        else
        transform.Translate(Vector2.down * Time.deltaTime * 4);
    }

    // Update is called once per frame
    void Update()
    {
        isHoldingShift = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKey(KeyCode.W)) MoveForward();
        if (Input.GetKey(KeyCode.S)) MoveBackward();
        if (Input.GetKey(KeyCode.A)) MoveLeft();
        if (Input.GetKey(KeyCode.D)) MoveRight();
    }
}

