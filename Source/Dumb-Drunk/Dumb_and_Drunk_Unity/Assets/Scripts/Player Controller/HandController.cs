using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HandController : LimbController
{
    float MoveForce = 350f, ActivateDuration = 1f;

    [SerializeField]
    PlayerBalanceManager PlayerBalance;

    bool Detachable = false;

    FixedJoint Joint;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Active = false;
    }

    // ----------------------------- UPDATES -----------------------------

    private void Update()
    {
        if (Moving) KeepMoving();

    }

    public void KeepMoving()
    {
        rb.velocity = Vector3.zero;

        Vector3 FinalDirection = PlayerBalance.GetInitialPosition() - transform.position + CurrentDirection.normalized * 0.9f;

        FinalDirection.y = PlayerBalance.transform.position.y - transform.position.y;

        rb.AddForce(FinalDirection * MoveForce);

    }

    public override bool Release()
    {
        Invoke("Deactivate", ActivateDuration);
        rb.velocity = Vector3.zero;

        return base.Release();
    }

    void Deactivate()
    {
        Active = false;
    }

    public override bool Move()
    {
        Active = true;

        if (Joint != null) Destroy(Joint);
        return base.Move();
    }

    public override void Detach()
    {
        if (Joint != null) Destroy(Joint);
    }

    // ----------------------------- COLLIDERS -----------------------------

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer >= 9 && coll.gameObject.layer <= 12 && coll.gameObject.layer != gameObject.layer && Active)
        {
            if (coll.gameObject.GetComponent<PlayerObstacleManager>()) coll.gameObject.GetComponent<PlayerObstacleManager>().Fall();
        }
    }

}
