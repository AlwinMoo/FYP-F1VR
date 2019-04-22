using UnityEngine;
using System.Collections;

public class BirdClass : MonoBehaviour
{
    enum birdBehaviors
    {
        sing,
        preen,
        ruffle,
        peck,
        hopForward,
        hopBackward,
        hopLeft,
        hopRight,
    }

    private GameObject audioManagerGO;
    private AudioManager audioManager;
    public AudioClip song;

    Animator anim;

    bool paused = false;
    bool idle = false;
    bool flying = true;
    bool landing = false;
    bool onGround = false;
    bool reachedTarget = false;
    bool soundCheck = false;

    BoxCollider birdCollider;
    Vector3 bColCenter;
    Vector3 bColSize;
    SphereCollider solidCollider;
    Rigidbody rigidbody;
    AudioSource audioSource;
    float distanceToTarget = 0.0f;
    float agitationLevel = .5f;
    float originalAnimSpeed = 1.0f;
    Vector3 originalVelocity = Vector3.zero;
    Vector3 target = Vector3.zero;
    float soundEndTime;

    int idleAnimationHash;
    int singAnimationHash;
    int ruffleAnimationHash;
    int preenAnimationHash;
    int peckAnimationHash;
    int hopForwardAnimationHash;
    int hopBackwardAnimationHash;
    int hopLeftAnimationHash;
    int hopRightAnimationHash;
    int landingAnimationHash;
    int flyAnimationHash;
    int hopIntHash;
    int flyingBoolHash;
    int peckBoolHash;
    int ruffleBoolHash;
    int preenBoolHash;
    int landingBoolHash;
    int singTriggerHash;
    int flyingDirectionHash;

    private GameObject player;
    Vector3 playerPosition;

    void OnEnable()
    {
        audioManagerGO = GameObject.FindGameObjectWithTag("AudioManager");
        audioManager = audioManagerGO.GetComponent<AudioManager>();

        player = GameObject.FindGameObjectWithTag("Player");

        birdCollider = gameObject.GetComponent<BoxCollider>();
        bColCenter = birdCollider.center;
        bColSize = birdCollider.size;
        solidCollider = gameObject.GetComponent<SphereCollider>();
        anim = gameObject.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
        flyAnimationHash = Animator.StringToHash("Base Layer.fly");
        hopIntHash = Animator.StringToHash("hop");
        flyingBoolHash = Animator.StringToHash("flying");
        peckBoolHash = Animator.StringToHash("peck");
        ruffleBoolHash = Animator.StringToHash("ruffle");
        preenBoolHash = Animator.StringToHash("preen");
        landingBoolHash = Animator.StringToHash("landing");
        singTriggerHash = Animator.StringToHash("sing");
        flyingDirectionHash = Animator.StringToHash("flyingDirectionX");
        anim.SetFloat("IdleAgitated", agitationLevel);

        anim.SetBool("flying", flying); //!< Birds default starting animation is set to flying */
    }

    /// <summary>
    /// pauses bird
    /// </summary>
    void PauseBird()
    {
        originalAnimSpeed = anim.speed;
        anim.speed = 0;
        if (!rigidbody.isKinematic) { originalVelocity = rigidbody.velocity; }
        rigidbody.isKinematic = true;
        audioSource.Stop();
        paused = true;
    }

    /// <summary>
    /// resumes bird
    /// </summary>
    void UnPauseBird()
    {
        anim.speed = originalAnimSpeed;
        rigidbody.isKinematic = false;
        rigidbody.velocity = originalVelocity;
        paused = false;
    }

