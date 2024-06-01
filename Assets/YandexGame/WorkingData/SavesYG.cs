using System.Linq;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];
        
        public int totalCoins = 0;
        public int totalKills = 0;
        public int selectedSkinIndex = -1;
        public bool[] boughtSkins = Enumerable.Repeat(false, 20).ToArray();
        public bool isAudioMuted = false;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            if (selectedSkinIndex == -1)
            {
                boughtSkins[0] = true;
                selectedSkinIndex = 0;
            }
        }
    }
}
