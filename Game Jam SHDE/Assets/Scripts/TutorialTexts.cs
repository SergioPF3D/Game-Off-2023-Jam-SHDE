using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class TutorialTexts : MonoBehaviour
{
	[SerializeField]
	Image imageToShow;

	[SerializeField]
	List<Image> imagesToShow;

	[SerializeField]
	List<TMPro.TextMeshProUGUI> texts;

	[SerializeField]
	float timeToFade;

	[SerializeField]
	float timeToStart;

	[SerializeField]
	List<KeyCode> inputToFadeOut;

	[SerializeField]
	MoveObjects player;

	[SerializeField]
	TutorialTexts nextext;

	[SerializeField]
	bool time;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
			StartCoroutine(ControlFade());
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
		}
    }

    IEnumerator FadeImage(Image _imageToFade, float _timeToFade, float _TimeToStart, bool toTransparent)
	{
		_imageToFade.gameObject.SetActive(true);
		float timePassed = 0;
		float startalpha = _imageToFade.color.a;
		yield return new WaitForSeconds(_TimeToStart);
		while (timePassed / _timeToFade < 1)
		{
			timePassed += Time.fixedDeltaTime;
			if (toTransparent)
			{
				_imageToFade.color = new Color(_imageToFade.color.r, _imageToFade.color.b, _imageToFade.color.g, Mathf.Lerp(startalpha, 0, timePassed / _timeToFade));
			}
			else
			{
				_imageToFade.color = new Color(_imageToFade.color.r, _imageToFade.color.b, _imageToFade.color.g, Mathf.Lerp(startalpha, 1, timePassed / _timeToFade));
			}

			//new Vector4(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 
			yield return new WaitForFixedUpdate();
		}

		if (toTransparent)
		{
			_imageToFade.gameObject.SetActive(false);
		}

	}

	IEnumerator FadeText(TMPro.TextMeshProUGUI textToFade, float timeToFade, float TimeToStart, bool toTransparent)
	{
		
		textToFade.gameObject.SetActive(true);
		float timePassed = 0;
		float startalpha = textToFade.color.a;
		yield return new WaitForSeconds(TimeToStart);
		while (timePassed / timeToFade < 1)
		{
			timePassed += Time.fixedDeltaTime;
			if (toTransparent)
			{
				textToFade.color = new Color(textToFade.color.r, textToFade.color.b, textToFade.color.g, Mathf.Lerp(startalpha, 0, timePassed / timeToFade));
			}
			else
			{
				textToFade.color = new Color(textToFade.color.r, textToFade.color.b, textToFade.color.g, Mathf.Lerp(startalpha, 1, timePassed / timeToFade));
			}

			//new Vector4(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 
			yield return new WaitForFixedUpdate();
		}

		if (toTransparent)
		{
			textToFade.gameObject.SetActive(false);
		}
	}

	IEnumerator ControlFade()
    {
        if (imageToShow)
        {
			StartCoroutine(FadeImage(imageToShow, timeToFade, timeToStart, false));
		}
		
		foreach (var image in imagesToShow)
		{
			//StartCoroutine(FadeImage(image, timeToFade, timeToStart, true));
		}
		foreach (var text in texts)
        {
			StartCoroutine(FadeText(text, timeToFade, timeToStart, false));
		}


		//yield return new WaitForSeconds(timeToFade + timeToStart);
		bool pressed = false;
        while (pressed == false)
        {
            if (inputToFadeOut.Count > 0)
            {
				foreach (var key in inputToFadeOut)
				{
					if (Input.GetKey(key))
					{
						if (key != KeyCode.Mouse0 || player.target != null)
						{
							StopCoroutine("FadeImage");
							yield return new WaitForEndOfFrame();

							if (imageToShow)
							{
								StartCoroutine(FadeImage(imageToShow, 0.75f, 0, true));
							}

                            foreach (var image in imagesToShow)
                            {
								/*
								StopCoroutine("FadeImage");
								yield return new WaitForEndOfFrame();
								StartCoroutine(FadeImage(image, 0.75f, 0, true));
								*/
							}
							foreach (var text in texts)
							{
								StopCoroutine("FadeText");
								yield return new WaitForEndOfFrame();
								StartCoroutine(FadeText(text, 0.75f, 0, true));
							}
							pressed = true;
							yield return new WaitForSeconds(1);
						}
					}
				}
			}
            else if (time)
            {
				yield return new WaitForSeconds(2);
				StopCoroutine("FadeImage");
				if (imageToShow)
				{
					StartCoroutine(FadeImage(imageToShow, 0.75f, 0, true));
				}
				foreach (var text in texts)
				{
					StopCoroutine("FadeText");
					StartCoroutine(FadeText(text, 0.75f, 0, true));
				}
				pressed = true;
				yield return new WaitForSeconds(1);

			}
            else if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
				StopCoroutine("FadeImage");
				if (imageToShow)
				{
					StartCoroutine(FadeImage(imageToShow, 0.75f, 0, true));
				}
				foreach (var text in texts)
				{
					StopCoroutine("FadeText");
					StartCoroutine(FadeText(text, 0.75f, 0, true));
				}
				pressed = true;
				yield return new WaitForSeconds(1);
			}
            
			//yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();

			
		}
		if (nextext)
		{
			nextext.StartCoroutine("ControlFade");
		}

		//this.gameObject.SetActive(false);
	}
}
