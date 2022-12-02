using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]private float speed = 5f;
    [SerializeField]private float jumpHeight = 1f;
    [SerializeField]private float gravity = -9.81f;
	[SerializeField]public Transform cam;

    private float currentVelocity;
    [SerializeField]private float smoothTime = 0.5f;

    [SerializeField]private Transform groundSensor;
    [SerializeField]private LayerMask groundLayer;
	public float sensorRadius = 0.1f;
	public bool isGrounded;

    private Vector3 playerVelocity;

    [SerializeField]private LayerMask ignoreLayer;

    void Awake()
    {
	    controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
	    RaycastHit hit;
	    if(Physics.Raycast(transform.position, transform.forward, out hit, 10f, ignoreLayer))
	    {
		    Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
		
		    Vector3 hitPosition = hit.point;
		    string hitName = hit.transform.name;
		    string hitTag = hit.transform.tag;
		    float hitDistance = hit.distance;
		    Animator hitAnim = hit.transform.gameObject.GetComponent<Animator>();

		    if(hitTag == "obstaculo")
		    {
			    Debug.Log("impacto en obstaculo");
		    }
		
		    if(hitAnim != null)
		    {
			    hitAnim.SetBool("Jump", true);
		    }
	    }
	    
        else
	    {
		    Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
	    }

	    if(Input.GetButtonDown("Fire1"))
	    {
		    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    RaycastHit rayHit;
		
			if(Physics.Raycast (ray, out hit))
            {
                Debug.Log(hit.point);
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
	    }

	    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
	
	    if(movement != Vector3.zero)
	    {
		    float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
	
		    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref currentVelocity, smoothTime);

		    transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

		
		    Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
		    controller.Move(moveDirection * speed * Time.deltaTime);
	    }

	    isGrounded = Physics.CheckSphere(groundSensor.position, sensorRadius, groundLayer);

	    if(Physics.Raycast(groundSensor.position, Vector3.down, sensorRadius, groundLayer))
	    {
		    isGrounded = true;
		    Debug.DrawRay(groundSensor.position, Vector3.down * sensorRadius, Color.green);
	    }

	    else
	    {
		    isGrounded = false;
		    Debug.DrawRay(groundSensor.position, Vector3.down * sensorRadius, Color.red);
	    }

	    if(playerVelocity.y < 0 && isGrounded)
	    {
		    playerVelocity.y = 0;
	    }


	    if(isGrounded && Input.GetButtonDown("Jump"))
	    {
		    playerVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
	    }

	    playerVelocity.y += gravity * Time.deltaTime;
	    controller.Move(playerVelocity * Time.deltaTime);

	    void OnDrawGizmosSelected()
	    {
		    Gizmos.color = Color.blue;
		    Gizmos.DrawRay(groundSensor.position, Vector3.down * sensorRadius);
	    } 
    }
}
