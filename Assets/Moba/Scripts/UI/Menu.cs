using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Start()
    {
        transform.Find("btnRestart").GetComponent<Button>().onClick.AddListener(() =>
        {
            ServerController_III serverController_III = FindObjectOfType<ServerController_III>();
            if (serverController_III != null)
            {
                serverController_III.StopHost();
                serverController_III.StopClient();
                serverController_III.StopServer();
            }
            SceneManager.LoadScene("ServerManage");
        });
    }
}
