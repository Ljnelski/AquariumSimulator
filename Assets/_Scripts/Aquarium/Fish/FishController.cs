using System.Collections;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;

    [SerializeField] private Fish _fish;

    private CharacterController _controller;
    private float _heading;
    private Vector3 _targetRotation;
    private bool _alive = true;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        _fish.OnFishDeath += Die;


        // Set random initial rotation
        _heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(transform.rotation.x, _heading, transform.rotation.z);

        StartCoroutine(NewHeading());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("TankWall"))
        {
            transform.forward = -transform.forward;
        }
    }

    private void Update()
    {
        if(!_alive) { return; }

        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, _targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.right);
        _controller.Move(forward * speed);
    }

    IEnumerator NewHeading()
    {
        while (_alive)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(3);
        }
    }

    private void Die()
    {
        Debug.Log("DIE");
        transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
        _alive = false;
    }

    private void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(_heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(_heading + maxHeadingChange, 0, 360);
        _heading = Random.Range(floor, ceil);
        _targetRotation = new Vector3(transform.rotation.x, _heading, transform.rotation.z);
    }

    private void OnDestroy()
    {
        _fish.OnFishDeath -= Die;
    }


}
