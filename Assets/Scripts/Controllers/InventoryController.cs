using System;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class InventoryController : MonoBehaviour {
        private bool _opened = true;
        private Animator _animator;
        private static readonly int Opened1 = Animator.StringToHash("opened");
        private static readonly int Toggle = Animator.StringToHash("toggle");
        [SerializeField] private Sprite openedSprite = null;
        [SerializeField] private Sprite closedSprite = null;
        [SerializeField] private Image _image = null; 

        public void Start() {
            _animator = GetComponent<Animator>();
        }

        public void ToggleInventory() {
            _animator.SetBool(Toggle,!_opened);
            if (!_opened) {
                _image.sprite = openedSprite;
            }
            else {
                _image.sprite = closedSprite;
            }
        
        }
    
        public void Opened(int opened) {
            _opened = Convert.ToBoolean(opened);
            _animator.SetBool(Opened1,_opened);
        }
    }
}
