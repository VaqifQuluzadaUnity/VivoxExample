using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VivoxUnity;


public class UserChatController : MonoBehaviour
{
    //public GameObject ChatGridObjectParent;     //Bu object bizim yazdigimiz ve gonderilen textleri biryerde horizontal layout groupda saxlayan gameObjectdir.

    //public GameObject TextMessageVisualObject;  //Bu ise ekranda text-i gostereceyimiz objectdir. Her defe yeni mesaj gonderdikde ve ya qebul etdikde bu objecti instantiate edib, icindeki text componentini deyiseceyik.

    //[SerializeField] Sprite SelfMessageSprite;      //Bizim gonderdiyimiz messageler bu sprite-la gosterilecek.(backgroundda)

    //[SerializeField] Sprite OtherMessageSprite;     //Digerleri terefinden gonderilmis message ise bu spritela gosterilecek.

    //public Button SendMessageButton;            //Enter buttonundan elave Send message button-da istifade ede bilerik.

    //public TMP_InputField TextInputField;           //Bu ise bizim yazi yazdigimiz input field-dir.

    //ChannelId CurrentChannelId;                 //Hal-hazirda qosuldugumuz channelin Id-ni aliriq ki, mesajlari hemin Id-e gonderek.

    //VivoxVoiceManager VoiceManagerInstance;     //Bu bizim Vivox Voice managerin singleton-nun referencedir.

    //private List<GameObject> _messageObjPool = new List<GameObject>();      //Bu ise hal hazirda gonderdiyimiz mesajlarin siyahisidir.

    //private void Start()
    //{
    //    VoiceManagerInstance = VivoxVoiceManager.Instance;      //Evvelce instance-i aliriq.
    //    Debug.Log(VoiceManagerInstance.ActiveChannels.Count);   //Qosuldugumuz channelin olub olmadigini yoxlayiriq.
    //    if (VoiceManagerInstance.ActiveChannels.Count > 0)      //Eger hansisa channele qosulmusuqsa
    //    {
    //        CurrentChannelId = VoiceManagerInstance.ActiveChannels.FirstOrDefault().Key;//Hemin channelin ID-ni aliriq ki, mesajlari ora gonderek.
    //    }
    //    //VoiceManagerInstance.OnParticipantAddedEvent += OnParticipantAdded;             //Yeni istifadeci daxil olanda bu metodlari respond olaraq elave edirik.
    //    VoiceManagerInstance.OnTextMessageLogReceivedEvent += OnTextMessageLogReceivedEvent;
    //    SendMessageButton.onClick.AddListener(sendTextMessage);                         //Send button-a metod elave edirik.
    //}

    //void sendTextMessage()//Bu metodla mesaji gonderirik.
    //{
    //    if (string.IsNullOrEmpty(TextInputField.text))//Eger input field bosdursa mesaj gonderilmir.
    //    {
    //        return;
    //    }

    //    VoiceManagerInstance.SendTextMessage(TextInputField.text, CurrentChannelId);//Hazirki channele mesajimizi yollayiriq.
    //    TextInputField.text = "";//Ve Input field-imizi sifirlayiriq.
    //}


    //private void Update()//Burda ise entere basdiqda mesaji gondermeyi temin edirik.
    //{
    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        if (!string.IsNullOrEmpty(TextInputField.text))
    //        {
    //            sendTextMessage();
    //        }
    //    }
    //}


    ////void OnParticipantAdded(string username, ChannelId channel, IParticipant participant)//
    ////{
    ////    if (VoiceManagerInstance.ActiveChannels.Count > 0)
    ////    {
    ////        CurrentChannelId = VoiceManagerInstance.ActiveChannels.FirstOrDefault().Channel;
    ////    }
    ////}

    //private void OnTextMessageLogReceivedEvent(string sender, IChannelTextMessage channelTextMessage)//Bu metod vasitesile ise Vivox uzerinden messageleri qebul edirik ve otururuk.
    //{
    //    if (!String.IsNullOrEmpty(channelTextMessage.ApplicationStanzaNamespace))//Gelen mesajin bos olub olmadigini yoxlayiriq.Yuxarida sendMessage metodunda yoxlasaq da her ehtimala qarsi.
    //    {
    //        // If we find a message with an ApplicationStanzaNamespace we don't push that to the chat box.
    //        // Such messages denote opening/closing or requesting the open status of multiplayer matches.
    //        return;
    //    }

    //    GameObject newMessageObject = Instantiate(TextMessageVisualObject, ChatGridObjectParent.transform);//Yeni mesaj obyekti instantiate edib, parent objecte elave edirik ki, vertical layout qrupda yerlessin.
    //    _messageObjPool.Add(newMessageObject);                                 //Mesajlarin siyahisina bu yeni mesaji elave edirik.
       

    //    if (channelTextMessage.FromSelf)                                    //Eger mesaji biz gondermisikse
    //    {
    //        newMessageObject.GetComponent<Image>().sprite = SelfMessageSprite;
    //        newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = channelTextMessage.Message;
    //        newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = channelTextMessage.ReceivedTime.ToString();
    //    }
    //    else//Eger basqasi terefinden gonderilibse
    //    {
    //        newMessageObject.GetComponent<Image>().sprite = OtherMessageSprite;
    //        newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = channelTextMessage.Message;
    //        newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = sender + channelTextMessage.ReceivedTime;
    //    }
    //}


