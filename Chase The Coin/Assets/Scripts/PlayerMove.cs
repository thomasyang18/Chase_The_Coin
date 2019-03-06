using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string mouseLeftClickInputName;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float shotSpeed;
    [SerializeField] private GameObject playerHitIndicator;
    private float startPoint;
    private float endPoint;
    [SerializeField] private LayerMask floor;
    private float playerRadius;
    private Rigidbody playerRB;
    private bool isInAir;
    void Start()
    {
        playerRadius = GetComponent<SphereCollider>().radius;
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMovement();
        playerRB.velocity = Vector3.zero;
    }

    

    private void PlayerMovement() {
        // The player will move by shooting a vector in a certain direction, which will propel the player
        if (Input.GetButtonDown(mouseLeftClickInputName) && !isInAir) {
            // since we are shooting straight foward, the player will shoot out a ray in the center of the screen
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, floor))
            {
                Debug.DrawLine(ray.origin, hit.point);
                StartCoroutine(playerMoveAlongRay(hit));
            }
        }

    }

    private IEnumerator playerMoveAlongRay(RaycastHit hit) {
        GameObject playerHit = Instantiate(playerHitIndicator, hit.point, Quaternion.identity) as GameObject;
        isInAir = true;

        Vector3 endPoint = hit.point;
        Vector3 startPoint= transform.position;

        float time = 0f;
        float timeToMove = 1f;
        while (time < timeToMove)
        {
            time += Time.deltaTime;
            playerRB.position = Vector3.Lerp(startPoint, endPoint, time/timeToMove);
            yield return null;
        }

        isInAir = false;
        Destroy(playerHit);
        yield return null;
    }


    


}
