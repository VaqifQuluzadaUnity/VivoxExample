using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Photon.Realtime;

public class PhotonLoginScript : MonoBehaviourPunCallbacks              //We used this class as we wanna override OnConnectedToMaster,OnJoinedRoom,and OnRandomFailed events.
{
    [Header("UI element references")]
    [SerializeField] Button loginButton;                    //our Login button.We add additional events for connecting server.
    [SerializeField] TMP_InputField UserNameInputField;     //If we havent connected to Master server we set input field non-interactable.
    [SerializeField] TextMeshProUGUI InfoFieldText;         //We gave info about state of server using this text element.

    [Header("Photon Room Properties")]
    [SerializeField] string RoomName;                       //Default room name. Then we can expand functionality of this variable.
    [SerializeField] string DefaultSceneName;               //The default scene which is loaded when we connected to Room.
    [SerializeField] byte MaxPlayerNum = 4;                 //The max number of players in the room
    [SerializeField] bool isOpenValue = true;               //This two variables are visibility and accessible variables of Room.
    [SerializeField] bool isVisibleValue = true;

    private void Start()
    {
        UserNameInputField.interactable = false;        //we set input field interactibility force till we join the master server.
        StartCoroutine(InfoTextFieldDelay("Connecting to master server"));      //Give info about state.
        PhotonNetwork.ConnectUsingSettings();           //Try to connect master server.
        
    }

    public override void OnConnectedToMaster()//When we connect to master server this event occured.(Note: there is a known issue in Photon generally about Joining Room-JoinRoom Failed-it will be fixed)
    {
        StartCoroutine(InfoTextFieldDelay("Successfully Connected to master server"));      //Give info about state.
        PhotonNetwork.AutomaticallySyncScene=true;                                          //If master client load any level,others clients must be synchronized and load that level so we set this variable true.
        UserNameInputField.interactable = true;                                             //We set InputField Interactable.
        loginButton.onClick.AddListener(onJoinRandomRoom);                                  //Add the event for loginButton.
    }



    private void onJoinRandomRoom()//For Joining Random Created Room we need to set this event in login Button.
    {
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinRandomFailed(short returnCode, string message)       //If there is no room in the server this event will be called.
    {
        StartCoroutine(InfoTextFieldDelay("There is no room in server"));
        StartCoroutine(InfoTextFieldDelay("Creating a new room..."));
        RoomOptions newRoomOptions = new RoomOptions {MaxPlayers =MaxPlayerNum,IsOpen=isOpenValue,IsVisible=isVisibleValue};//For creating new room we set room properties.
        PhotonNetwork.CreateRoom(RoomName, newRoomOptions);
        StartCoroutine("New room successfully created...");
        StartCoroutine(InfoTextFieldDelay("Joining Room"));
        PhotonNetwork.NickName = UserNameInputField.text;       //Set our Photon user nickName.
        PhotonNetwork.JoinRoom(DefaultSceneName);               //Then join this room.
    }



    public override void OnJoinedRoom()//When we joined any room we need to load room scene.
    {
        PhotonNetwork.LoadLevel("DefaultRoom");
    }
    IEnumerator InfoTextFieldDelay(string message)//This coroutine is used for giving info to other users.
    {
        InfoFieldText.text = message;

        yield return new WaitForSeconds(1);

        InfoFieldText.text = "";
    }


}
