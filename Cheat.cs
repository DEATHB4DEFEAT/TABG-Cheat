using RootMotion.FinalIK;
using UnityEngine;

namespace ExampleAssembly
{
    public class Cheat : MonoBehaviour
    {
        private readonly int _mainWid = 1024;
        private Rect _mainWRect = new Rect(5f, 5f, 300f, 150f);

        private bool _magicBullet;
        private bool _godMode;
        private bool _drawMenu = true;

        private float _lastCacheTime = Time.time + 5f;
        private float _lastItemCache = Time.time + 1f;

        public static Player[] Players;
        // public static Item[] droppedItems;
        // public static RiderHolder[] vehicles;


        public void KeyHandler()
        {
            if (Input.GetKeyDown(KeyCode.Insert)) {
                _drawMenu = !_drawMenu;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && _magicBullet) {
                if (Players.Length > 0) {
                    foreach (Player player in Players) {
                        if (player != Player.localPlayer && player != null) {
                            foreach (ProjectileHit proj in FindObjectsOfType<ProjectileHit>()) {
                                player.GetComponent<HealthHandler>().TakeDamage(1000f, proj.transform.position);
                            }
                        }
                    }
                }
            }
        }

        public void SuperGun(ref Weapon gun)
        {
            gun.attackSpeedM = 100f;
            gun.auto = true;
            gun.internalCooldown = 0f;
            gun.maxAngle = 0f;
            gun.maxRange = 10000f;
            gun.m_weaponType = Weapon.WeaponType.Magic;

            Destroy(gun.GetComponent<Recoil>());
        }

        public void Update() {
            KeyHandler();

            if (_godMode) {
                if (Player.localPlayer != null) {
                    Player.localPlayer.data.defaultJumps = 1000;
                    // Player.localPlayer.stats.regenerationAdd = 3000f;
                    Player.localPlayer.GetComponent<PlayerHealth>().health = 1000f;
                    Player.localPlayer.GetComponent<PlayerHealth>().maxHealth = 1000f;
                }
            }

            if (Time.time >= _lastCacheTime) {
                _lastCacheTime = Time.time + 5f;

                Players = FindObjectsOfType<Player>();
                // vehicles = FindObjectsOfType<RiderHolder>();

                Esp.MainCam = Camera.main;
            }

            if (Time.time >= _lastItemCache) {
                _lastItemCache = Time.time + 1f;

                // droppedItems = FindObjectsOfType<Item>();
            }
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
                    Weapon rightGun = Player.localPlayer.GetComponentInChildren<WeaponHandler>().rightWeapon;
                    Weapon leftGun = Player.localPlayer.GetComponentInChildren<WeaponHandler>().leftWeapon;

                    if (rightGun)
                        SuperGun(ref rightGun);

                    if (leftGun)
                        SuperGun(ref leftGun);
                }

                if (GUILayout.Button("Chams")) {
                    Esp.DoChams();
                }
            }
            
            GUILayout.Space(20f);

            GUILayout.BeginVertical("ESP", GUI.skin.box);
            {
                GUILayout.Space(20f);

                GUILayout.BeginHorizontal();
                {
                    Esp.Crosshair = GUILayout.Toggle(Esp.Crosshair, "Crosshair");
                    Esp.Item = GUILayout.Toggle(Esp.Item, "Item");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    Esp.PlayerBox = GUILayout.Toggle(Esp.PlayerBox, "Player Box");
                    Esp.Vehicle = GUILayout.Toggle(Esp.Vehicle, "Vehicle");
                }
                GUILayout.EndHorizontal();

                Esp.PlayerName = GUILayout.Toggle(Esp.PlayerName, "Player Name");
            }
            GUILayout.EndVertical();

            GUI.DragWindow();
        }
    }
}
