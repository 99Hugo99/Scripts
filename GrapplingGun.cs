using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, PlayerCamera, player;
    public GameObject Hook;
    public float maxDistance = 1000f;
    private SpringJoint joint;

    [Header("Rope Settings")]
    public float ropeExtendSpeed = 60f;
    public float ropeRetractSpeed = 70f;
    public float distanceSpeedScale = 0.5f;
    public float wobbleAmplitude = 0.2f;
    public float wobbleFrequency = 3f;
    public float wobbleDuration = 0.6f;
    public float distanceWobbleScale = 0.02f;
    public int ropeSegments = 20;

    [Header("Hit Object Spawn")]
    public GameObject objectToSpawnAtHit;

    private Vector3 currentGrapplePosition;
    private float wobbleTimer = 0f;
    private bool isExtending = false;
    private bool isRetracting = false;
    private GameObject spawnedHitObject;
    private float scaledExtendSpeed;
    private float scaledRetractSpeed;
    private float scaledWobbleDuration;
    private RaycastHit pendingHit;
    private bool hasPendingHit = false;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
            StartGrapple();
        else if (Input.GetMouseButtonUp(2))
            StopGrapple();
    }

    void LateUpdate()
    {
        DrawRope();

        // CHANGED: Update hook position every frame to follow rope end
        if (spawnedHitObject != null && (isExtending || isRetracting || joint != null))
        {
            Vector3 moveDir = (currentGrapplePosition - gunTip.position).normalized;
            if (moveDir != Vector3.zero)
                spawnedHitObject.transform.rotation = Quaternion.LookRotation(moveDir);

            spawnedHitObject.transform.position = currentGrapplePosition;
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            Hook.SetActive(false);
            grapplePoint = hit.point;
            pendingHit = hit;
            hasPendingHit = true;

            float distance = Vector3.Distance(gunTip.position, grapplePoint);
            scaledExtendSpeed = ropeExtendSpeed + distance * distanceSpeedScale;
            scaledRetractSpeed = ropeRetractSpeed + distance * distanceSpeedScale;
            scaledWobbleDuration = wobbleDuration + distance * distanceWobbleScale;

            lr.positionCount = ropeSegments;
            currentGrapplePosition = gunTip.position;
            isExtending = true;
            isRetracting = false;
            wobbleTimer = scaledWobbleDuration;

            // CHANGED: Spawn hook immediately at gun tip so it travels with the rope
            if (spawnedHitObject != null) Destroy(spawnedHitObject);
            if (objectToSpawnAtHit != null)
            {
                spawnedHitObject = Instantiate(objectToSpawnAtHit, gunTip.position, Quaternion.LookRotation(PlayerCamera.forward));
            }

            FindObjectOfType<AudioManager>().Play("Grapple");
        }
        else
        {
            lr.positionCount = 0;
            isExtending = false;
            isRetracting = false;
            hasPendingHit = false;
        }
    }

    void AttachGrapple()
    {
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        // CHANGED: Hook is already spawned, just snap it to the exact grapple point
        if (spawnedHitObject != null)
            spawnedHitObject.transform.position = grapplePoint;

        hasPendingHit = false;
    }

    void StopGrapple()
    {
        Destroy(joint);
        isExtending = false;
        isRetracting = true;
        hasPendingHit = false;
        wobbleTimer = scaledWobbleDuration;
    }

    void DrawRope()
    {
        if (isRetracting)
        {
            currentGrapplePosition = Vector3.MoveTowards(currentGrapplePosition, gunTip.position, scaledRetractSpeed * Time.deltaTime);

            if (Vector3.Distance(currentGrapplePosition, gunTip.position) < 0.01f)
            {
                isRetracting = false;
                lr.positionCount = 0;
                Hook.SetActive(true);

                // CHANGED: Destroy hook object when fully retracted
                if (spawnedHitObject != null)
                {
                    Destroy(spawnedHitObject);
                    spawnedHitObject = null;
                }
                return;
            }
        }

        if (!isRetracting && !isExtending && joint == null) return;

        if (isExtending)
        {
            currentGrapplePosition = Vector3.MoveTowards(currentGrapplePosition, grapplePoint, scaledExtendSpeed * Time.deltaTime);

            if (Vector3.Distance(currentGrapplePosition, grapplePoint) < 0.01f)
            {
                isExtending = false;
                if (hasPendingHit) AttachGrapple();
            }
        }

        if (lr.positionCount < 2) return;

        Vector3 ropeDir = (currentGrapplePosition - gunTip.position).normalized;
        Vector3 perp = Vector3.Cross(ropeDir, Vector3.up);
        if (perp.sqrMagnitude < 0.001f)
            perp = Vector3.Cross(ropeDir, Vector3.right);
        perp.Normalize();

        float wobbleFade = wobbleTimer > 0f ? wobbleTimer / scaledWobbleDuration : 0f;
        if (wobbleTimer > 0f) wobbleTimer -= Time.deltaTime;

        for (int i = 0; i < lr.positionCount; i++)
        {
            float t = i / (float)(lr.positionCount - 1);
            Vector3 basePos = Vector3.Lerp(gunTip.position, currentGrapplePosition, t);

            float wave = Mathf.Sin(t * wobbleFrequency * Mathf.PI + Time.time * 8f)
                         * wobbleAmplitude
                         * wobbleFade
                         * (t * (1f - t) * 4f);

            lr.SetPosition(i, basePos + perp * wave);
        }
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
