﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GlobalController : Singleton<GlobalController> {

    public GameObject ndsCanvas, graphy;

    public RenderTexture ndsTexture;
    public PlayerData[] characters;
    public Settings settings;
    public DiscordController discordController;
    public string controlsJson = null;

    public bool joinedAsSpectator = false;
    public DisconnectCause? disconnectCause = null;

    private int windowWidth, windowHeight;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void CreateInstance() {
        Instantiate(Resources.Load("Prefabs/Static/GlobalController"));
    }

    void Awake() {
        if (!InstanceCheck())
            return;
        Instance = this;
        settings = GetComponent<Settings>();
        discordController = GetComponent<DiscordController>();

        PhotonPeer.RegisterType(typeof(PlayerInput), (byte) 'i', PlayerInputs.Serialize, PlayerInputs.Deserialize);
    }

    void Start() {
        //Photon settings.
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.MaxResendsBeforeDisconnect = 15;

        InputSystem.controls.UI.DebugInfo.performed += (context) => {
            graphy.SetActive(!graphy.activeSelf);
        };
    }

    void Update() {
        //todo: move this somewhere else???
        ndsCanvas.SetActive(Settings.Instance.ndsResolution && SceneManager.GetActiveScene().buildIndex != 0);

        //todo: this jitters to hell
#if UNITY_STANDALONE
        int currentWidth = Screen.width;
        int currentHeight = Screen.height;
        if (Screen.fullScreenMode == FullScreenMode.Windowed && Keyboard.current[Key.LeftShift].isPressed && (windowWidth != currentWidth || windowHeight != currentHeight)) {
            currentHeight = (int) (currentWidth * (9f / 16f));
            Screen.SetResolution(currentWidth, currentHeight, FullScreenMode.Windowed);
        }
        windowWidth = currentWidth;
        windowHeight = currentHeight;
#endif
    }
}
