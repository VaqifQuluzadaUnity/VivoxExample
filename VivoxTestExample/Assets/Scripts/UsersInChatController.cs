using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using VivoxUnity;

/// <summary>
/// Using this class we can control and see the data of the users(their nickname, profile picture and etc) in the current channel.
/// We will place this script in Content GameObject of Scroll view object.
/// </summary>
public class UsersInChatController : MonoBehaviour
{
    #region Private Variables

    private const string LobbyChannelNameTemp = "General";          //This is the temp name for our current channel then we add a event whether channel changed will be triggered and change the Users in Chat.

    private VivoxVoiceManager _voiceManagerInstance;            //Reference for Voice Manager

    private Dictionary<ChannelId, List<UserChatIdentifier>> UserChatIdentDict = new Dictionary<ChannelId, List<UserChatIdentifier>>();  //Dictionary that contains channels and list of users of this each channel

    #endregion


    #region Serialized Variables

    [SerializeField] GameObject UserChatIdentPrefab;                //We will instantiate this and set the user's image, mute , unmute state and profile pic over this UI element.

    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        _voiceManagerInstance = VivoxVoiceManager.Instance;     //Decleration of reference of Voice manager

        _voiceManagerInstance.OnParticipantAddedEvent += onParticipantAdded;        //When an user added this event will be triggered in all users
        _voiceManagerInstance.OnParticipantRemovedEvent += onParticipantRemoved;    //When an user removed this event will be triggered in all users
        //_voiceManagerInstance.OnUserLoggedOutEvent += onUserLoggedOut;              //When an user logged out this event will be triggered in all users(we can add this if we have login ,chat menu in one scene)
        if (_voiceManagerInstance && _voiceManagerInstance.ActiveChannels.Count > 0)    //if voice manager reference exists and we have active channels
        {
            IChannelSession currentChannelSession = _voiceManagerInstance.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == LobbyChannelNameTemp);     //We get the channel session of our current active channel
            foreach(IParticipant userInChannel in _voiceManagerInstance.LoginSession.GetChannelSession(currentChannelSession.Channel).Participants)         //and in this channel for all users
            {
                UpdateUserIdentDictionary(userInChannel,userInChannel.ParentChannelSession.Channel,true);                                                   //We set visual UserChat identifiers
            }
        }
    }

    private void OnDisable()        //when any error happened or connection lost, for preventing any problem we remove events from our voiceManager
    {
        _voiceManagerInstance.OnParticipantAddedEvent -= onParticipantAdded;
        _voiceManagerInstance.OnParticipantRemovedEvent -= onParticipantRemoved;
        //_voiceManagerInstance.OnUserLoggedOutEvent -= onUserLoggedOut;            //we can add this if we have login ,chat menu in a single scene
    }
    #endregion


    #region Public Methods
    public void CleanChannelUsersVisually(ChannelId channel)        //In our scroll view using this method we reset the positions of user chat identifiers in current connected channel
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, UserChatIdentDict[channel].Count * 50);
    }

    //public void ClearAllUserIdentifiers()
    //{
    //    foreach(List<UserChatIdentifier> userList in UserChatIdentDict.Values)
    //    {
    //        foreach(UserChatIdentifier chatIdent in userList)
    //        {
    //            Destroy(chatIdent.gameObject);
    //        }
    //        userList.Clear();
    //    }
    //    UserChatIdentDict.Clear();
    //}        
/* This method is depend upon the onUserLoggedOut event*/
#endregion


#region Private Methods

private void UpdateUserIdentDictionary(IParticipant participant, ChannelId channel, bool isNewUser)
{
    if (isNewUser)      //we check if the user is new user that is connected to channel(we set isNewUser value in onParticipantAdded event)
    {
        GameObject newUserIdentObject = Instantiate(UserChatIdentPrefab, gameObject.transform);     //Create a new visual User Chat Identifier
        UserChatIdentifier newUserIdent = newUserIdentObject.GetComponent<UserChatIdentifier>();    //get the UserChatIdentifier component from GameObject
        List<UserChatIdentifier> userChannelList;                                                   //That is the list where we add new user. decleration depends upon following situtations...

        if (UserChatIdentDict.ContainsKey(channel))                                                 //If there is current connected channel name in our dictionary
        {
            UserChatIdentDict.TryGetValue(channel, out userChannelList);                            //Get the list of users from this channel and declare userChannelList as them
            newUserIdent.SetupUserChatIdentifier(participant);                                      //Setup participant visually
            userChannelList.Add(newUserIdent);                                                      //Add this participant to the list
            UserChatIdentDict[channel] = userChannelList;                                           //Set this list as a key of channel in dict
        }
        else                                                                                        //if there is no channel name for the given parameter
        {
            userChannelList = new List<UserChatIdentifier>();                                       //Create a new list
            newUserIdent.SetupUserChatIdentifier(participant);                                      //Setup the user
            userChannelList.Add(newUserIdent);                                                      //add this user to new list
            UserChatIdentDict.Add(channel, userChannelList);                                        //And create the value(list) to the current channel in Dictionary
        }
        CleanChannelUsersVisually(channel);                                                         //We set the users in correct positions
    }

    else// if user left the channel
    {
        if (UserChatIdentDict.ContainsKey(channel))         //if there is a channel in our dictionary
        {
            UserChatIdentifier RemovedUser = UserChatIdentDict[channel].FirstOrDefault(predicate => predicate.Participant.Account.Name == participant.Account.Name);        //We find the name of user in list that is the value of current channel
            if (RemovedUser != null)            //if user exists
            {
                UserChatIdentDict[channel].Remove(RemovedUser);     //Remove it from dictionary channel
                Destroy(RemovedUser.gameObject);                    //Destroy the visual User Chat Identifier of user
                CleanChannelUsersVisually(channel);                 //Reset the UI
            }
            else                                                    //If some error happens
            {
                Debug.LogError("There is no channelIdentifier of this user");
            }
        }
    }
}

#endregion

#region Vivox Custom Callbacks
void onParticipantAdded(string userName,ChannelId channel, IParticipant participant)        //When participant join the channel this event will be triggered by VivoxVoiceManager
{
    UpdateUserIdentDictionary(participant, channel, true);
}

void onParticipantRemoved(string userName, ChannelId channel, IParticipant participant)     //When participant left the channel this event will be triggered by VivoxVoiceManager
    {
    UpdateUserIdentDictionary(participant, channel, false);
}
#endregion
}
