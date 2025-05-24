using UnityEngine;
using UnityEngine.UI;

namespace UIAnimations
{
	public class ScrollUV : MonoBehaviour
	{
		private RawImage rawImage;

		[Header("Parameters")]
		public Vector2 speed = new Vector2(1, 0);
		public Vector2 size = new Vector2(256, 256);

		private void Awake()
		{
			rawImage = GetComponent<RawImage>();
		}

		private void Update()
		{
			if (!gameObject.activeInHierarchy) return;

			var calculatedSizeBasedOnScreen = new Vector2(
				rawImage.rectTransform.rect.width / size.x,
				rawImage.rectTransform.rect.height / size.y
			);
			
			rawImage.uvRect = new Rect(
				rawImage.uvRect.position + (speed * Time.deltaTime),
				calculatedSizeBasedOnScreen
			);
		}
	}
}