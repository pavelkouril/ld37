using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Factory;
using UnityEngine;
using OneRoomFactory.Managers;

namespace OneRoomFactory.Transporters
{
    public class Hand : ITransporter
    {
        public float speed = 1.0f;
        public Transform core;
        public Transform lowerHand;
        public Transform upperHand;
        public Transform grabber;
        public LayerMask MovableLayer;

        private Transform grabbedObject;

        public override TransporterType Type { get { return TransporterType.Hand; } }

        public enum State
        {
            STATE_DEFAULT,
            STATE_GRABBED,
            STATE_LANDING,
            STATE_HOMECOMING
        }

        private State state;
        private float handLength = 1.0f;
        private float coreAngle;
        private float coreTargetAngle;
        private float lowerAngle;
        private float lowerTargetAngle;
        private float upperAngle;
        private float upperTargetAngle;

        public MovableType AcceptedType;

        private Movable lastMoved { get; set; }

        void Start()
        {
            state = State.STATE_DEFAULT;
            coreAngle = 0.0f;
            coreTargetAngle = 0.0f;
            lowerAngle = 0.0f;
            lowerTargetAngle = 0.0f;
            upperAngle = 0.0f;
            upperTargetAngle = 0.0f;
            lastMoved = null;
        }

        private bool AngleDirection(float targetAngle, float angle, out float direction)
        {
            float a1 = (targetAngle - angle + 360) % 360;
            float a2 = (angle - targetAngle + 360) % 360;
            if (a1 < a2)
            {
                direction = a1;
            }
            else
            {
                direction = -a2;
            }

            bool reached = false;
            if (direction < speed * 0.5f && direction > speed * -0.5f)
            {
                reached = true;
            }

            direction = Mathf.Max(Mathf.Min(direction, speed), -speed);

            return reached;
        }

        void FixedUpdate()
        {
            if (Output == null)
            {
                return;
            }

            if (ToMove == null && state == State.STATE_DEFAULT)
            {
                foreach (var collider in Physics.OverlapSphere(transform.position, 2.2f, MovableLayer))
                {
                    var movable = collider.GetComponent<Movable>();
                    if ((lastMoved == null || movable != lastMoved) && movable.Type == AcceptedType)
                    {
                        ToMove = movable;
                        lastMoved = movable;
                        break;
                    }
                }
            }

            float distance = 0.0f;

            if (ToMove != null)
            {
                distance = CalculateAnglesForTransform(ToMove.transform);
            }
            else
            {
                if (state == State.STATE_LANDING && grabbedObject != null)
                {
                    CalculateAnglesForTransform(Output.transform);
                }
                else
                {
                    coreTargetAngle = 0.0f;
                    upperTargetAngle = 0.0f;
                    lowerTargetAngle = 0.0f;
                }
            }

            if (distance > 2.2f)
            {
                ToMove = null;
                lastMoved = null;
                state = State.STATE_HOMECOMING;
            }
            else
            {
                float coreDir = 0.0f;
                bool hReached = AngleDirection(coreTargetAngle, coreAngle, out coreDir);
                coreAngle += coreDir;

                core.localRotation = Quaternion.AngleAxis(coreAngle, new Vector3(0.0f, 0.0f, 1.0f));

                float upperDir = 0.0f;
                bool uReached = AngleDirection(upperTargetAngle, upperAngle, out upperDir);
                upperAngle += upperDir;

                float lowerDir = 0.0f;
                bool lReached = AngleDirection(lowerTargetAngle, lowerAngle, out lowerDir);
                lowerAngle += lowerDir;

                lowerHand.localRotation = Quaternion.AngleAxis(lowerAngle, new Vector3(1.0f, 0.0f, 0.0f));
                upperHand.localRotation = Quaternion.AngleAxis(upperAngle, new Vector3(1.0f, 0.0f, 0.0f));

                if (hReached && uReached && lReached)
                {
                    switch (state)
                    {
                        case State.STATE_DEFAULT:
                            if (ToMove != null)
                            {
                                ToMove.transform.parent = grabber.transform;
                                ToMove.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                                ToMove.transform.localRotation = Quaternion.AngleAxis(0.0f, new Vector3(0.0f, 1.0f, 0.0f));
                                ToMove.GetComponent<Rigidbody>().isKinematic = true;
                                ToMove.ResetMove();
                                ToMove.GetComponent<Rigidbody>().useGravity = false;
                                ToMove.isGrabbed = true;
                                grabbedObject = ToMove.transform;
                                ToMove = null;
                                state = State.STATE_GRABBED;
                            }
                            break;

                        case State.STATE_GRABBED:
                            state = State.STATE_LANDING;
                            break;

                        case State.STATE_LANDING:
                            if (grabbedObject != null)
                            {
                                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                                grabbedObject.GetComponent<Rigidbody>().useGravity = true;
                                grabbedObject.GetComponent<Movable>().isGrabbed = false;
                                grabbedObject.transform.position = Output.transform.position + new Vector3(0.0f, 0.2f, 0.0f);
                                grabbedObject.rotation = Quaternion.identity;
                                grabbedObject.transform.parent = null;
                                grabbedObject = null;
                            }
                            state = State.STATE_HOMECOMING;
                            break;

                        case State.STATE_HOMECOMING:
                            state = State.STATE_DEFAULT;
                            break;
                    }
                }
            }
        }

        private float CalculateAnglesForTransform(Transform t)
        {
            float distance = Vector3.Distance(t.position, core.position);
            float tmpDistance = distance;
            if (tmpDistance > 1.9f)
            {
                tmpDistance = 1.9f;
            }

            Vector3 direction = t.position - core.position;
            coreTargetAngle = (Mathf.Atan2(direction.z, direction.x) * -Mathf.Rad2Deg + 90.0f);

            upperTargetAngle = 90.0f - Mathf.Asin((tmpDistance * 0.5f) / handLength) * Mathf.Rad2Deg * 2.0f;
            lowerTargetAngle = 136.0f - Mathf.Acos((tmpDistance * 0.5f) / handLength) * Mathf.Rad2Deg - 15.0f;  // 15 degrees for offset :D (Black magic)
            
            return distance;
        }

        private void OnMouseDown()
        {
            if (!Tile.TileManager.UIManager.HasMenuOpen)
            {
                Tile.TileManager.UIManager.ShowHandPanel(this);
            }
        }
    }
}