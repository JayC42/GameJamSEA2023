using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneToLoad;

    PlayerController pc;
    NPC npc;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        npc = GameObject.FindGameObjectWithTag("NPC").GetComponentInChildren<NPC>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pc.SceneNumber++;
            npc.tutorialActive = true;
            //SceneManager.LoadScene(sceneToLoad);
        }
    }

}
