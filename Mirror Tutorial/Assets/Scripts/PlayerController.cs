using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject PlayerSphere, BallPrefab;
    [HideInInspector]
    public GameObject Ball;
    public GameObject Camera;
    [SyncVar][HideInInspector]
    public bool Ball_is_attached = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
            Camera.SetActive(true);
        else
            Camera.SetActive(false);

        if (Ball == null)
            Ball = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer == true)
        {
            if (Input.GetKey(KeyCode.D))
                this.transform.Translate(Vector3.right * Time.deltaTime * 3f);
            else if (Input.GetKey(KeyCode.W))
                this.transform.Translate(Vector3.forward * Time.deltaTime * 3f);
            else if (Input.GetKey(KeyCode.A))
                this.transform.Translate(Vector3.left * Time.deltaTime * 3f);
            else if (Input.GetKey(KeyCode.S))
                this.transform.Translate(Vector3.back * Time.deltaTime * 3f);
            else if (Input.GetKeyDown(KeyCode.I))
                CmdSpawn();
            else if (Input.GetKeyDown(KeyCode.P))
                CmdGrabTheBall();
        }

        if (Ball_is_attached == true)
            Ball.transform.position = this.gameObject.transform.localPosition + new Vector3(0, 0.5f, 0)  + Vector3.forward;
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject sphere = (GameObject)Instantiate(PlayerSphere, this.gameObject.transform.position + PlayerSphere.transform.position, Quaternion.identity);
        NetworkServer.Spawn(sphere, connectionToClient);
    }

    [Command]
    public void CmdGrabTheBall()
    {
        if (Ball_is_attached == false && Ball.GetComponent<Ball_Controller>().BallIsAttached == false)
        {
            Ball.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            Ball.transform.position = this.gameObject.transform.localPosition + new Vector3(0, 0.5f, 0) + Vector3.forward;
            Ball_is_attached = true;
            Ball.GetComponent<Ball_Controller>().BallIsAttached = true;
            CmdUpdateBallIsAttached(Ball_is_attached, Ball.GetComponent<Ball_Controller>().BallIsAttached);
        }
        else if (Ball_is_attached == true && Ball.GetComponent<Ball_Controller>().BallIsAttached == true)
        {
            Ball.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            Ball_is_attached = false;
            Ball.GetComponent<Ball_Controller>().BallIsAttached = false;
            CmdUpdateBallIsAttached(Ball_is_attached, Ball.GetComponent<Ball_Controller>().BallIsAttached);
        }
    }

    [Command]
    public void CmdUpdateBallIsAttached(bool value1, bool value2)
    {
        Ball_is_attached = value1;
        Ball.GetComponent<Ball_Controller>().BallIsAttached = value2;
    }
}
