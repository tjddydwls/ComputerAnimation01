using UnityEngine;

public class Test : MonoBehaviour
{
    float moveSpeed = 10f;
    float jumpForce = 20.0f;
    float gravity = 9.8f;
    float groundY = 0.0f;
    bool isJumping = false;
    bool isFallingDown = false;
    float verticalVelocity = 0.0f;
    bool groundYSet = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Position: " + transform.position);
        Debug.Log("Rotation (Quaternion): " + transform.rotation);
        Debug.Log("Euler Angles: " + transform.eulerAngles);
        Debug.Log("Forward: " + transform.forward);
        Debug.Log("Up: " + transform.up);
        Debug.Log("Right: " + transform.right);
    }

    // Update is called once per frame
    void Update()
    {
        if (!groundYSet)
        {
            groundY = transform.position.y;
            groundYSet = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            verticalVelocity = jumpForce;
        }
    
        if (isJumping || isFallingDown)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y += verticalVelocity * Time.deltaTime;

            if (isJumping)
            {
                if (pos.y >= groundY +5.0f)
                {
                    pos.y = groundY +5.0f;
                    verticalVelocity = 0.0f;
                }
                
                if (pos.y <= groundY)
                {
                    pos.y = groundY;
                    isJumping = false;
                    verticalVelocity = 0.0f;
                }
            }
            transform.position = pos;
        }

        // 오른쪽 화살표 입력 처리
        if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x < 2f)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }

        // 왼쪽 화살표 입력 처리
        if (Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x > -2f)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }

        // // R키 입력 처리
        // if (Input.GetKey(KeyCode.R))
        // {
        //     transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        // }
    }
}
