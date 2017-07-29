using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{

    public float Acceleration = 5.0f;
    public float RotationPerSecond = 180.0f;

    private Rigidbody2D body;

	void Start ()
	{
	    this.body = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
	    if (Input.GetKey(KeyCode.A))
	    {
	        RotateLeft();
	    }

	    if (Input.GetKey(KeyCode.D))
	    {
	        RotateRight();
	    }

	    if (Input.GetKey(KeyCode.W))
	    {
	        Thrust();
	    }
	    else
	    {
	        Stop();
	    }
	}

    void Thrust()
    {
        this.body.velocity = this.transform.up * this.Acceleration;
    }

    void Stop()
    {
        this.body.velocity = Vector2.zero;
    }

    void RotateLeft()
    {
        Rotate(RotationPerSecond * Time.deltaTime);
    }

    void RotateRight()
    {
        Rotate(-RotationPerSecond * Time.deltaTime);
    }

    void Rotate(float degrees)
    {
        this.body.rotation += degrees;
    }
}
