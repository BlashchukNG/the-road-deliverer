using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.UI
{
	public class MainMenuButton : MonoBehaviour
	{
		public TMP_Text Label { get; }
		public Button Button { get; }
		public Image Image { get; }

		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _text;
		[SerializeField] private Image _image;
	}
}