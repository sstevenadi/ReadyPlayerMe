using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class WebAvatarCreate : MonoBehaviour
    {
        [Header("Game Button")] 
        public GameObject gameButton;

        [Header("Pop Up")] 
        public GameObject popUp;
        public GameObject loadingPopUp;

        [Header("WebView")]
        public WebView webView;

        [Header("Stats")] 
        public GameObject stats;
        public Transform cameraLook;
        
        // Avatar Url
        private GameObject m_Avatar;
        private string m_URL;
        private AvatarLoader m_AvatarLoader;

        private void Awake()
        {
            m_AvatarLoader = new AvatarLoader();
        }

        private void Start()
        {
            m_URL = PlayerPrefs.GetString("URL", "None");

            if (m_URL != "None")
            {
                loadingPopUp.SetActive(true);
                m_AvatarLoader.LoadAvatar(m_URL, null, OnAvatarImported);
            }
            else
            {
                popUp.SetActive(true);
            }
        }

        public void DisplayWebView()
        {
            if(webView == null)
            {
                webView = FindObjectOfType<WebView>();
            }
            else if (webView.Loaded)
            {
                webView.SetVisible(true);
            }
            else
            {
                webView.CreateWebView();
                webView.OnAvatarCreated = OnAvatarCreated;
            }
        }

        // WebView callback for retrieving avatar url
        private void OnAvatarCreated(string url)
        {

            webView.SetVisible(false);
            loadingPopUp.SetActive(true);
            popUp.SetActive(false);
            
            Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
            
            m_AvatarLoader.LoadAvatar(url, null, OnAvatarImported);
        }

        // AvatarLoader callback for retrieving loaded avatar game object
        private void OnAvatarImported(GameObject avatar, AvatarMetaData metaData)
        {
            Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
            
            // Set Avatar to newly created
            m_Avatar = avatar;
            
            var statsObject = Instantiate(stats, avatar.transform);
            statsObject.GetComponent<StatsLook>().lookAt = cameraLook.transform;
            statsObject.transform.position = new Vector3(0.05f, 2f, 0f);
            statsObject.transform.localScale = new Vector3(0.013f, 0.013f, 1);

            loadingPopUp.SetActive(false);
            gameButton.SetActive(true);
        }

        public void ChangeScene()
        {
            SceneManager.LoadScene("Playground");
        }

        public void DeletePlayer()
        {
            PlayerPrefs.DeleteKey("URL");
            Destroy(m_Avatar);
            gameButton.SetActive(false);
            popUp.SetActive(true);
        }
    }
}