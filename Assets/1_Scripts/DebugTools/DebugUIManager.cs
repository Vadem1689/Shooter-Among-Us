using BRAmongUS.Utils;
using UnityEngine;

namespace BRAmongUS.DebugTools
{
    public sealed class DebugUIManager : MonoBehaviour
    {
        [SerializeField] private DebugButton clearSavesButton;
        [SerializeField] private int maxClicksToResetSave = 5;
        
        private int _counterToResetSave;

        private void Start()
        {
            clearSavesButton.OnPointerUpAction += OnClearButtonClicked;
            clearSavesButton.OnPointerExitAction += OnClearButtonExit;
        }

        private void OnDestroy()
        {
            clearSavesButton.OnPointerUpAction -= OnClearButtonClicked;
            clearSavesButton.OnPointerExitAction -= OnClearButtonExit;
        }

        private void OnClearButtonClicked()
        {
            _counterToResetSave++;
            if (_counterToResetSave >= maxClicksToResetSave)
            {
                SavingUtils.ClearSave();
                ResetSaveCounter();
                this.Log("Saves cleared");
            }
        }

        private void OnClearButtonExit()
        {
            ResetSaveCounter();
        }

        private void ResetSaveCounter()
        {
            _counterToResetSave = 0;
        }
    }
}