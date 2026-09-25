using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private int speed = 6;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isMoving = false;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

   void MoveLeft()
    {
        
        transform.Translate(Vector2.left * Time.deltaTime * speed);
        isMoving = true;
    }
    void MoveRight()
    {

        transform.Translate(Vector2.right * Time.deltaTime * speed);
        isMoving = true;
    }
    void MoveForward()
    {
        
        transform.Translate(Vector2.up * Time.deltaTime * speed);
        isMoving = true;
    }
    void MoveBackward()
    {

        transform.Translate(Vector2.down * Time.deltaTime * speed);
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mouseScreen;
        if (Mouse.current != null)
        {
            mouseScreen = Mouse.current.position.ReadValue();
        }
        else
        {
            mouseScreen = Input.mousePosition;
        }

        if (mouseScreen.x < Screen.width / 2)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }



        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isMoving = true;
          
        }
        else
        {
            isMoving = false;
        }
        if (Input.GetKey(KeyCode.W)) MoveForward();
        if (Input.GetKey(KeyCode.S)) MoveBackward();
        if (Input.GetKey(KeyCode.A)) MoveLeft();
        if (Input.GetKey(KeyCode.D)) MoveRight();

        if (isMoving == true)
        {
            animator.SetBool("IsRunning", true);
        }

        else
        {
            animator.SetBool("IsRunning", false);
        }
 
    }
}

