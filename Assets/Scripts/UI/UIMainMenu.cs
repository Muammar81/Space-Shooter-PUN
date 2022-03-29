using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using SpaceShooter.Events;

namespace SpaceShooter.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        private const string PLAYER_NAME = "PLAYER_NAME";
        [SerializeField] Button btnStartGame;
        [SerializeField] TMP_InputField txtPlayerName;

        private void OnEnable() => btnStartGame.onClick.AddListener(OnStartGame);
        private void OnDisable() => btnStartGame.onClick.RemoveListener(OnStartGame);

        private void Start()
        {
            var playerName = PlayerPrefs.GetString(PLAYER_NAME, Environment.UserName);
            txtPlayerName.text = playerName;
            //Debug.Log(txtPlayerName.text);
        }

        private void OnStartGame()
        {
            MenuEvents.OnStartGame?.Invoke(txtPlayerName.text);
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
