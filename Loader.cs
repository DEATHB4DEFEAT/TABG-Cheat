using System;
using BepInEx;

namespace ExampleAssembly
{
    [BepInPlugin("com.biney.exampleassembly", "ExampleAssembly", "1.0")]
    public class Loader : BaseUnityPlugin
    {
        static UnityEngine.GameObject _gameObject;

        private void Awake()
        {
            Load();
        }

        public static void Load()
        {
            Console.WriteLine("Loading ExampleAssembly...");
            _gameObject = new UnityEngine.GameObject();
            _gameObject.AddComponent<Cheat>();
            _gameObject.AddComponent<Esp>();
            UnityEngine.Object.DontDestroyOnLoad(_gameObject);
            Console.WriteLine("Loaded ExampleAssembly!");
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(_gameObject);
        }
    }
}
