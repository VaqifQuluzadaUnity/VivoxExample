using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
/// <summary>
/// Main menuda Userin login emeliyyatlarini bu class vasitesile heyata kecireceyik.
/// </summary>
public class UserLoginController : MonoBehaviour
{
    [SerializeField] Button LoginButton;         //Bu button vasitesile InputField-a ad daxil etdikden sonra text chat-a daxil ola bileceyik.
    [SerializeField] TMP_InputField UserNameInputField; //Bu ise playerin adini daxil edeceyimiz input fielddir.
    [SerializeField] int ChatRoomSceneIndex;            //Bizim chat room scene-in indeksidir.

    VivoxVoiceManager VoiceMgrInstance;
    private void Awake()
    {
        VoiceMgrInstance = VivoxVoiceManager.Instance;
        LoginButton.interactable = false;          //Buttonu player ad daxil edene qeder sondururuk.
    }

    private void OnEnable()     //Script enable olduqda(yeni player aktiv olduqda)
    {
        VoiceMgrInstance.OnUserLoggedInEvent += onUserLoggedIn;           //Voice Manager-e bu onUserLoggedIn eventi-ni elave edirik ki, User Login eden zaman bu event ise dussun.
        UserNameInputField.onValueChanged.AddListener(onInputFieldChanged);         //InputField-da ad daxil eden zaman Login buttonun reaksiyasini temin etmek ucun onInputFieldChanged eventini elave edirik.
        LoginButton.onClick.AddListener(onLoginButtonClicked);                      //Login button-a ise basilan zaman ne edeceyini teyin etmek ucun onLoginButtonClicked eventini elave edirik.
    }

    private void OnDisable()   //Script disable olduqda ise yuxaridakilarin eksini edirik.
    {
        VoiceMgrInstance.OnUserLoggedInEvent -= onUserLoggedIn;
        UserNameInputField.onValueChanged.RemoveListener(onInputFieldChanged);
        LoginButton.onClick.RemoveListener(onLoginButtonClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))       //Eger istifadeci Entere basarsa
        {
            onLoginButtonClicked();
        }
    }

    private void onUserLoggedIn()//Bu metodu yuxarida onEnable metodunda VivoxVoiceManager-deki OnUserLoggedIn eventlarina elave edeceyik ki, user login eden zaman bu metod ise dussun.
    {
        SceneManager.LoadScene(ChatRoomSceneIndex);//User login eden zaman ChatRoom-a kecmeliyik.
    }

    private void onInputFieldChanged(string UserInput)      //Input fielda elave olunan string burdaki UserInput parametrine oturulecek.
    {
        LoginButton.interactable = !string.IsNullOrEmpty(UserInput);    //User input bos(yazili) olarsa loginButton ona uygun non-interactable(interactable) olacaq.
    }

    private void onLoginButtonClicked()     //Login buttonu onInputFieldChanged-de aktiv deaktiv edirik amma User Enter buttonuna basaraq da daxil olmaq isteyecek ona gore de string.isNullOrEmpty detection elave etmisik.
    {
        
        string userName = UserNameInputField.text;//Input fielddaki text-i aliriq.
        if (string.IsNullOrEmpty(userName))       //Eger input field bosdursa
        {
            return;                                 //hecne etmirik.
        }
            VivoxVoiceManager.Instance.Login(userName);//eger ad yazilibsa bu zaman login edirik.
    }
}
