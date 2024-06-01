using UnityEngine;
using UnityEngine.SceneManagement;

namespace BRAmongUS.UI
{
    public sealed class LeaderboardUiManager : MonoBehaviour
    {
        public void CloseLeaderboard()
        {
            SceneManager.UnloadSceneAsync(Constants.Scenes.Leaderboard);
        }
    }
}