using UnityEngine;

namespace BashoKit.GameDebug.Unity
{
    [CreateAssetMenu(fileName = "DebugInstaller", menuName = "Debug/Debug Installer")]
    public class DebugCanvasReferences : ScriptableObject {
        [SerializeField] private GameObject _canvasPrefab;
        [SerializeField] private GameObject _cheatButtonPrefab;
        [SerializeField] private GameObject _cheatHeaderPrefab;
        [SerializeField] private GameObject _cheatPanelContainerPrefab;
        [SerializeField] private GameObject _tabPrefab;

        public GameObject cheatPanelContainerPrefab => _cheatPanelContainerPrefab;
        public GameObject tabPrefab => _tabPrefab;
        public GameObject CanvasPrefab => _canvasPrefab;
        public GameObject CheatButtonPrefab => _cheatButtonPrefab;
        public GameObject cheatHeaderPrefab => _cheatHeaderPrefab;
    }
}