    /// <summary>
    /// Birds fly to the target
    /// </summary>
    /// <param name="target">destination</param>
    /// <returns></returns>
    IEnumerator FlyToTarget(Vector3 target)
    {
        flying = true;
        landing = false;
        onGround = false;
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.drag = 0.5f;
        anim.applyRootMotion = false;
        anim.SetBool(flyingBoolHash, true);
        anim.SetBool(landingBoolHash, false);

        while (anim.GetCurrentAnimatorStateInfo(0).nameHash != flyAnimationHash)//!< Wait to apply velocity until the bird is entering the flying animation */
        {
            yield return 0;
        }

        rigidbody.AddForce((transform.forward * 50.0f) + (transform.up * 100.0f));//!< birds fly up and away for 1 second before orienting to the next target */
        float t = 0.0f;
        while (t < 1.0f)
        {
            if (!paused)
            {
                t += Time.deltaTime;
                if (t > .2f && !solidCollider.enabled)
                {
                    solidCollider.enabled = true;
                }
            }
            yield return 0;
        }

        Vector3 vectorDirectionToTarget = (target - transform.position).normalized;//!< start to rotate toward target */
        Quaternion finalRotation = Quaternion.identity;
        Quaternion startingRotation = transform.rotation;
        distanceToTarget = Vector3.Distance(transform.position, target);
        Vector3 forwardStraight;//the forward vector on the xz plane
        Vector3 tempTarget = target;
        t = 0.0f;

        if (vectorDirectionToTarget.y > .5f)//!< if the target is directly above the bird the bird needs to fly out before going up, preventing them from taking off like a rocket upwards */
        {
            tempTarget = transform.position + (new Vector3(transform.forward.x, .5f, transform.forward.z) * distanceToTarget);

            while (vectorDirectionToTarget.y > .5f)
            {
                vectorDirectionToTarget = (tempTarget - transform.position).normalized;
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                t += Time.deltaTime * 0.5f;
                rigidbody.AddForce(transform.forward * 70.0f * Time.deltaTime);

                vectorDirectionToTarget = (target - transform.position).normalized;//!< reset the variable to reflect the actual target and not the temptarget */

                yield return null;
            }
        }

        finalRotation = Quaternion.identity;
        startingRotation = transform.rotation;
        distanceToTarget = Vector3.Distance(transform.position, target);

        while (transform.rotation != finalRotation || distanceToTarget >= 1.5f)//!< rotate the bird toward the target over time */
        {
            if (!paused)
            {
                distanceToTarget = Vector3.Distance(transform.position, target);
                vectorDirectionToTarget = (target - transform.position).normalized;
                if (vectorDirectionToTarget == Vector3.zero)
                {
                    vectorDirectionToTarget = new Vector3(0.0001f, 0.00001f, 0.00001f);
                }
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                t += Time.deltaTime * 0.5f;
                rigidbody.AddForce(transform.forward * 70.0f * Time.deltaTime);
            }
            yield return 0;
        }

        float flyingForce = 50.0f;//!< keep the bird pointing at the target and move toward it*/
        while (true)
        {
            if (!paused)
            {
                vectorDirectionToTarget = (target - transform.position).normalized;
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                transform.rotation = finalRotation;
                rigidbody.AddForce(transform.forward * flyingForce * Time.deltaTime);
                distanceToTarget = Vector3.Distance(transform.position, target);
                if (distanceToTarget <= 1.5f)
                {
                    solidCollider.enabled = false;
                    if (distanceToTarget < 0.5f)
                    {
                        break;
                    }
                    else
                    {
                        rigidbody.drag = 2.0f;
                        flyingForce = 50.0f;
                    }
                }
                else if (distanceToTarget <= 5.0f)
                {
                    rigidbody.drag = 1.0f;
                    flyingForce = 50.0f;
                }
            }
            yield return 0;
        }

        anim.SetFloat(flyingDirectionHash, 0);
    }

    /// <summary>
    /// Birds attempting to fly in a curve will rotate their bodies accordingly for realism
    /// </summary>
    /// <param name="birdForward">front of bird</param>
    /// <param name="dirToTarget">direction to target</param>
    /// <returns></returns>
    float FindBankingAngle(Vector3 birdForward, Vector3 dirToTarget)//!< Sets a variable between -1 and 1 to control the left and right banking animation */
    {
        Vector3 cr = Vector3.Cross(birdForward, dirToTarget);
        float ang = Vector3.Dot(cr, Vector3.up);
        return ang;
    }

    /// <summary>
    /// Function that checks conditions before deciding to fly to target
    /// </summary>
    void FlyAway()
    {
        if (target == Vector3.zero || reachedTarget)
        {
            StopCoroutine("FlyToTarget");
            target = new Vector3(Random.Range(-20, 20) + playerPosition.x, Random.Range(5, 10) + playerPosition.y, Random.Range(-20, 20) + playerPosition.z);
           // Debug.Log(target);
            StartCoroutine("FlyToTarget", target);
            reachedTarget = false;
        }
    }

    /// <summary>
    /// Plays audio at the bird's position
    /// </summary>
    /// <param name="music">selected clip to play</param>
    void PlayAudio(AudioClip music)
    {
        if (audioSource.clip != null)
        {
            if (audioSource.clip.name == music.name)
                return;
        }
        else
        {
            //changing music it plays
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
        }
    }

    void Update()
    {
        playerPosition = player.transform.position;

        if (!paused)
        {
            FlyAway();

            if (flying)
            {
                audioSource.clip = song;
                audioSource.volume = audioManager.GetMasterVolume();

                if (!audioSource.isPlaying)
                {
                    if (!soundCheck)
                    {
                        soundEndTime = Time.time;
                        soundCheck = true;
                    }

                    if (soundEndTime + Random.Range(5, 100) < Time.time)
                    {
                        audioSource.Play();
                        soundCheck = false;
                    }
                }
            }

            if (target != Vector3.zero)
            {
                if (Vector3.Distance(target, transform.position) < 5.0f)
                {
                    reachedTarget = true;
                }
            }
        }
    }
}
