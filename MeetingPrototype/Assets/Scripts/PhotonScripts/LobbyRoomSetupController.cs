using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class LobbyRoomSetupController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField] GameObject RoomParentGameObject;       //Content GameObject of Scroll View

    [SerializeField] GameObject RoomElementPrefab;          //The Visual prefab that shows RoomData

    [SerializeField] GameObject LoginPanel;                 //Login Panel for enable/disabling

    private List<GameObject> RoomVisualGameObjects = new List<GameObject>();

    public override void OnJoinedLobby()
    {
        LoginPanel.SetActive(false);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+" joined to lobby: " + PhotonNetwork.CurrentLobby.Name);
    }

    public override void OnLeftLobby()
    {
        throw new System.NotImplementedException();
    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        //
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count != 0)                //if there are rooms in the server
        {
            foreach (GameObject roomGameObject in RoomVisualGameObjects)
            {
                Destroy(roomGameObject);
            }
            RoomVisualGameObjects.Clear();


            foreach (RoomInfo roomInfo in roomList)
            {
                GameObject RoomElementInstance = Instantiate(RoomElementPrefab, gameObject.transform);
                RoomElementPrefab.GetComponent<RoomSetupIdentifier>().SetupRoomIdentifier(roomInfo);
                RoomVisualGameObjects.Add(RoomElementPrefab);
            }
        }
        
    }
}
