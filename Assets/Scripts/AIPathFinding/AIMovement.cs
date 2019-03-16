using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed = 1f;
    Vector3 myTransform;
    Vector3 tartgetPosition;
    Vector3 nextTargetPosition;
    Vector3 originalTargetPosition;

    bool canChange = true;

    BoxCollider2D myCollider;
    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myTransform = transform.position;
    }
    void Start()
    {
        tartgetPosition = transform.position;
        var pathFinder = FindObjectOfType<PathFinder_BFS>();
        var AiPath = pathFinder.GetPath();
        StartCoroutine(FollowPath(AiPath));
    }

    private void Update()
    {
        myTransform = transform.position;
    }

    IEnumerator FollowPath(List<Node> myPath)
    {
        foreach (var node in myPath)
        {
            tartgetPosition = node.transform.position;
            if (myPath.Count == myPath.IndexOf(node) + 1)
            {
                nextTargetPosition = tartgetPosition;
            }
            else
            {
                nextTargetPosition = myPath[myPath.IndexOf(node) + 1].transform.position;
            }
            originalTargetPosition = tartgetPosition;
            bool reachGoal = false;
            while (!reachGoal)
            {
                transform.position = Vector3.MoveTowards(transform.position, tartgetPosition, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                if (transform.position == tartgetPosition)
                {
                    canChange = true;
                    if (tartgetPosition == originalTargetPosition)
                    {
                        reachGoal = true;
                    }
                    else if (originalTargetPosition == nextTargetPosition)
                    {
                        tartgetPosition = nextTargetPosition;
                    }
                    else
                    {
                        tartgetPosition = nextTargetPosition;
                        originalTargetPosition = tartgetPosition;
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canChange)
        {
            ChangeTargetPosition();
            StartCoroutine(TurnOffCollider2d());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Block")
        {
            myCollider.isTrigger = false;
            canChange = true;
        }
    }

    private void ChangeTargetPosition()
    {
        var newXPos = UnityEngine.Random.Range(myTransform.x - 0.7f, myTransform.x + 0.7f);
        var newYPos = UnityEngine.Random.Range(myTransform.y - 0.7f, myTransform.y + 0.7f);
        tartgetPosition = new Vector3(newXPos, newYPos, 0);
    }

    IEnumerator TurnOffCollider2d()
    {
        canChange = false;
        myCollider.isTrigger = true;
        yield return new WaitUntil(() => canChange);
        myCollider.isTrigger = false;
    }
}
