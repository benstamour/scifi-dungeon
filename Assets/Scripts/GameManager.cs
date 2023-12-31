using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script that stores game data so that it is saved if a new scene loads or if the current scene reloads
public class GameManager : MonoBehaviour
{
	[SerializeField] private int score = 0; // number of orbs collected
	[SerializeField] private float gameStartTime = 0f; // time when gameplay begins
	[SerializeField] private float gameTotalTime = 0f; // total time taken during game (incl. player deaths)
	//private float timeTaken = 0f; // total time taken in successful run
	private float savePointTime = 0f; // cumulative time to get to each save point and the end zone; does not include time beyond save points that resulted in character being killed
	[SerializeField] private int numAttempts = 0; // total number of attempts taken
	//[SerializeField] private string character = "";
	[SerializeField] private int character;
	
	[SerializeField] private int spawnPoint = -1; // save point that character should spawn at; -1 for beginning location
	[SerializeField] private float spawnRot = 0; // rotation of player when spawning
	
	public AudioClip menuSoundtrack;
	public AudioClip introSoundtrack;
	public AudioClip neonSoundtrack;
	public AudioClip holoSoundtrack;
	public AudioClip circuitSoundtrack;
	public AudioClip warehouseSoundtrack;
	public AudioClip battleSoundtrack;
	public AudioClip buttonClip;
	public AudioClip orbClip;
	public AudioClip leverClip;
	public AudioClip endZoneClip;
	
	public AudioClip[] robotVoiceClips;
	
	public AudioSource audioSource1;
	public AudioSource audioSource2;
	[SerializeField] private bool volume = true; // is music/sound on or off?
	
	[SerializeField] private int curArenaMusic = 0; // which arena track is currently playing?
	
	[SerializeField] private bool savePoints = true; // are save points enabled or disabled?
	[SerializeField] private bool[] orbsCollected = new bool[6]; // bool for whether each orb has been collected
	
