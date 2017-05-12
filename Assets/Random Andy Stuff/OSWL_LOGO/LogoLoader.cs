using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogoLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("countDown");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private IEnumerator countDown(){
		yield return new WaitForSeconds(9f);
        SceneManager.LoadScene(1);
	}
}
