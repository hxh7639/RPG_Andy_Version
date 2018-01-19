using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

	public float autoLoadNextLevelAfter;

	void Awake () {
		DontDestroyOnLoad(gameObject);
		Debug.Log ("Don't destory on load: " + name);
		
	}
	
	void Start(){
		if (autoLoadNextLevelAfter <=0){
			Debug.Log ("Level auto load disabled, use a postive number in seconds");
		}else {
		Invoke ("LoadNextLevel", autoLoadNextLevelAfter);
		}
	}
	public void LoadLevel(string name){
		Debug.Log ("Level load requested for: " + name);
		SceneManager.LoadScene(name);
	}
	
	
	public void QuitRequest (){
		Debug.Log ("Quit Request received");
		Application.Quit();
	}
	
	public void LoadNextLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	/*	Application.LoadLevel(Application.loadedLevel + 1);   // old system, unity 4 */
	}
	
}

