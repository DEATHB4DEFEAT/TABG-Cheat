using UnityEngine;

namespace ExampleAssembly {
    class Esp : MonoBehaviour {
        public static bool PlayerBox;
        public static bool PlayerName;
        public static bool Crosshair;
        public static bool Item;
        public static bool Vehicle;

        private static readonly float CrosshairScale = 7f;
        private static readonly float LineThickness = 1.75f;

        private static Material _chamsMaterial;

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

        public static void DoChams() {
            foreach (Player player in FindObjectsOfType<Player>()) {
                if (player == null) {
                    continue;
                }

                foreach (Renderer renderer in player.gameObject.GetComponentsInChildren<Renderer>())
                {
                    renderer.material = _chamsMaterial;
                    // renderer.material = TABGMaterialDatabase.Instance.GetRarityMaterial(Curse.Rarity.Legendary);
                }

                /*Highlighter h = player.GetOrAddComponent<Highlighter>();
                
                if (h) {
                    h.FlashingOff();
                    h.ConstantOnImmediate(Color.red);
                }*/
            }
        }

        public void OnGUI() {
            if (Event.current.type != EventType.Repaint) {
                return;
            }

            // Items();
            // Vehicles();
            MakePlayerName();
            MakePlayerBox();
            MakeCrosshair();
        }

        // private static void Items() {
        //     if (!item) {
        //         return;
        //     }
        //
        //     if (Cheat.droppedItems.Length > 0) {
        //         foreach (var item in Cheat.droppedItems) {
        //             if (item == null) {
        //                 continue;
        //             }
        //
        //             Vector3 w2s = mainCam.WorldToScreenPoint(item.transform.position);
        //             w2s.y = Screen.height - (w2s.y + 1f);
        //
        //             if (ESPUtils.IsOnScreen(w2s)) {
        //                 ESPUtils.DrawString(w2s, item.name, Color.green, true, 12, FontStyle.BoldAndItalic, 1);
        //             }
        //         }
        //     }
        // }

        // private static void Vehicles() {
        //     if (!vehicle) {
        //         return;
        //     }
        //
        //     if (Cheat.vehicles.Length > 0) {
        //         foreach (var vehicle in Cheat.vehicles) {
        //             if (vehicle == null) {
        //                 continue;
        //             }
        //
        //             Vector3 w2s = mainCam.WorldToScreenPoint(vehicle.transform.position);
        //             w2s.y = Screen.height - (w2s.y + 1f);
        //
        //             if (ESPUtils.IsOnScreen(w2s)) {
        //                 ESPUtils.DrawString(w2s, "Vehicle", Color.yellow, true, 12, FontStyle.BoldAndItalic, 1);
        //             }
        //         }
        //     }
        // }

        private static void MakePlayerBox() {
            if (!PlayerBox) {
                return;
            }

            if (Cheat.Players.Length > 0) {
                foreach (Player player in Cheat.Players) {
                    if (player != null && player != Player.localPlayer) {
                        Vector3 w2SHead = MainCam.WorldToScreenPoint(player.GetComponentInChildren<Head>().transform.position);
                        Vector3 w2SBottom = MainCam.WorldToScreenPoint(player.GetComponentInChildren<FootLeft>().transform.position);

                        float height = Mathf.Abs(w2SHead.y - w2SBottom.y);

                        if (ESPUtils.IsOnScreen(w2SHead)) {
                            ESPUtils.CornerBox(new Vector2(w2SHead.x, Screen.height - w2SHead.y - 20f), height / 2f, height + 20f, 2f, Color.cyan, true);
                        } 
                    }
                }
            }
        }

        private static void MakePlayerName() {
            if (!PlayerName) {
                return;
            }

            if (Cheat.Players.Length > 0) {
                foreach (Player player in Cheat.Players) {
                    if (player != null && player != Player.localPlayer) {
                        Vector3 w2S = MainCam.WorldToScreenPoint(player.GetComponentInChildren<FootLeft>().transform.position);
                        w2S.y = Screen.height - (w2S.y + 1f);

                        if (ESPUtils.IsOnScreen(w2S)) {
                            ESPUtils.DrawString(w2S, "Player", Color.cyan, true, 12, FontStyle.Bold, 1);
                        }
                    }
                }
            }
        }

        private static void MakeCrosshair() {
            if (!Crosshair) {
                return;
            }

            Color32 col = new Color32(30, 144, 255, 255);

            Vector2 lineHorizontalStart = new Vector2(Screen.width / 2 - CrosshairScale, Screen.height / 2);
            Vector2 lineHorizontalEnd = new Vector2(Screen.width / 2 + CrosshairScale, Screen.height / 2);

            Vector2 lineVerticalStart = new Vector2(Screen.width / 2, Screen.height / 2 - CrosshairScale);
            Vector2 lineVerticalEnd = new Vector2(Screen.width / 2, Screen.height / 2 + CrosshairScale);

            ESPUtils.DrawLine(lineHorizontalStart, lineHorizontalEnd, col, LineThickness);
            ESPUtils.DrawLine(lineVerticalStart, lineVerticalEnd, col, LineThickness);
        }
    }
}