    [SerializeField] GameObject ChatGridParent;     //Mesajlari yazacagimiz Grid-in GameObjectidir.
    [Header("Text message objects and properties")]
    [SerializeField] GameObject TextMessagePrefab;  //Bu ise mesaji yazacagimiz GameObjectin prefabidir. Her mesaja gore bu prefabdan instantiate edeceyik.
    [SerializeField] Sprite SelfMessageSprite;      //Bizim gonderdiyimiz messageler bu sprite-la gosterilecek.(backgroundda)
    [SerializeField] Sprite OtherMessageSprite;     //Digerleri terefinden gonderilmis message ise bu spritela gosterilecek.
    [Header("Other properties")]
    [SerializeField] Button SendMessageButton;      //Bu button vasitesile mesaji gonderirik.
    [SerializeField] TMP_InputField MessageInputField;//Yazacagimiz yazini goturecek Input field ucun referencedir.
    [SerializeField] GameObject ErrorAndMessagePanel;   //Melumat oturmek ve ya error gostermek ucun bu panelden istifade edeceyik.
    [Header("Vivox Properties")]
    ChannelId CurrentConnectedChannelId;            //Hazirda qosuldugumuz channelin Id-ni temin edir.
    VivoxVoiceManager VoiceMgrInstance;
    private List<GameObject> allMessages = new List<GameObject>();  //Gonderdiyimiz butun mesajlari yerlestireceyimiz listdir.

    private void Start()    //Burda start-da bu kodlari yazdiq cunki ChannelPanelControllerde qosulma awake metodunda bas verir.
    {
        VoiceMgrInstance = VivoxVoiceManager.Instance;
        if (VoiceMgrInstance.ActiveChannels.Count > 0)
        {
            CurrentConnectedChannelId = VoiceMgrInstance.ActiveChannels.FirstOrDefault().Key;//Hemin channelin ID-ni aliriq ki, mesajlari ora gonderek.
            Debug.Log(VoiceMgrInstance.ActiveChannels.FirstOrDefault().Key);
        }

        //VivoxVoiceManager.Instance.OnParticipantAddedEvent += OnParticipantAdded;     //yeni User daxil olduqda onun haqqinda melumatlari gormek ucun bu eventdan istifade edeceyik.
        VoiceMgrInstance.OnTextMessageLogReceivedEvent += onTextMessageLogReceivedEvent;       //MEsaj qebul eden zaman ise dusen metodlara onTextMessageLogReceived-de elave edirik.
        SendMessageButton.onClick.AddListener(sendTextMessage);                                     //Send button-a ise SendTextMessage eventini elave edirik.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sendTextMessage();
        }
    }
    void sendTextMessage()
    {
        if (string.IsNullOrEmpty(MessageInputField.text))
        {
            return;
        }
        VoiceMgrInstance.SendTextMessage(MessageInputField.text, CurrentConnectedChannelId);  //hal hazirda qosuldugumuz channele mesaji gonderirik.BURA DIQQET:Burda text messageleri SendTextMessage metoundan istifade etdik.
        MessageInputField.text = "";    //Daha sonra Input field-i bos stringle evez edirik ki, yazini silmeli olmayaq.
    }

    private void onTextMessageLogReceivedEvent(string sender, IChannelTextMessage channelTextMessage)
    {
        if (!String.IsNullOrEmpty(channelTextMessage.ApplicationStanzaNamespace))
        {
            //Stanza namespace-i ile teyin olunmus mesajlar sadece multiplayer oyunlar haqqinda melumatlar ucun istifade edilir.
            //Buna gore de onlari chat boxda gondermirik.
            return;
        }

        GameObject newMessageObject = Instantiate(TextMessagePrefab, ChatGridParent.transform); //Yeni bir message gameObjecti yaradiriq.
        allMessages.Add(newMessageObject);                                                      //Ve bu message-i yazilan mesajlara elave edirik.
        if (channelTextMessage.FromSelf)                                                        //Mesaji ozumuz gondermisikse
        {
            newMessageObject.GetComponent<Image>().sprite = SelfMessageSprite;                  //Sprite olaraq oz gonderdiyimiz ferqlendirici sprite-i istifade edirik.
            newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = channelTextMessage.Message;       //Message-GameObjectindeki message text hissesine message i yaziriq.
            newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = channelTextMessage.ReceivedTime.ToString();       //Message GameObjectinin asagidaki info hissesine ise melumatlari elave edirik.
        }
        else
        {
            newMessageObject.GetComponent<Image>().sprite = OtherMessageSprite;                 //Eger mesaj basqasi terefinden gonderilibse other sprite-ni yerlestiririk.
            newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = channelTextMessage.Message;   //Digerleri ise eynidir sadece
            newMessageObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = sender + channelTextMessage.ReceivedTime; //melumat hissesinde gonderen adamin adini da qeyd edirik.

        }
    }
}