	private int introcode;
	private string colourcode;
	private string holocode;
	
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		if(SceneManager.GetActiveScene().name == "BootstrapScene")
		{
			this.LoadStartScreen();
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
		Array.Fill(this.orbsCollected,false); // at beginning, no orbs have been collected
		
		
		// move the below code to the ARENA MANAGER after making it
		
		// open door after spikeball area
		/*GameObject door = GameObject.Find("Second Door");
		Animator anim = door.GetComponent<Animator>();
		anim.SetBool("character_nearby", true);
		
		// open door to elevator
		door = GameObject.Find("Seventh Door");
		anim = door.GetComponent<Animator>();
		anim.SetBool("character_nearby", true);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SetCharacter(int character)
	{
		this.character = character;
	}
	
	public void setScore(int score)
	{
		this.score = score;
	}
	public int getScore()
	{
		return this.score;
	}
	
	/*public void setTime(float time)
	{
		this.timeTaken = time;
	}
	public float getTime()
	{
		return this.timeTaken;
	}*/
	public float getTotalTime()
	{
		return this.gameTotalTime;
	}
	public void addSavePointTime(float time)
	{
		this.savePointTime = this.savePointTime + time;
		//Debug.Log(this.savePointTime);
	}
	public void setSavePointTime(float time)
	{
		this.savePointTime = time;
	}
	public float getSavePointTime()
	{
		return this.savePointTime;
	}
	
	public void setSpawnPoint(int spawnPoint)
	{
		this.spawnPoint = spawnPoint;
	}
	public int getSpawnPoint()
	{
		return this.spawnPoint;
	}
	public void setSpawnRotation(float spawnRot)
	{
		this.spawnRot = spawnRot;
	}
	public float getSpawnRotation()
	{
		return this.spawnRot;
	}
	
	public void LoadStartScreen()
	{
		SceneManager.LoadScene("StartScreen");
		
		// reset variables
		this.score = 0;
		//this.timeTaken = 0f;
		this.gameStartTime = 0f;
		this.gameTotalTime = 0f;
		this.savePointTime = 0f;
		this.numAttempts = 0;
		this.spawnPoint = -1;
		this.spawnRot = 0;
		this.orbsCollected = new bool[6];
		
		// play menu soundtrack
		AudioSource audioSource = this.audioSource1;
		audioSource.clip = menuSoundtrack;
		audioSource.Stop();
		audioSource.loop = true;
		audioSource.Play();
	}
	
	public void StartGame()
	{
		this.spawnPoint = -1;
		SceneManager.LoadScene("Arena");
		
		// move the below code to the ARENA MANAGER after making it
		this.introcode = UnityEngine.Random.Range(1000,10000);
		
		string[] arr = {"C", "C", "M", "M", "Y", "Y"};
		for(int i = arr.Length - 1; i > 0; i--)
		{
			int num = UnityEngine.Random.Range(0,i);
			string temp = arr[i];
			arr[i] = arr[num];
			arr[num] = temp;
		}
		this.colourcode = arr[0] + arr[1] + arr[2] + arr[3] + arr[4];
		
		int numHoloTiles = 6;
		for(int i = 0; i < numHoloTiles; i++)
		{
			this.holocode = this.holocode + UnityEngine.Random.Range(0,2);
		}
		
		// play arena soundtrack
		StartCoroutine(CheckArenaLoaded());
		/*AudioSource audioSource = this.audioSource1;
		audioSource.Stop();
		audioSource.clip = arenaSoundtrack;
		audioSource.loop = true;
		audioSource.Play();*/
	}
	
	public void EndZone()
	{
		this.gameTotalTime = Time.time - this.gameStartTime;
		SceneManager.LoadScene("EndScreen");
		
		AudioSource audioSource = this.audioSource1;
		audioSource.Stop();
		audioSource.clip = endZoneClip;
		audioSource.loop = false;
		audioSource.Play();
		
		if(this.audioSource1.volume > 0)
		{
			this.audioSource1.volume = 1;
		}
	}
	
	public void incrementAttempts()
	{
		this.numAttempts += 1;
	}
	
	public int getAttempts()
	{
		return this.numAttempts;
	}
	
	public int getChar()
	{
		return this.character;
	}
	
	// play sound effects
	public void PlayButtonClip()
	{
		AudioSource audioSource = this.audioSource2;
		audioSource.Stop();
		audioSource.clip = buttonClip;
		audioSource.loop = false;
		audioSource.Play();
	}	
	public void PlayOrbClip()
	{
		AudioSource audioSource = this.audioSource2;
		audioSource.Stop();
		audioSource.clip = orbClip;
		audioSource.loop = false;
		audioSource.Play();
	}	
	public void PlayLeverClip()
	{
		AudioSource audioSource = this.audioSource2;
		audioSource.Stop();
		audioSource.clip = leverClip;
		audioSource.loop = false;
		audioSource.Play();
	}
	
	// turn music/sound effects on and off
	public bool getVolume()
	{
		return this.volume;
	}
	public void ToggleVolume()
	{
		this.volume = !this.volume;
		
		if(this.volume == false)
		{
			this.audioSource1.volume = 0;
			this.audioSource2.volume = 0;
		}
		else
		{
			this.audioSource1.volume = 1;
			this.audioSource2.volume = 1;
			this.PlayButtonClip();
		}
	}
	
	// enable/disable save points
	public bool getSavePointsEnabled()
	{
		return this.savePoints;
	}
	public void ToggleSavePoints()
	{
		this.savePoints = !this.savePoints;
		this.PlayButtonClip();
	}
	
	// get/set info on which orbs are collected
	public bool getOrbCollected(int id)
	{
		return this.orbsCollected[id];
	}
	public bool[] getOrbsCollected()
	{
		return this.orbsCollected;
	}
	public void setOrbsCollected(bool[] b)
	{
		b.CopyTo(this.orbsCollected,0);
	}
	public void setOrbCollected(int id, bool b)
	{
		this.orbsCollected[id] = b;
	}
	public void addSavePointScore(int num)
	{
		this.score += num;
		//Debug.Log(num.ToString() + " " + this.score.ToString());
	}
	
	IEnumerator CheckArenaLoaded()
	{
		while(true)
		{
			if(SceneManager.GetActiveScene().name == "Arena")
			{
				// play arena soundtrack
				AudioSource audioSource = this.audioSource1;
				audioSource.Stop();
				audioSource.clip = introSoundtrack;
				audioSource.loop = true;
				audioSource.Play();
				this.curArenaMusic = 0;
				break;
			}
			else
			{
				yield return null;
			}
		}
		this.gameStartTime = Time.time;
	}
	
	public void triggerIntroMusic()
	{
		if(this.curArenaMusic != 0)
		{
			AudioSource audioSource = this.audioSource1;
			audioSource.Stop();
			audioSource.clip = introSoundtrack;
			audioSource.loop = true;
			audioSource.Play();
			this.curArenaMusic = 0;
			
			if(this.audioSource1.volume > 0)
			{
				this.audioSource1.volume = 1;
			}
		}
	}
	public void triggerNeonMusic()
	{
		if(this.curArenaMusic != 1)
		{
			AudioSource audioSource = this.audioSource1;
			audioSource.Stop();
			audioSource.clip = neonSoundtrack;
			audioSource.loop = true;
			audioSource.Play();
			this.curArenaMusic = 1;
			
			if(this.audioSource1.volume > 0)
			{
				this.audioSource1.volume = 0.75f;
			}
		}
	}
	public void triggerHoloMusic()
	{
		if(this.curArenaMusic != 2)
		{
			AudioSource audioSource = this.audioSource1;
			audioSource.Stop();
			audioSource.clip = holoSoundtrack;
			audioSource.loop = true;
			audioSource.Play();
			this.curArenaMusic = 2;
			
			if(this.audioSource1.volume > 0)
			{
				this.audioSource1.volume = 1;
			}
		}
	}
	public void triggerCircuitMusic()
	{
		if(this.curArenaMusic != 3)
		{
			AudioSource audioSource = this.audioSource1;
			audioSource.Stop();
			audioSource.clip = circuitSoundtrack;
			audioSource.loop = true;
			audioSource.Play();
			this.curArenaMusic = 3;
			
			if(this.audioSource1.volume > 0)
			{
				this.audioSource1.volume = 1;
			}
		}
	}
	public void triggerWarehouseMusic()
	{
		if(this.curArenaMusic != 4)
		{
			AudioSource audioSource = this.audioSource1;
			audioSource.Stop();
			audioSource.clip = warehouseSoundtrack;
			audioSource.loop = true;
			audioSource.Play();
			this.curArenaMusic = 4;
			
			if(this.audioSource1.volume > 0)
			{
				this.audioSource1.volume = 1;
			}
		}
	}
	public void triggerBattleMusic()
	{
		if(this.curArenaMusic != 5)
		{
			AudioSource audioSource = this.audioSource1;
			audioSource.Stop();
			audioSource.clip = battleSoundtrack;
			audioSource.loop = true;
			audioSource.Play();
			this.curArenaMusic = 5;
			
			if(this.audioSource1.volume > 0)
			{
				this.audioSource1.volume = 0.35f;
			}
		}
	}
	public int getCurArenaMusic()
	{
		return this.curArenaMusic;
	}
	
	public int getIntroCode()
	{
		return this.introcode;
	}
	public string getColourCode()
	{
		return this.colourcode;
	}
	public string getHoloCode()
	{
		return this.holocode;
	}
	
	public void robotVoicePlay(int i)
	{
		Debug.Log("O");
		AudioSource audioSource = this.audioSource2;
		audioSource.Stop();
		audioSource.clip = robotVoiceClips[i];
		audioSource.loop = false;
		audioSource.Play();
	}
}
