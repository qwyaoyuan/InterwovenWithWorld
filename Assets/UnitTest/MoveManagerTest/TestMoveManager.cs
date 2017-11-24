using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveManager : MonoBehaviour
{

    CharacterController characterController;

    public Camera playerCamera;

    // Use this for initialization
    void Awake()
    {
        (new GameState()).Start();
        GameState.Instance.PlayerObj = gameObject;
        GameState.Instance.PlayerCamera = playerCamera;
        GameState.Instance.MoveSpeed = 3;
        GameState.Instance.CameraRotateSpeed = new Vector2(10, 10);
        GameState.Instance.CameraYAngleRange = new Vector2(20, 160);
        GameState.Instance.GameRunType = EnumGameRunType.Safe;
       
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(Vector3.down * Time.deltaTime * 10);
        float horizontal = Input.GetAxis("Horizontal");//水平
        float vertical = Input.GetAxis("Vertical");//垂直
        MoveManager.Instance.Move((new Vector2(horizontal, vertical)).normalized);
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        MoveManager.Instance.View(new Vector2(-mouseX, -mouseY));

        if (Input.GetKeyDown(KeyCode.D))
        {
            BuffState buffState = GameState.Instance.Xuanyun;
            buffState.Time = buffState.Time > 0 ? 0 : 1;
            GameState.Instance.Xuanyun = buffState;
        }
    }
}
