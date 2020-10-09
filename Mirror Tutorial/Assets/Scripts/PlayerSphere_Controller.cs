using Mirror;
using UnityEngine;

public class PlayerSphere_Controller : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (hasAuthority == true)
        {
            if (Input.GetMouseButtonDown(0))
                this.transform.Translate(Vector3.forward * Time.deltaTime * 20f);
        }
    }
}
