using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private float stepLength = 0.25f;
    [SerializeField]
    private float speed = 0.05f;

    private bool isMove;
    private Vector3 direction, targetPos;
    private Transform pushedObject;

    void Start()
    {
        targetPos = player.position;
    }

    void Update()
    {
        if (isMove)
        {
            //入力下方向に進み続ける
            KeepMove();

            // 箱の前に壁や箱があるとき動きを止める
            if (targetPos == AlignPosition(player.position))
            {
                StopMove();
            }

            return;
        }

        HandleUserInput();
    }

    private void KeepMove()
    {
        // キャラと箱の動き
        player.position = Vector3.MoveTowards(player.position, targetPos, speed);

        // 箱が目の前にあれば押す
        if (pushedObject != null)
        {
            pushedObject.position = Vector3.MoveTowards(pushedObject.position,
                targetPos + direction * stepLength, speed);
        }
    }

    private void StopMove()
    {
        isMove = false;

        // 位置の初期化
        player.position = AlignPosition(player.position);

        if (pushedObject != null)
        {
            pushedObject.position = AlignPosition(pushedObject.position);
        }

        ScoreManager.Instance.AddScore(1);
        Debug.Log("動く" + ScoreManager.Instance.Score
            + "(ベストスコア " + ScoreManager.Instance.HighScore + ")");
    }

    private void HandleUserInput()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");

        if (vertical != 0)
        {
            direction = vertical > 0
                ? Vector3.up
                : Vector3.down;
        }
        else if (horizontal != 0)
        {
            direction = horizontal > 0
                ? Vector3.right
                : Vector3.left;
        }
        else
        {
            direction = Vector3.zero;
        }

        if (direction.magnitude != 0)
        {
            Move();
        }
    }

    private void Move()
    {
        // 進行方向の物体を探す
        Transform front = GetTransform(new Vector2(player.position.x + stepLength * direction.x,
            player.position.y + stepLength * direction.y));

        Transform nextToFront = GetTransform(new Vector2(player.position.x + stepLength * direction.x * 2f,
            player.position.y + stepLength * direction.y * 2f));

        if (MovementIsPosible(front, nextToFront))
        {
            isMove = true;

            pushedObject = TryGetPushedObject(front);

            targetPos = new Vector3(player.position.x + stepLength * direction.x,
                player.position.y + stepLength * direction.y, player.position.z);
        }
    }

    private bool MovementIsPosible(Transform front, Transform nextToFront)
    {

        return !(front == null                         // 正面には進めない

            || front.tag.CompareTo("Wall") == 0        // 外壁より先には進めない

            || front != null && front.tag.CompareTo("Box") == 0 && nextToFront
            != null && nextToFront.tag.CompareTo("Box") == 0      // 箱が2つ重なっているときは押せない

            || front != null && front.tag.CompareTo("Box") == 0 && nextToFront
            != null && nextToFront.tag.CompareTo("Wall") == 0);   // 箱を壁に押し込めない様にする

    }

    private Transform TryGetPushedObject(Transform front)
    {
        return (front != null && front.tag.CompareTo("Box") == 0)
            ? front
            : null;
    }

    private Transform GetTransform(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);

        return hit.collider != null
            ? hit.transform
            : null;
    }

    private Vector3 AlignPosition(Vector3 pos)
    {
        return new Vector3(
            AlignCoordinate(pos.x),
            AlignCoordinate(pos.y),
            AlignCoordinate(pos.z));
    }

    private float AlignCoordinate(float coord)
    {
        return Mathf.Round(coord * 100f) / 100f; ;
    }
}
