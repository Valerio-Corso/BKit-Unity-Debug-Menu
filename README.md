# bkit-unity-debug-menu
A lightweight simple unity debug menu, it uses an assembly definition to resolve your debug classes and ivoke actions

## Requirements
- TextMeshPro 3.0.0

## Installation
- Import the Debug.Unity folder or the package in release.

## Usage
1. Create a new object in your scene that holds the following:

![image](https://github.com/user-attachments/assets/7a358760-701b-4174-b065-e46b2a552d9b)

2. Make sure to define an assembly, if you don't have one create it for the folder holding your debug scripts.
3. Press F1 while in-game

Start using it like this:

```
    [DebugTab("Test Debug One")]
    public class TestDebugOne
    {
        [DebugAction("Test Action")]
        public void TestAction()
        {
        }
    }
```

![image](https://github.com/user-attachments/assets/175321ef-2787-4efc-87c1-d0d5fe83b066)
