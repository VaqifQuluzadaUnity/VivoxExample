using Photon.Pun;
using UnityEngine;
using VivoxUnity;

public class PhotonVivoxPlayerSetup : MonoBehaviourPunCallbacks//We extend this class as we need to use isMine bool variable in Photon
{
    #region Private&Serialized Variables
    private VivoxVoiceManager _vivoxVoiceManager;       //The reference to the instance of voice manager.

    private IParticipant _currentParticipant;           //reference to the current connected participant 

    private PositionalVoice _playerPositionalVoice;     //reference to the currently connected PositionalVoice

    [SerializeField] string PositionalChannelName = "Default";      //This is temporary for development purpose only. We can then expand this variable.

    #endregion

    #region Public Variables
    public int AudibleDistance = 32;                //From this distance listener can hear speaking player and receive text message. Default value is 32. 
    public int ConversationalDistance = 1;          //Below this distance the speaking player's voice is clear, but passing this distance the voice begins to fade.
    public float AudioFadeIntensityByDistance = 1.0f;       //That is the intensity of fAudioFade. This multiplies the fading while passing a unit of Conversational Distance.
    public AudioFadeModel audioFadeModel = AudioFadeModel.InverseByDistance;    //That is how the fade operation applied.
    #endregion

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        _vivoxVoiceManager = VivoxVoiceManager.Instance;

        _playerPositionalVoice = GetComponent<PositionalVoice>();

        _vivoxVoiceManager.OnUserLoggedInEvent += onUserLoggedIn;

        _vivoxVoiceManager.OnParticipantAddedEvent += onParticipantAdded;
    }


    public void onUserLoggedIn()
    {
        _vivoxVoiceManager.DisconnectAllChannels();//This is test-only purpose. Then we can connect multiple positional channels.
        Channel3DProperties ChannelProperties = new Channel3DProperties(AudibleDistance, ConversationalDistance, AudioFadeIntensityByDistance, audioFadeModel);// We set the channelProperties according to variables above
        _vivoxVoiceManager.JoinChannel(PositionalChannelName, ChannelType.Positional, VivoxVoiceManager.ChatCapability.TextAndAudio, true, ChannelProperties);//And join this channel.
    }

    public void onParticipantAdded(string username,ChannelId channel,IParticipant participant)
    {
        _currentParticipant = participant;      //We set this variable for future purposes.
        _playerPositionalVoice.Participant = participant;   //We set the participant and PositionalGameObject with declaring this variable.
    }
}
