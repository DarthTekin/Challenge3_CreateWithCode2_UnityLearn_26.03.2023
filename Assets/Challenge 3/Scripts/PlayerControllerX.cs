using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public bool isLowEnough = true;

    private float floatForce;
    public float maxFloatForce = 30;
    public float minFloatForce = 0;
    public float upperBound = 17;
    public float lowerBound = 1f;
    private float gravityModifier = 2.0f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip groundSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        floatForce = maxFloatForce;


        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * floatForce, ForceMode.Force);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && isLowEnough && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Force);
        }

        if (transform.position.y >= upperBound)
        {
            isLowEnough = false;
            playerRb.AddForce(Vector3.down * lowerBound, ForceMode.VelocityChange);
            transform.position = new Vector3(transform.position.x, upperBound, transform.position.z);
            floatForce = minFloatForce;
            //playerRb.mass = gravityModifier;
        }
        else if (transform.position.y < upperBound && transform.position.y >= lowerBound)
        {
            floatForce = upperBound;
            isLowEnough = true;
            playerRb.mass = lowerBound;
        }

        else if (transform.position.y < lowerBound)
        {
            floatForce = upperBound;
            playerRb.mass = gravityModifier;
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            if (!gameOver)
            {
                playerAudio.PlayOneShot(groundSound, 1.0f);
            }
            //transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
            Invoke("DestroyObject", 1.5f);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }

    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }



}
