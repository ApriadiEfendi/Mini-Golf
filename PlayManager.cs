using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.Events;

public class PlayManager : MonoBehaviour
{
    [SerializeField] BallController ballController;
    [SerializeField] CameraController camController;
    [SerializeField] GameObject finishWindow;
    [SerializeField] TMP_Text finishText;
    [SerializeField] TMP_Text shootCounttext;
    [SerializeField] UnityEvent sfxBolaMasuk;
    bool isBallOutSide;
    bool isBallTeleporting;
    bool isGoal;
    Vector3 lastBallPosition;

    private void OnEnable() {
        ballController.onBallShooted.AddListener(UpdateShootCount);
    }

    private void OnDisable() {
        ballController.onBallShooted.RemoveListener(UpdateShootCount);
    }

    private void Update() 
    {
        // Debug.Log(
        //     ballController.ShootingMode.ToString() + " " +
        //     ballController.IsMove() + " " +
        //     isBallOutSide + " " +
        //     ballController.enabled + " " +
        //     isBallTeleporting + " " +
        //     isGoal
        //     );

        if(ballController.ShootingMode)
        {
            lastBallPosition = ballController.transform.position;
        }
        var inputActive = Input.GetMouseButton(0)
            && ballController.IsMove() == false
            && ballController.ShootingMode == false
            && isBallOutSide == false;
            
        camController.SetInputActive(inputActive);
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        sfxBolaMasuk.Invoke();
        ballController.enabled = false;
        //TO DO player win window pop up
        finishWindow.gameObject.SetActive(true);
        finishText.text = "Finished!\n" + "Shoot Count: " + ballController.ShootCount;
    }
    public void OnBallOutside()
    {
        if(isGoal)
            return;

        if(isBallTeleporting == false)
            Invoke("TeleportBallLastPosition", 3);
        
        ballController.enabled = false;
        isBallOutSide = true;
        isBallTeleporting = true;
    }

    public void TeleportBallLastPosition()
    {
        TeleportBall(lastBallPosition);
    }

    public void TeleportBall(Vector3 targetPosition)
    {
        var rb = ballController.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballController.transform.position = targetPosition;
        rb.isKinematic = false;

        ballController.enabled = true;
        isBallOutSide = false;
        isBallTeleporting = false;
    }

    public void UpdateShootCount(int shootCount)
    {
        shootCounttext.text = shootCount.ToString();
    }

}
