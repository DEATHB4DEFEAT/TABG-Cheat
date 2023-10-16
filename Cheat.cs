using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable Unity.PerformanceCriticalCodeNullComparison
// ReSharper disable Unity.NoNullPropagation

namespace TABGMonoInternal
{
    public class Cheat : MonoBehaviour
    {
        private readonly int _mainWid = 1024;
        private Rect _mainWRect = new Rect(10f, 10f, 300f, 150f);

        private bool _drawMenu;
        private bool _magicBullet;

        private float _lastCacheTime = Time.time + 2.5f;
        private float _lastItemCache = Time.time + 1f;

        public static Player[] Players;
        public static Pickup[] DroppedItems;
        public static Car[] Vehicles;


        public void Update() {
            KeyHandler();

            if (Time.time >= _lastCacheTime) {
                _lastCacheTime = Time.time + 2.5f;

                Players = FindObjectsOfType<Player>();
                Vehicles = FindObjectsOfType<Car>();

                Esp.MainCam = Camera.main;
            }

            if (Time.time >= _lastItemCache) {
                _lastItemCache = Time.time + 1f;

                DroppedItems = FindObjectsOfType<Pickup>();
            }
        }

        public void KeyHandler()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                _drawMenu = !_drawMenu;

            if (Input.GetKeyDown(KeyCode.Mouse0) && _magicBullet && Players.Length > 0)
                foreach (Player player in Players)
                    if (player != Player.localPlayer && player != null)
                        foreach (ProjectileHit proj in FindObjectsOfType<ProjectileHit>())
                            player.m_playerDeath.TakeDamage(proj.transform.position, new Vector3());

            if (Input.GetKey(KeyCode.Home))
                Player.localPlayer.GetComponent<Skydiving>().Launch(Player.localPlayer.m_playerCamera.transform.forward);
        }


        public void OnGUI() {
            if (_drawMenu)
                _mainWRect = GUILayout.Window(_mainWid, _mainWRect, MainWindow, "Main");
        }

        private void MainWindow(int id) {
            GUILayout.BeginHorizontal();
            {
                _magicBullet = GUILayout.Toggle(_magicBullet, "Magic Bullet");
                if (GUILayout.Button("GodMode"))
                {
                    Player.localPlayer.stats.regenerationAdd = 3000f;
                    Player.localPlayer.stats.extraJumps = 1000;
                }
            }
            GUILayout.EndHorizontal();

            if (Player.localPlayer != null) {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"Added Speed {Mathf.Floor(Player.localPlayer.stats.movementSpeedAdd)}");
                    Player.localPlayer.stats.movementSpeedAdd = GUILayout.HorizontalSlider(Player.localPlayer.stats.movementSpeedAdd, 0f, 30f);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("SuperGuns"))
                {
                    Gun rightGun = Player.localPlayer?.m_weaponHandler?.rightWeapon?.gun;
                    Gun leftGun = Player.localPlayer?.m_weaponHandler?.leftWeapon?.gun;

                    if (rightGun)
                        SuperGun(ref rightGun);

                    if (leftGun)
                        SuperGun(ref leftGun);
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
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                    {
                        Esp.Crosshair = GUILayout.Toggle(Esp.Crosshair, "Crosshair");
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


        public void SuperGun(ref Gun gun)
        {
            gun.bullets = 65535;
            gun.bulletsInMag = 65535;
            gun.extraSpread = 0f;
            gun.hasFullAuto = true;
            gun.hipSpreadValue = 0f;
            gun.projectileRecoilMultiplier = 0f;
            gun.rateOfFire = 0.025f;
            gun.currentFireMode = 2; // Full auto.

            Destroy(gun.GetComponent<Recoil>());
            Destroy(FindObjectOfType<AddScreenShake>());
        }
    }
}
