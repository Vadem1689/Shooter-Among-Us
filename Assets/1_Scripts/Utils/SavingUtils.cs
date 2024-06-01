using System.Runtime.CompilerServices;
using YG;

namespace BRAmongUS.Utils
{
    public enum ESaveType
    {
        TotalCoins,
        TotalKills,
        SelectedSkinIndex,
        BoughtSkins,
        IsAudioMuted, 
    }

    public static class SavingUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetInt(in ESaveType saveType, in int value)
        {
            switch (saveType)
            {
                case ESaveType.TotalCoins:
                    YandexGame.savesData.totalCoins = value;
                    break;
                case ESaveType.TotalKills:
                    YandexGame.savesData.totalKills = value;
                    break;
                case ESaveType.SelectedSkinIndex:
                    YandexGame.savesData.selectedSkinIndex = value;
                    break;
            }
            YandexGame.SaveProgress();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetBool(in ESaveType saveType, in bool value)
        {
            switch (saveType)
            {
                case ESaveType.IsAudioMuted:
                    YandexGame.savesData.isAudioMuted = value;
                    break;
            }
            YandexGame.SaveProgress();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetBoolArrayElement(in ESaveType saveType, in int index, in bool value)
        {
            switch (saveType)
            {
                case ESaveType.BoughtSkins:
                    YandexGame.savesData.boughtSkins[index] = value;
                    break;
            }
            YandexGame.SaveProgress();
        }
        
        public static void ClearSave()
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();
        }
    }
}