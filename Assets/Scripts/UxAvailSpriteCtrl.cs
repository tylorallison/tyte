using UnityEngine;
using UnityEngine.UI;

namespace TyTe {

    // =========================================================================
    public class UxAvailSpriteCtrl : MonoBehaviour {
        // INSTANCE VARIABLES --------------------------------------------------
        public Button selectButton;
        public GameObject selectedPanel;
        public Image spriteImage;

        // INSTANCE METHODS ----------------------------------------------------
        public void Select(
            bool selected
        ) {
            if (selectedPanel != null) {
                selectedPanel.SetActive(selected);
            }
        }

        public void AssignSprite(
            Sprite sprite
        ) {
            if (spriteImage != null) {
                spriteImage.sprite = sprite;
            }
        }

    }
}
