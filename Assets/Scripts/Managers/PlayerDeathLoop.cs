using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeathLoop : MonoBehaviour
{
    public GameObject ExplosionPrefab;

    public void KillPlayer(PlayerMovement player)
    {
        Instantiate(this.ExplosionPrefab, player.gameObject.transform.position, player.gameObject.transform.rotation);
        GameObject hookObj = GameObject.FindGameObjectWithTag("Hook");
        Hook hook = hookObj.GetComponent<Hook>();
        hook.Breakaway();
        player.gameObject.SetActive(false);
    }

    public void RespawnPlayer(PlayerMovement player)
    {
        var respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = new Vector3(camera.transform.position.x, player.transform.position.y / 4, camera.transform.position.z);

        player.transform.position = respawnPoint.transform.position;
        player.gameObject.SetActive(true);
        player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
