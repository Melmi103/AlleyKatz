using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private int speed = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

   void MoveLeft()
    {
        
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }
    void MoveRight()
    {

        transform.Translate(Vector2.right * Time.deltaTime * speed);
    }
    void MoveForward()
    {
        
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }
    void MoveBackward()
    {

        transform.Translate(Vector2.down * Time.deltaTime * speed);
    }

    // Update is called once per frame
    void Update()
    {
      

        if (Input.GetKey(KeyCode.W)) MoveForward();
        if (Input.GetKey(KeyCode.S)) MoveBackward();
        if (Input.GetKey(KeyCode.A)) MoveLeft();
        if (Input.GetKey(KeyCode.D)) MoveRight();
    }
}

