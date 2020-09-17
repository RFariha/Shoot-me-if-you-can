using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmunitionManager : MonoBehaviour
{
	public static int ammo;
	public static int mag;

	Text text;


	void Awake ()
	{
		text = GetComponent <Text> ();
		ammo = 0;
		mag = 0;
	}


	void Update ()
	{
		text.text = mag + "/" + ammo;
	}
}
