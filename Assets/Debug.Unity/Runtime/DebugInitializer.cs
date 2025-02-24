using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BashoKit.GameDebug.Unity {
    public class DebugInitializer : MonoBehaviour {
        [FormerlySerializedAs("debugReferences")] [SerializeField] private DebugCanvasReferences _debugReferences;
        [SerializeField] private string _debugAssemblyName;

        private readonly Dictionary<string, Button> _tabButtons = new Dictionary<string, Button>();
        private readonly Dictionary<string, GameObject> _tabPanels = new Dictionary<string, GameObject>();
        private DebugCanvasView _canvasView;
        private bool IsInitialized => _canvasView != null;

        private void Update() {
            if (Input.GetKeyUp(KeyCode.F1)) {
                if (!IsInitialized) Initialize();
                _canvasView?.ToggleDebugCanvas();
            } else if (_canvasView is { IsVisible: true } && Input.GetKeyUp(KeyCode.Escape)) {
                _canvasView?.CloseDebugCanvas();
            }
        }

        private void Initialize() {
            var canvasGo = Instantiate(_debugReferences.CanvasPrefab);
            _canvasView = canvasGo.GetComponent<DebugCanvasView>();
            
            var debugMethods = DebugResolver.GetDebugActions(_debugAssemblyName);
            // Group by tab name using the DebugTabAttribute on the declaring type.
            var groupedMethods = debugMethods.GroupBy(tuple => {
                var headerAttr = tuple.method.DeclaringType.GetCustomAttribute<DebugTabAttribute>();
                return headerAttr != null ? headerAttr.TabName : "N/A";
            });

            foreach (var debugGroup in groupedMethods) {
                // Create a panel for each tab.
                var panel = Instantiate(_debugReferences.cheatPanelContainerPrefab, _canvasView.PanelContainer);
                var panelContentTransform = panel.transform;
                CreateTab(_canvasView, debugGroup.Key, panel);

                // Group the methods further by header name.
                var headersGrouping = debugGroup.GroupBy(p => p.actionAttribute.HeaderName).ToList();
                foreach (var headerGroup in headersGrouping) {
                    CreateHeader(headerGroup, panelContentTransform);
                    foreach (var (method, attribute) in headerGroup) {
                        CreateDebugActions(panelContentTransform, attribute, method);
                    }
                }
            }

            SelectFirstTab(groupedMethods);
        }

        private void SelectFirstTab(IEnumerable<IGrouping<string, (MethodInfo method, DebugActionAttribute actionAttribute)>> groupedMethods) {
            var firstTab = groupedMethods.First();
            _tabButtons[firstTab.Key].Select();
            ChangeTab(firstTab.Key);
        }

        private void CreateDebugActions(Transform panelContentTransform, DebugActionAttribute attribute, MethodInfo method) {
            var buttonObj = Instantiate(_debugReferences.CheatButtonPrefab, panelContentTransform);
            var btn = buttonObj.GetComponent<Button>();
            var btnText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = string.IsNullOrEmpty(attribute.DisplayName) ? method.Name : attribute.DisplayName;

            btn.onClick.AddListener(() => {
                // Resolve instance automatically via the static registry.
                object instance = DebugInstanceRegistry.GetInstance(method.DeclaringType);
                if (method.IsStatic) {
                    method.Invoke(null, null);
                }
                else if (instance != null) {
                    method.Invoke(instance, null);
                }
            });
        }

        private void CreateHeader(IGrouping<string, (MethodInfo method, DebugActionAttribute actionAttribute)> headerGroup, Transform panelContentTransform) {
            var headerName = headerGroup.Key;
            if (string.IsNullOrEmpty(headerName))
                return;

            var header = Instantiate(_debugReferences.cheatHeaderPrefab, panelContentTransform);
            header.GetComponent<TextMeshProUGUI>().text = headerName;
        }

        private void CreateTab(DebugCanvasView canvasView, string tabName, GameObject panel) {
            var tab = Instantiate(_debugReferences.tabPrefab, canvasView.TabContainer);
            var tabBtn = tab.GetComponent<Button>();
            var tabText = tab.GetComponentInChildren<TextMeshProUGUI>();
            tabText.text = tabName;

            tabBtn.onClick.AddListener(() => { ChangeTab(tabText.text); });

            _tabButtons.Add(tabName, tabBtn);
            _tabPanels.Add(tabName, panel);
        }

        public void ChangeTab(string tabName) {
            foreach (var kvp in _tabPanels) {
                kvp.Value.SetActive(kvp.Key.Equals(tabName));
            }
        }
    }
}