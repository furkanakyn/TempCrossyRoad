using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera mainCamera;
    public float characterMoveDuration;

    public GameDirector gameDirector;

    bool isCharacterMoving;
    bool isCrushed = false;

    private Vector3 initialPoint;
    private Vector3 endPoint;

    public LayerMask layerMask;
    public LayerMask treeLayerMask;

    private int playerScore;

    public CarDetector carDetector;

    public ParticleSystem jumpPS;


    public void ResetPlayer()
    {
        Invoke(nameof(RemoveMotionLocks), .5f);
        GetComponent<BoxCollider>().enabled = true;
        transform.DOKill();
        transform.position = new Vector3(14, 0, 3);
        transform.localScale = new Vector3(.3f, .4f, .2f);
        transform.LookAt(transform.position + Vector3.forward);
        playerScore = 0;
        gameDirector.UpdatePlayerScore(playerScore);
    }
    public void RemoveMotionLocks()
    {
        isCharacterMoving = false;
        isCrushed = false;
    }


    void Update()
    {

        if (isCharacterMoving || isCrushed)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveForward();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveBack();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            {
                initialPoint = hit.point;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            {
                endPoint = hit.point;
            }
            if ((endPoint - initialPoint).magnitude < .1f)
            {
                MoveCharacter(transform.forward);
                return;
            }
            MoveCharacter(endPoint - initialPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            PlayerFailed();
        }
        if (other.CompareTag("Coin"))
        {
            CollectCoin();
            other.gameObject.SetActive(false);
            gameDirector.audioManager.PlayPositiveSound();
            gameDirector.fxManager.CollectCoinParticles(other.transform.position);
        }
    }
    private void CollectCoin()
    {
        gameDirector.coinUI.coinTMP.gameObject.SetActive(true);
        gameDirector.coinUI.CoinBounceAnimation();
        gameDirector.coinManager.EarnCoins(1);
        gameDirector.coinUI.UpdateCoinCount();
    }

    private void PlayerFailed()
    {
        GetComponent<BoxCollider>().enabled = false;
        carDetector.gameObject.SetActive(false);
        gameDirector.audioManager.PlayDeadSound();
        isCrushed = true;
        transform.DOKill();
        transform.DOScaleY(.05f, .1f);
        transform.DOScaleX(0.3f, .1f);
        transform.DOScaleZ(0.3f, .1f);
        transform.DOMoveY(0, .1f);
        gameDirector.failUI.Show();
        gameDirector.coinUI.Hide();
        
    }

    void MoveCharacter(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x < 0)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
        else
        {
            if (direction.z < 0)
            {
                MoveBack();
            }
            else
            {
                MoveForward();
            }
        }
    }

   

    void FinishCharacterMovement()
    {
        isCharacterMoving = false;
    }

    private void MoveLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.left, out hit, 1, treeLayerMask))
        {
            return;
        }
        transform.DOLocalMoveX(transform.position.x - 1, characterMoveDuration).OnComplete(DoSquashAnimation);
        DoJumpAnimation();
        transform.LookAt(transform.position + Vector3.left);
        isCharacterMoving = true;
    }

    private void MoveRight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 1, treeLayerMask))
        {
            return;
        }
        transform.DOLocalMoveX(transform.position.x + 1, characterMoveDuration).OnComplete(DoSquashAnimation);
        isCharacterMoving = true;
        transform.LookAt(transform.position + Vector3.right);
        DoJumpAnimation();
    }
    private void MoveBack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1, treeLayerMask))
        {
            return;
        }
        transform.DOLocalMoveZ(transform.position.z - 1, characterMoveDuration).OnComplete(DoSquashAnimation);
        isCharacterMoving = true;
        transform.LookAt(transform.position + Vector3.back);
        DoJumpAnimation();
    }

    private void MoveForward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1, treeLayerMask))
        {
            return;
        }
        transform.DOLocalMoveZ(transform.position.z + 1, characterMoveDuration).OnComplete(DoSquashAnimation);
        isCharacterMoving = true;
        transform.LookAt(transform.position + Vector3.forward);
        DoJumpAnimation();

        if (playerScore < transform.position.z - 2)
        {
            playerScore = Mathf.RoundToInt(transform.position.z - 2);
            gameDirector.UpdatePlayerScore(playerScore);
        }
        if (gameDirector.mapGenerator.lastRowCount - transform.position.z <= 20)
        {
            gameDirector.mapGenerator.AddNewRows(10);
            gameDirector.DeleteRow(10);
        }
    }
    void DoSquashAnimation()
    {
        transform.DOScaleY(.6f, .06f).SetLoops(2, LoopType.Yoyo).OnComplete(FinishCharacterMovement);
        jumpPS.Play();
    }

    private void DoJumpAnimation()
    {
        gameDirector.audioManager.PlayJumpSound();
        transform.DOMoveY(1, characterMoveDuration * .5f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleY(.7f, characterMoveDuration * .5f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleX(.4f, characterMoveDuration * .5f).SetLoops(2, LoopType.Yoyo);
        transform.DOScaleZ(.3f, characterMoveDuration * .5f).SetLoops(2, LoopType.Yoyo);
    }


}
