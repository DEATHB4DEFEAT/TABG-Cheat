using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeNullComparison
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable Unity.NoNullPropagation

namespace TABGMonoInternal {
    class ESP : MonoBehaviour {
        public static bool Chams;
        private static Material _chamsMaterial;
        private static float _chamRefreshTime = Time.time + 1f;

        public static bool Item;
        public static bool Vehicle;
        public static bool PlayerName;
        public static bool PlayerBox;

        public static bool Crosshair;
        private static readonly float CrosshairScale = 10f;
        private static readonly float LineThickness = 1.75f;

        public static Camera MainCam;


        public void Start() {
            _chamsMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
            {
                hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy
            };

            _chamsMaterial.SetInt("_SrcBlend", 5);
            _chamsMaterial.SetInt("_DstBlend", 10);
            _chamsMaterial.SetInt("_Cull", 0);
            _chamsMaterial.SetInt("_ZTest", 8); // 8 = see through walls.
            _chamsMaterial.SetInt("_ZWrite", 0);
            _chamsMaterial.SetColor("_Color", Color.magenta);

            MainCam = Camera.main;
        }


        public void OnGUI() {
            if (Event.current.type != EventType.Repaint)
                return;

            DoChams();
            Items();
            Vehicles();
            MakePlayerName();
            MakePlayerBox();
            MakeCrosshair();
        }


        private static void DoChams() {
            if (!Chams)
                return;

            if (!(Time.time >= _chamRefreshTime))
                return;

            _chamRefreshTime = Time.time + 1f;

            foreach (var player in FindObjectsOfType<Player>())
            {
                if (player == null || player == Player.localPlayer)
                    continue;

                foreach (var renderer in player.gameObject.GetComponentsInChildren<Renderer>())
                    renderer.material = _chamsMaterial;
            }
        }

        private static void Items()
        {
            if (!Item)
                return;

            if (Cheat.DroppedItems.Length > 0)
            {
                foreach (var item in Cheat.DroppedItems)
                {
                    if (item == null)
                        continue;

                    Vector3 w2S = MainCam.WorldToScreenPoint(item.transform.position);
                    w2S.y = Screen.height - (w2S.y + 1f);

                    if (ESPUtils.IsOnScreen(w2S))
                        ESPUtils.DrawString(w2S, item.name, Color.green, true, 12, FontStyle.BoldAndItalic, 1);
                }
            }
        }

        private static void Vehicles()
        {
            if (!Vehicle)
                return;

            if (Cheat.Vehicles.Length > 0)
            {
                foreach (var vehicle in Cheat.Vehicles)
                {
                    if (vehicle == null)
                        continue;

                    Vector3 w2S = MainCam.WorldToScreenPoint(vehicle.transform.position);
                    w2S.y = Screen.height - (w2S.y + 1f);

                    if (ESPUtils.IsOnScreen(w2S))
                        ESPUtils.DrawString(w2S, "Vehicle", Color.yellow, true, 12, FontStyle.BoldAndItalic, 1);
                }
            }
        }

        private static void MakePlayerName()
        {
            if (!PlayerName)
                return;

            if (Cheat.Players.Length > 0)
            {
                foreach (var player in Cheat.Players)
                {
                    if (player != null && player != Player.localPlayer)
                    {
                        var w2S = MainCam.WorldToScreenPoint(player.GetComponentInChildren<FootLeft>().transform.position);
                        w2S.y = Screen.height - (w2S.y + 1f);

                        if (ESPUtils.IsOnScreen(w2S)) {
                            ESPUtils.DrawString(w2S, player.name, Color.cyan, true, 12, FontStyle.Bold, 1);
                        }
                    }
                }
            }
        }

        private static void MakePlayerBox()
        {
            if (!PlayerBox)
                return;

            if (Cheat.Players.Length > 0)
            {
                foreach (var player in Cheat.Players)
                {
                    if (player != null && player != Player.localPlayer)
                    {
                        var w2SHead = MainCam.WorldToScreenPoint(player.GetComponentInChildren<Head>().transform.position);
                        var w2SBottom = MainCam.WorldToScreenPoint(player.GetComponentInChildren<FootLeft>().transform.position);

                        var height = Mathf.Abs(w2SHead.y - w2SBottom.y);

                        if (ESPUtils.IsOnScreen(w2SHead))
                            ESPUtils.CornerBox(new Vector2(w2SHead.x, Screen.height - w2SHead.y - 20f), height / 2f,
                                height + 20f, 2f, Color.cyan, true);
                    }
                }
            }
        }

        private static void MakeCrosshair()
        {
            if (!Crosshair) {
                return;
            }

            var col = new Color32(30, 144, 255, 255);

            var lineHorizontalStart = new Vector2(Screen.width / 2 - CrosshairScale, Screen.height / 2);
            var lineHorizontalEnd = new Vector2(Screen.width / 2 + CrosshairScale, Screen.height / 2);

            var lineVerticalStart = new Vector2(Screen.width / 2, Screen.height / 2 - CrosshairScale);
            var lineVerticalEnd = new Vector2(Screen.width / 2, Screen.height / 2 + CrosshairScale);

            ESPUtils.DrawLine(lineHorizontalStart, lineHorizontalEnd, col, LineThickness);
            ESPUtils.DrawLine(lineVerticalStart, lineVerticalEnd, col, LineThickness);
        }
    }
}
