using BepInEx;

namespace ExampleAssembly
{
    [BepInPlugin("com.biney.exampleassembly", "ExampleAssembly", "1.0")]
    public class Loader : BaseUnityPlugin
    {
        static UnityEngine.GameObject _gameObject;

        private void Awake()
        {
            _gameObject = new UnityEngine.GameObject();
            _gameObject.AddComponent<Cheat>();
            _gameObject.AddComponent<Esp>();
            DontDestroyOnLoad(_gameObject);
        }

        private void OnDestroy()
        {
            Destroy(_gameObject);
        }
    }
}
