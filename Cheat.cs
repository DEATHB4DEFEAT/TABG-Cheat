using System;
using UnityEngine;

namespace ExampleAssembly
{
    public class Cheat : MonoBehaviour
    {
        private readonly int _mainWid = 1024;
        private Rect _mainWRect = new Rect(5f, 5f, 300f, 150f);

        private bool _drawMenu;
        private bool _magicBullet;
        private bool _spaceFly;
        private bool _godMode;

        private float _lastCacheTime = Time.time + 5f;
        private float _lastItemCache = Time.time + 1f;

        public static Player[] Players;
        // public static Item[] droppedItems;
        // public static RiderHolder[] vehicles;


        public void KeyHandler()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _drawMenu = !_drawMenu;
            }

            if (Input.GetKey(KeyCode.Mouse0) && _magicBullet)
            {
                if (Players.Length > 0)
                {
                    foreach (var player in Players)
                    {
                        if (player != null && player != Player.localPlayer)
                        {
                            foreach (var proj in FindObjectsOfType<ProjectileHit>())
                            {
                                // player.GetComponent<HealthHandler>().TakeDamage(1000f, proj.transform.position);
                                proj.damage = 1000f;
                                proj.force = 0.2f;
                                proj.transform.position = player.transform.position;

                                Console.WriteLine($"proj.name: {proj.name}");
                                Console.WriteLine($"proj.damage: {proj.damage}");
                                Console.WriteLine($"proj.transform.position: {proj.transform.position}");
                                Console.WriteLine($"player.transform.position: {player.transform.position}");
                            }
                        }
                    }
                }
            }

            if (Input.GetKey(KeyCode.Space) && _spaceFly && Player.localPlayer != null)
            {
                var player = Player.localPlayer;
                var cam = Camera.main;
                var camTransform = cam.transform;

                var mousePos = Input.mousePosition;
                var worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
                var dir = (worldPos - camTransform.position).normalized;

                player.transform.position += dir * (Time.deltaTime * 15f);
            }
        }

        public void SuperGun(Weapon weapon)
        {
            //
        }

        public void Update() {
            KeyHandler();

            // if (_godMode && Player.localPlayer != null) {
            //     Player.localPlayer.data.defaultJumps = 1000;
            //     Player.localPlayer.stats.regenerationAdd = 3000f;
            //     Player.localPlayer.GetComponent<PlayerHealth>().health = 1000f;
            //     Player.localPlayer.GetComponent<PlayerHealth>().maxHealth = 1000f;
            //     Player.localPlayer.GetComponent<HealthHandler>().SetInvulnerable(true);
            // }

            if (Time.time >= _lastCacheTime) {
                _lastCacheTime = Time.time + 5f;

                Players = FindObjectsOfType<Player>();
                // vehicles = FindObjectsOfType<RiderHolder>();

                Esp.MainCam = Camera.main;
            }

            // if (Time.time >= _lastItemCache) {
            //     _lastItemCache = Time.time + 1f;
            //
            //     droppedItems = FindObjectsOfType<Item>();
            // }
        }

        public void OnGUI() {
            if (_drawMenu) {
                _mainWRect = GUILayout.Window(_mainWid, _mainWRect, MainWindow, "Main");
            }
        }

        private void MainWindow(int id) {
            GUILayout.BeginHorizontal();
            {
                _magicBullet = GUILayout.Toggle(_magicBullet, "Magic Bullet");
                _godMode = GUILayout.Toggle(_godMode, "GodMode");
            }
            GUILayout.EndHorizontal();

            if (Player.localPlayer != null) {
                // GUILayout.BeginHorizontal();
                // {
                //     GUILayout.Label($"Added Speed {Mathf.Floor(Player.localPlayer.data.move.movememtForce)}");
                //     Player.localPlayer.data.move.movememtForce = GUILayout.HorizontalSlider(Player.localPlayer.data.move.movememtForce, 0f, 30f);
                // }
                // GUILayout.EndHorizontal();

                if (GUILayout.Button("SuperGuns")) {
                    var rightGun = Player.localPlayer.GetComponentInChildren<WeaponHandler>().rightWeapon;
                    var leftGun = Player.localPlayer.GetComponentInChildren<WeaponHandler>().leftWeapon;

                    if (rightGun)
                        SuperGun(rightGun);

                    if (leftGun)
                        SuperGun(leftGun);
                }
            }

            GUILayout.Space(20f);

            GUILayout.BeginVertical("ESP", GUI.skin.box);
            {
                GUILayout.Space(20f);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        Esp.Chams = GUILayout.Toggle(Esp.Chams, "Chams");
                        Esp.PlayerName = GUILayout.Toggle(Esp.PlayerName, "Player Name");
                        Esp.PlayerBox = GUILayout.Toggle(Esp.PlayerBox, "Player Box");
                        Esp.HealthBars = GUILayout.Toggle(Esp.HealthBars, "Player Health");
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    {
                        Esp.Crosshair = GUILayout.Toggle(Esp.Crosshair, "Crosshair");
                        _spaceFly = GUILayout.Toggle(_spaceFly, "Space Fly");
                        Esp.Item = GUILayout.Toggle(Esp.Item, "Item");
                        Esp.Vehicle = GUILayout.Toggle(Esp.Vehicle, "Vehicle");
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUI.DragWindow();
        }
    }
}
