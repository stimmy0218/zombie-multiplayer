using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public GameObject spawnFx; 
    
    //랜덤 위치 소환
    void Start()
    {
        var initPos = Random.insideUnitSphere * 5f;
        initPos.y = 0;
        
        PhotonNetwork.Instantiate("Woman", initPos, Quaternion.identity);
    }

    void Update()
    {
        if(!photonView.IsMine)
            return;
        GameObject fx = Instantiate(spawnFx, transform.position, Quaternion.identity);
        Destroy(fx, 1f);
    }
    
}
