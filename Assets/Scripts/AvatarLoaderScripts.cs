using System;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Wolf3D.ReadyPlayerMe.AvatarSDK;

public class AvatarLoaderScripts : MonoBehaviour
{
    //dummy
    private string avatarURL = "https://d1a370nemizbjq.cloudfront.net/d8fd8fa5-578b-4c11-8f37-8fd7be868340.glb";

    public GameObject stats;
    public GameObject loading;

    public Camera cam;
    public CinemachineVirtualCamera cvc;
    public RuntimeAnimatorController rac;
    public InputActionAsset iaa;
    public UICanvasControllerInput uicci;
    public MobileDisableAutoSwitchControls mdasc;
    public GameObject uigc;
    
    // Start is called before the first frame update

    private void Awake()
    {
        avatarURL = PlayerPrefs.GetString("URL");
    }

    void Start()
    {
        Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(avatarURL, OnAvatarImported, OnAvatarLoaded);
    }

    private void OnAvatarImported(GameObject avatar)
    {
        Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
        
        uigc.SetActive(true);
        SetupAvatar(avatar);
        loading.SetActive(false);
    }

    private void SetupAvatar(GameObject avatar)
    {
        // Add Camera
        GameObject cameraTarget = new GameObject("CameraTarget");
        cameraTarget.transform.parent = avatar.transform;
        cameraTarget.transform.localPosition = new Vector3(0, 1.5f, 0);
        cvc.Follow = cameraTarget.transform;
        
        // Set the animator
        Animator animator = avatar.GetComponent<Animator>();
        animator.runtimeAnimatorController = rac;
        animator.applyRootMotion = false;
        
        // Add tp controller and set values
        ThirdPersonController tpsController = avatar.AddComponent<ThirdPersonController>();
        tpsController.GroundedOffset = 0.1f;
        tpsController.GroundLayers = 1;
        tpsController.JumpTimeout = 0.5f;
        tpsController.CinemachineCameraTarget = cameraTarget;
        
        // Add character controller and set size
        CharacterController characterController = avatar.GetComponent<CharacterController>();
        characterController.center = new Vector3(0, 1, 0);
        characterController.radius = 0.3f;
        characterController.height = 1.9f;
        
        // Add player input and set actions asset
        PlayerInput playerInput = avatar.GetComponent<PlayerInput>();
        playerInput.actions = iaa;
        
        // Add components with default values
        avatar.AddComponent<BasicRigidBodyPush>();
        StarterAssetsInputs starterAssetsInputs = avatar.AddComponent<StarterAssetsInputs>();

        uicci.starterAssetsInputs = starterAssetsInputs;
        mdasc.playerInput = playerInput;

        var statsObject = Instantiate(stats, avatar.transform);
        statsObject.GetComponent<StatsLook>().lookAt = cam.transform;
    }
}
