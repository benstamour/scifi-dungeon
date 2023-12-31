using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourCode : MonoBehaviour
{
	private string code;
	private string input = "";
	private GameManager gameManagerScript;
	[SerializeField] private Material standardMaterial;
	[SerializeField] private Material incorrectMaterial;
	[SerializeField] private Material correctMaterial;
	private bool correct = false;
	
	private GameObject screen;
	
	[SerializeField] private GameObject[] unhideRooms;
	
    // Start is called before the first frame update
    void Start()
    {
		this.gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.code = gameManagerScript.getColourCode();
		this.screen = GameObject.Find("Light-Up Screen");
		
		// remove unneeded colour code icons
		for(int i = 0; i < this.code.Length; i++)
		{
			if(this.code[i] != 'C')
			{
				GameObject cyanIcon = GameObject.Find("Cyan Icon " + (i+1));
				cyanIcon.SetActive(false);
			}
			if(this.code[i] != 'M')
			{
				GameObject magentaIcon = GameObject.Find("Magenta Icon " + (i+1));
				magentaIcon.SetActive(false);
			}
			if(this.code[i] != 'Y')
			{
				GameObject yellowIcon = GameObject.Find("Yellow Icon " + (i+1));
				yellowIcon.SetActive(false);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void addInput(char colour)
	{
		if(this.correct)
		{
			return;
		}
		
		this.input = this.input + colour;
		//Debug.Log(this.input);
		if(this.input.Length == 5)
		{
			if(this.input == this.code)
			{
				//Debug.Log("Correct");
				this.screen.GetComponent<MeshRenderer>().material = correctMaterial;
				this.correct = true;
				
				// open exit door
				GameObject door = GameObject.Find("Third Door");
				Animator anim = door.GetComponent<Animator>();
				anim.SetBool("character_nearby", true);
				
				for(int i = 0; i < this.unhideRooms.Length; i++)
				{
					unhideRooms[i].SetActive(true);
				}
			}
			else
			{
				this.screen.GetComponent<MeshRenderer>().material = incorrectMaterial;
				//Debug.Log("Incorrect");
				StartCoroutine(revertScreen());
				
			}
			this.input = "";
		}
	}
	
	IEnumerator revertScreen()
	{
		yield return new WaitForSeconds(1f);
		this.screen.GetComponent<MeshRenderer>().material = standardMaterial;
	}
}
