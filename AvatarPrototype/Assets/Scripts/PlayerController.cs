using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform groundedBody;
    public Transform leftSurface;
    public Transform rightSurface;
    public Transform playerSpawn;
    public Transform punchPrefab;

    public int playerNumber;

    public bool grounded = false;
    public bool leftJump = false;
    public bool rightJump = false;
    public bool wallCling = false;
    public bool stunned = false;

    public float dirx;

    // Player Handling
    public float speed = 20.0f;
	public bool isAcceleration = false;
    public float acceleration = 40.0f;
	public float jumpHeight = 7.0f;
    private float jumpVeloc = 0.0f;
    public float jumpReleaseVeloc = 6.0f;
	public bool isFriction = false;
    public float friction = 1000.0f;
    public float gravity = -100.0f;
    public float wallJumpVeloc = 25.0f;
    public float wallClingTime = 0.5f; // in seconds
    public int startingHealth = 100;
    public float stunTime = 0.5f;

    public float test = 0.0f;
    //public float listen;

    private Vector2 gravityVector;

    private Vector2 leftWallJump;
    private Vector2 rightWallJump;

    private float joystickEpsilon = 0.2f;
    private float velocityEpsilon = 2.0f;
    private float wallClingTimer = 0.0f;
    private float stunTimer = 0.0f;

    public string horizontalAxis = "Horizontal";
    public string jump = "Jump";
    public string punch = "Punch";

    public int health;

    void Start()
    {
        health = startingHealth;
        dirx = 1.0f;
        Set();
    }

    void Set()
    {
        if (playerNumber > 0)
        {
            horizontalAxis = "Horizontal" + playerNumber.ToString();
            jump = "Jump" + playerNumber.ToString();
            punch = "Punch" + playerNumber.ToString();
        }
		jumpVeloc = Mathf.Sqrt(2.0f * -gravity * jumpHeight);
        gravityVector = new Vector2(0.0f, gravity);

		leftWallJump = new Vector2(jumpVeloc, jumpVeloc);
		Debug.DrawRay(transform.position, leftWallJump);
        /*
		leftWallJump = new Vector2(1.0f, 1.0f);
        leftWallJump.Normalize();
        leftWallJump *= wallJumpVeloc;
        */
        rightWallJump = new Vector2(-leftWallJump.x, leftWallJump.y);
    }

    void Update()
    {
        Set();

        Vector2 accelerationVector = Vector2.zero;

        // identify what surfaces I am touching
        //grounded = groundedBody.GetComponent<TouchingScript>().touching;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 scale = transform.localScale;
        Vector2 s = collider.size;
        s.x *= scale.x;
        s.y *= scale.y;
        Vector2 c = collider.center;
        Vector2 p = transform.position;

        grounded = false;
        for (int i = 0; i < 3; i++)
        {
            float x = (p.x + c.x - s.x / 2) + s.x / 2 * i; // Left, center and then rightmost point of collider
            float y = p.y + c.y + s.y / 2 * -1.0f; // Bottom of collider

            LayerMask m = 1 << LayerMask.NameToLayer("SolidTerrain");
            Ray ray = new Ray(new Vector2(x, y), new Vector2(0, -1));
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics2D.Raycast(ray.origin, ray.direction, 0.05f, m))
            {
                grounded = true;
                break;
            }
        }

        leftJump = leftSurface.GetComponent<TouchingScript>().touching;
        rightJump = rightSurface.GetComponent<TouchingScript>().touching;

        // Decrement wallClingTimer
        if (wallCling)
        {
            wallClingTimer -= Time.deltaTime;
            if (grounded || wallClingTimer < 0.0f || leftSurface.GetComponent<TouchingScript>().LostContact || rightSurface.GetComponent<TouchingScript>().LostContact)
            {
                wallCling = false;
            }
        }
        // Cling to walls on contact
        else if (!grounded && (leftSurface.GetComponent<TouchingScript>().NewContact || rightSurface.GetComponent<TouchingScript>().NewContact))
        {
            wallClingTimer = wallClingTime;
            wallCling = true;
        }

        // Jump
        if (Input.GetButtonDown(jump))
        {
            if (grounded)
            {
                SetY(rigidbody2D, jumpVeloc);
            }
            else if (leftJump)
            {
                rigidbody2D.velocity = leftWallJump;
            }
            else if (rightJump)
            {
                rigidbody2D.velocity = rightWallJump;
            }
        }
        // Jump Release
        else if (Input.GetButtonUp(jump))
        {
            SetY(rigidbody2D, Mathf.Min(rigidbody2D.velocity.y, jumpReleaseVeloc));
        }
        
        // Apply gravity if not grounded
        if (!grounded)
        {
            accelerationVector = gravityVector * Time.deltaTime;
        }

        float dirx = Input.GetAxisRaw(horizontalAxis);

        // Avoid joystick bullshit
        if (Mathf.Abs(dirx) < joystickEpsilon)
        {
            dirx = 0.0f;
        }

        // Apply friction if there is no horizontal input
        if (Mathf.Abs(dirx) < joystickEpsilon && grounded)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) > velocityEpsilon && isFriction)
            {
                accelerationVector.x += friction * -Mathf.Sign(rigidbody2D.velocity.x) * Time.deltaTime;
            }
            else
            {
                // Round very slight motion down to zero
                rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);
            }
        }

        // Apply input
        if (!wallCling)
        {
			if (isAcceleration || !grounded)
			{
            	accelerationVector.x += dirx * acceleration * Time.deltaTime;
			}
			else
			{
				if (dirx > joystickEpsilon)
				{
					rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
				}
				else if (dirx < -joystickEpsilon)
				{
					rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
				}
			}
        }
        
        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0.0f)
            {
                stunned = false;
            }
        }

        rigidbody2D.velocity += accelerationVector;

        if (!stunned)
        {
            if (rigidbody2D.velocity.x > speed)
            {
                rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
            }
            else if (rigidbody2D.velocity.x < -speed)
            {
                rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
            }
        }

        if (dirx != 0.0f)
        {
            this.dirx = dirx;
        }

        //Debug.Log("Friction :\t" + frictionAccel.ToString() + " Acceleration:\t" + accelerationVector.ToString() + " Dir:\t" + dirx.ToString() + " Rigidbody2D.vel:\t" + rigidbody2D.velocity.ToString());
        if (Input.GetButtonDown(punch))
        {
            Debug.Log("Pawnch");
        }

    }

    public bool Damage(int damage)
    {
        health -= damage;
        stunned = true;
        stunTimer = stunTime;
        Debug.Log(damage.ToString());
        Debug.Log(health.ToString());
        if (health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public void Kill()
    {
        health = startingHealth;
        rigidbody2D.transform.position = playerSpawn.position;
        rigidbody2D.velocity = Vector2.zero;
    }

    private void SetX(Rigidbody2D r, float x)
    {
        r.velocity = new Vector2(x, r.velocity.y);
    }

    private void SetY(Rigidbody2D r, float y)
    {
        r.velocity = new Vector2(r.velocity.x, y);
    }
}
