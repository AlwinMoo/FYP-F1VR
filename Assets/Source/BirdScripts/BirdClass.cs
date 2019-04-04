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
    float distanceToTarget = 0.0f;
    float agitationLevel = .5f;
    float originalAnimSpeed = 1.0f;
    Vector3 originalVelocity = Vector3.zero;
    Vector3 target = Vector3.zero;
    float soundEndTime;

    //hash variables for the animation states and animation properties
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

    void OnEnable()
    {
        birdCollider = gameObject.GetComponent<BoxCollider>();
        bColCenter = birdCollider.center;
        bColSize = birdCollider.size;
        solidCollider = gameObject.GetComponent<SphereCollider>();
        anim = gameObject.GetComponent<Animator>();

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

        //Birds default starting animation is set to flying
        anim.SetBool("flying", flying);
    }

    void PauseBird()
    {
        originalAnimSpeed = anim.speed;
        anim.speed = 0;
        if (!GetComponent<Rigidbody>().isKinematic) { originalVelocity = GetComponent<Rigidbody>().velocity; }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<AudioSource>().Stop();
        paused = true;
    }

    void UnPauseBird()
    {
        anim.speed = originalAnimSpeed;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = originalVelocity;
        paused = false;
    }

    IEnumerator FlyToTarget(Vector3 target)
    {
        flying = true;
        landing = false;
        onGround = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().drag = 0.5f;
        anim.applyRootMotion = false;
        anim.SetBool(flyingBoolHash, true);
        anim.SetBool(landingBoolHash, false);

        //Wait to apply velocity until the bird is entering the flying animation
        while (anim.GetCurrentAnimatorStateInfo(0).nameHash != flyAnimationHash)
        {
            yield return 0;
        }

        //birds fly up and away for 1 second before orienting to the next target
        GetComponent<Rigidbody>().AddForce((transform.forward * 50.0f) + (transform.up * 100.0f));
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
        //start to rotate toward target
        Vector3 vectorDirectionToTarget = (target - transform.position).normalized;
        Quaternion finalRotation = Quaternion.identity;
        Quaternion startingRotation = transform.rotation;
        distanceToTarget = Vector3.Distance(transform.position, target);
        Vector3 forwardStraight;//the forward vector on the xz plane
        Vector3 tempTarget = target;
        t = 0.0f;

        //if the target is directly above the bird the bird needs to fly out before going up
        //this should stop them from taking off like a rocket upwards
        if (vectorDirectionToTarget.y > .5f)
        {
            tempTarget = transform.position + (new Vector3(transform.forward.x, .5f, transform.forward.z) * distanceToTarget);

            while (vectorDirectionToTarget.y > .5f)
            {
                //Debug.DrawLine (tempTarget,tempTarget+Vector3.up,Color.red);
                vectorDirectionToTarget = (tempTarget - transform.position).normalized;
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                t += Time.deltaTime * 0.5f;
                GetComponent<Rigidbody>().AddForce(transform.forward * 70.0f * Time.deltaTime);

                vectorDirectionToTarget = (target - transform.position).normalized;//reset the variable to reflect the actual target and not the temptarget
                
                yield return null;
            }
        }

        finalRotation = Quaternion.identity;
        startingRotation = transform.rotation;
        distanceToTarget = Vector3.Distance(transform.position, target);

        //rotate the bird toward the target over time
        while (transform.rotation != finalRotation || distanceToTarget >= 1.5f)
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
                GetComponent<Rigidbody>().AddForce(transform.forward * 70.0f * Time.deltaTime);
            }
            yield return 0;
        }

        //keep the bird pointing at the target and move toward it
        float flyingForce = 50.0f;
        while (true)
        {
            if (!paused)
            {
                vectorDirectionToTarget = (target - transform.position).normalized;
                finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                transform.rotation = finalRotation;
                GetComponent<Rigidbody>().AddForce(transform.forward * flyingForce * Time.deltaTime);
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
                        GetComponent<Rigidbody>().drag = 2.0f;
                        flyingForce = 50.0f;
                    }
                }
                else if (distanceToTarget <= 5.0f)
                {
                    GetComponent<Rigidbody>().drag = 1.0f;
                    flyingForce = 50.0f;
                }
            }
            yield return 0;
        }

        anim.SetFloat(flyingDirectionHash, 0);
    }

    //Sets a variable between -1 and 1 to control the left and right banking animation
    float FindBankingAngle(Vector3 birdForward, Vector3 dirToTarget)
    {
        Vector3 cr = Vector3.Cross(birdForward, dirToTarget);
        float ang = Vector3.Dot(cr, Vector3.up);
        return ang;
    }

    void FlyAway()
    {
        if (target == Vector3.zero || reachedTarget)
        {
            StopCoroutine("FlyToTarget");
            target = new Vector3(Random.Range(-50, 50), Random.Range(40, 60), Random.Range(-50, 50));
            Debug.Log(target);
            StartCoroutine("FlyToTarget", target);
            reachedTarget = false;
        }
    }

    void PlayAudio(AudioClip music)
    {
        if (GetComponent<AudioSource>().clip != null)
        {
            if (GetComponent<AudioSource>().clip.name == music.name)
                return;
        }
        else
        {
            //changing music it plays
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = music;
            GetComponent<AudioSource>().Play();
        }
    }

    void Update()
    {
        if (!paused)
        {
            FlyAway();

            if (flying)
            {
                GetComponent<AudioSource>().clip = song;

                if (!GetComponent<AudioSource>().isPlaying)
                {
                    if (!soundCheck)
                    {
                        soundEndTime = Time.time;
                        soundCheck = true;
                    }

                    if (soundEndTime + Random.Range(5, 100) < Time.time)
                    {
                        GetComponent<AudioSource>().Play();
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
