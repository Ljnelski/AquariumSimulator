using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;

    CharacterController controller;
    float heading;
    Vector3 targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(-90, heading, 0);

        StartCoroutine(NewHeading());
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.transform.CompareTag("TankWall"))
    //    //{
    //        transform.forward = -transform.forward;
    //        transform.right = -transform.right;
    //        Debug.Log("YOO");
    //    //}
    //}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("TankWall"))
        {
         //   transform.eulerAngles = new Vector3(-90, 0, -transform.rotation.z);

            transform.forward = -transform.forward;
        //transform.right = -transform.right;
        //Debug.Log("YOO");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.right);
        //if (transform.position.x >= 3 || transform.position.x <= -3 || transform.position.z >= 1.5 || transform.position.z <= -1.5)
        //{
        //    transform.forward = -transform.forward;
        //}
        controller.Move(forward * speed);
    }

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(3);
        }
    }

    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(-90, heading, 0);
    }
}
