using OneRoomFactory.Transporters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Factory
{
    public class Movable : MonoBehaviour
    {
        public Collider LastCollider { get; set; }
        public MovableType Type;
        public int Units = 1;
        public ITransporter TransportedBy;

        private Vector3 moveTarget = Vector3.down; //magical values that cant be reached normally by our game, Vector3 isn nullable :(
        private Vector3 nextTarget = Vector3.down; //magical values that cant be reached normally by our game, Vector3 isn nullable :(

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (moveTarget != Vector3.down)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, Time.fixedDeltaTime);
                if (Vector3.Distance(transform.position, moveTarget) < 0.5f)
                {
                    moveTarget = nextTarget;
                    nextTarget = Vector3.down;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Transporter"))
            {
                var trans = collision.collider.GetComponent<ITransporter>();
                if (trans.Type == TransporterType.Belt)
                {
                    var belt = trans as Belt;
                    if (moveTarget == Vector3.down)
                    {
                        moveTarget = belt.GetTransform().position + belt.MoveVector;
                    }
                    else
                    {
                        nextTarget = belt.GetTransform().position + belt.MoveVector;
                    }
                }
            }
        }
    }
}