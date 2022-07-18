using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Pathfinding {
    /// <summary>
    /// Sets the destination of an AI to the position of a specified object.
    /// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
    /// This component will then make the AI move towards the <see cref="target"/> set on this component.
    ///
    /// See: <see cref="Pathfinding.IAstarAI.destination"/>
    ///
    /// [Open online documentation to see images]
    /// </summary>
    [UniqueComponent(tag = "ai.destination")]
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
    public class AIDestinationSetter : VersionedMonoBehaviour {
        /// <summary>The object that the AI should move to</summary>
        public Transform target;

        private Vector3 pos;
        IAstarAI ai;

        #region Custom Fields

        [SerializeField] private bool isBoss = false;
        private bool _rage = false;
        private bool isDashing = false;
        private int direction = 0;

        #endregion


        void OnEnable() {
            ai = GetComponent<IAstarAI>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            // Update the destination right before searching for a path as well.
            // This is enough in theory, but this script will also update the destination every
            // frame as the destination is used for debugging and may be used for other things by other
            // scripts as well. So it makes sense that it is up to date every frame.
            if (ai != null) ai.onSearchPath += Update;
        }

        void OnDisable() {
            if (ai != null) ai.onSearchPath -= Update;
        }

        public void Rage() {
            this._rage = true;
        }

        /// <summary>Updates the AI's destination every frame</summary>
        void Update() {
            if (target != null && ai != null) {
                if (isBoss) {
                    if (!_rage) {
                        // this block makes the enemy hover around the player
                        // a bit hacky but whatever
                        float randX = Random.Range(-5, 5f);
                        if (randX < 0) randX -= 2f;
                        else randX += 2f;

                        if (transform.position.x > target.position.x) {
                            pos = new Vector3(target.position.x + randX, target.position.y + Random.Range(3, 6f),
                                target.position.z);
                            ai.destination = pos;
                        } else {
                            pos = new Vector3(target.position.x - randX, target.position.y + Random.Range(1, 4f),
                                target.position.z);
                            ai.destination = pos;
                        }
                    } else if (isDashing) {
                        pos = new Vector2(target.position.x + direction * 10, target.position.y);
                        ai.destination = pos;
                    } else {
                        ai.destination = target.position;
                    }
                } else {
                    ai.destination = target.position;
                }
            }
        }

        public void Dash(int direction) {
            StartCoroutine(DashCoroutine(direction));
        }

        private IEnumerator DashCoroutine(int direction) {
            this.isDashing = true;
            this.direction = direction;

            yield return new WaitForSeconds(2f);

            this.isDashing = false;
        }
    }
}
