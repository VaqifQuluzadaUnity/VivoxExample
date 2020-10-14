using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomSetupIdentifier : MonoBehaviour
{
    #region Private Serialized Variables
    [Header("UI element components")]
    [SerializeField] TextMeshProUGUI RoomNameField;
    [SerializeField] TextMeshProUGUI RoomMaxVsCurrentUsers;
    [SerializeField] Image RoomStatusImage;
    [SerializeField] Button RoomJoinButton;

    [Header("Room status Images")]
    [SerializeField] Sprite PrivateRoomImage;
    [SerializeField] Sprite PublicRoomImage;

    #endregion


    public void SetupRoomIdentifier(RoomInfo roomInfo)
    {
        RoomNameField.text = roomInfo.Name;
        RoomMaxVsCurrentUsers.text = roomInfo.MaxPlayers + "/" + roomInfo.PlayerCount;
        RoomStatusImage.sprite = roomInfo.IsVisible ? PublicRoomImage : PrivateRoomImage;
        RoomJoinButton.onClick.AddListener(onJoinRoom);
    }


    private void onJoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomNameField.text);
    }
}
