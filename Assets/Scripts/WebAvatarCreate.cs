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
        public WebView m_WebView;
        
        // Avatar Url
        private GameObject m_Avatar;
        private string m_URL;
        private AvatarLoader m_AvatarLoader = new AvatarLoader();

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
            if(m_WebView == null)
            {
                m_WebView = FindObjectOfType<WebView>();
            }
            else if (m_WebView.Loaded)
            {
                m_WebView.SetVisible(true);
            }
            else
            {
                m_WebView.CreateWebView();
                m_WebView.OnAvatarCreated = OnAvatarCreated;
            }
        }

        // WebView callback for retrieving avatar url
        private void OnAvatarCreated(string url)
        {

            m_WebView.SetVisible(false);
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