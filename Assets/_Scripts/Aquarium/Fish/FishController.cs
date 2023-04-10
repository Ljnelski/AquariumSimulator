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


    //void OnCollisionEnter(Collision collision)
    //{
    //    // If the fish collides with an object, apply a force to move it away from the collision point
    //    Vector3 normal = collision.contacts[0].normal;
    //    GetComponent<Rigidbody>().AddForce(normal * 10, ForceMode.Impulse);
    //    Debug.Log("Entered collision");
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    // When the fish exits a collision, stop applying the force to move it away
    //    GetComponent<Rigidbody>().velocity = Vector3.zero;
    //}




    void Start()
    {
        controller = GetComponent<CharacterController>();


        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(transform.rotation.x, heading, transform.rotation.z);

        StartCoroutine(NewHeading());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("TankWall"))
        {
            transform.forward = -transform.forward;
        }
        //else if (hit.transform.CompareTag("Obstacle"))
        //{
        //    Debug.Log("HO HO HO");
        //    controller.enableOverlapRecovery= false;
        //    //disable colliders for when hitting a rock or sth? 
        //    //gameObject.GetComponent<Collider>().enabled = false;
        //}
    }

    void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.right);
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
        targetRotation = new Vector3(transform.rotation.x, heading, transform.rotation.z);
    }


}
