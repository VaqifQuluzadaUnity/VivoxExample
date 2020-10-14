using UnityEngine;
using Photon.Pun;
public class PhotonGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject PlayerPrefab;//For instantiating the user's player we need this ref.
    void Start()
    {
        PhotonNetwork.Instantiate(PlayerPrefab.name, Vector3.zero, Quaternion.identity); //we spawn player in the server.
    }

    
}
