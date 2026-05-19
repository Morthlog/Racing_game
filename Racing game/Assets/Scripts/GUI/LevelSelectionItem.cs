using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionItem : MonoBehaviour
{
    [SerializeField] Image levelPreviewImg;
    [SerializeField] Button playBtn;   

    public void SetLevelPrevieImg(Sprite sprite)
    {
        levelPreviewImg.sprite=sprite;
    }

    public void SetLevelToPlayBtn(int levelIndex)
    {
        playBtn.onClick.AddListener(()=> GameManager.instance.LoadLevelByIndex(levelIndex));
    }
}
