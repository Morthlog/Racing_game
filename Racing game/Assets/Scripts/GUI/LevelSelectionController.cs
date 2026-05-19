using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionController : MonoBehaviour
{
    
    [SerializeField] Sprite[] scenePreviewSprites;
    [SerializeField] GameObject levelSelectionItemPrefab;
    [SerializeField] GameObject viewportContainer;
    

    void Start()
    {
        // this will hardcode values based on scenePreviewSprites order and sceneList order
        for (int i = 0; i < scenePreviewSprites.Length; i++)
        {
            GameObject profileBtnWrapper = Instantiate(levelSelectionItemPrefab, viewportContainer.transform);

            LevelSelectionItem selectionItem = profileBtnWrapper.GetComponent<LevelSelectionItem>();
            selectionItem.SetLevelPrevieImg(scenePreviewSprites[i]);
            selectionItem.SetLevelToPlayBtn(i+1);//0 is menu scene so we add one
        }
    }
}
