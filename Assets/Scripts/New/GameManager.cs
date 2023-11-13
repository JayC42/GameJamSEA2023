
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Tilemap DarkMap;
    public Tilemap BlurredMap;
    public Tilemap BackgroundMap;
    public Tile DarkTile;
    public Tile BlurredTile; 
    //public Image fadeImage;

    //public float fadeTime = 1f;

    private Vector3 CheckpointPosition;

    public static GameManager Inst;

    //public PlayerController Player;

    private string currentLevel;

    private void Start()
    {
        DarkMap.origin = BlurredMap.origin = BackgroundMap.origin;
        DarkMap.size = BlurredMap.size = BackgroundMap.size; 

        foreach (Vector3Int p in DarkMap.cellBounds.allPositionsWithin)
        {
            DarkMap.SetTile(p, DarkTile);
        }
        foreach (Vector3Int p in BlurredMap.cellBounds.allPositionsWithin)
        {
            BlurredMap.SetTile(p, DarkTile);
        }


    }
    //private void Awake()
    //{
    //    if (GameManager.Inst != null && GameManager.Inst != this)
    //    {
    //        Object.Destroy(base.gameObject);
    //        return;
    //    }
    //    base.transform.parent = null;
    //    this.currentLevel = SceneManager.GetActiveScene().name;
    //    GameManager.Inst = this;
    //    Object.DontDestroyOnLoad(base.gameObject);
    //    this.UpdateReferences();
    //}

    //public void UpdateCheckpoint(Vector3 position)
    //{
    //    this.CheckpointPosition = position;
    //}
    //public void ResetLevel()
    //{
    //    base.StartCoroutine(this.ResetLevelRoutine());
    //}
    //public void LoadLevel(string levelName)
    //{
    //    base.StartCoroutine(this.LoadLevelRoutine(levelName));
    //}
    //private IEnumerator ResetLevelRoutine()
    //{
    //    yield return new DOTweenCYInstruction.WaitForCompletion(DOTweenModuleUI.DOFade(this.fadeImage, 1f, this.fadeTime));
    //    yield return SceneManager.LoadSceneAsync(this.currentLevel, 0);
    //    this.UpdateReferences();
    //    if (this.CheckpointPosition != Vector3.zero)
    //    {
    //        this.Player.transform.position = this.CheckpointPosition;
    //        this.ProCamera2D.MoveCameraInstantlyToPosition(this.CheckpointPosition);
    //    }
    //    yield return new DOTweenCYInstruction.WaitForCompletion(DOTweenModuleUI.DOFade(this.fadeImage, 0f, this.fadeTime));
    //    yield break;
    //}

    //private IEnumerator LoadLevelRoutine(string levelName)
    //{
    //    yield return new DOTweenCYInstruction.WaitForCompletion(DOTweenModuleUI.DOFade(this.fadeImage, 1f, this.fadeTime));
    //    yield return SceneManager.LoadSceneAsync(levelName, 0);
    //    this.UpdateReferences();
    //    this.CheckpointPosition = Vector3.zero;
    //    this.currentLevel = levelName;
    //    yield return new DOTweenCYInstruction.WaitForCompletion(DOTweenModuleUI.DOFade(this.fadeImage, 0f, this.fadeTime));
    //    yield break;
    //}
    //private void UpdateReferences()
    //{
    //    this.ProCamera2D = Object.FindObjectOfType<ProCamera2D>();
    //    this.Player = Object.FindObjectOfType<PlayerController>();
    //}
}
