using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter.Events;

namespace SpaceShooter.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] Button btnStartGame;
        [SerializeField] TMP_InputField txtPlayerName;

        private const string PLAYER_NAME = "PLAYER_NAME";

        private void OnEnable() => btnStartGame.onClick.AddListener(OnStartGame);
        private void OnDisable() => btnStartGame.onClick.RemoveListener(OnStartGame);

        private void OnStartGame()
        {
            MenuEvents.OnStartGame?.Invoke(txtPlayerName.text);
        }


        private void Start()
        {
            var playerName = PlayerPrefs.GetString(PLAYER_NAME, System.Environment.UserName);
            txtPlayerName.text = playerName;
        }
    }
}

namespace SpaceShooter.Events
{
    public class MenuEvents
    {
        public static Action<string> OnStartGame = delegate { };
    }
}