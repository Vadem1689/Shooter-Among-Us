using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class RandomNamePicker : MonoBehaviour
{
    private static List<string> m_namesList;
    private TextAsset m_textAsset;

    public void Init()
    {
        string path = YandexGame.savesData.language == "ru" ? "TextFiles/names_ru" : "TextFiles/names_en";
        m_textAsset = Resources.Load(path) as TextAsset;
        ReadTextFile();
        //Debug.Log("NamesCount:"+m_namesList.Count);
    }

    private void ReadTextFile()
    {
        m_namesList = m_textAsset.text.Split('\n').ToList();
        
    }

    public string GetRandomPlayerName()
    {
        //Debug.Log("Random name:" + m_namesList[Random.Range(0, m_namesList.Count)]);
        return m_namesList[Random.Range(0, m_namesList.Count)];
    }
}

