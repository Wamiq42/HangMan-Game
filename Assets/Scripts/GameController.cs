using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class GameController : ButtonController
{
    public Text gameTimer;
    public Text guessWord , numberOfLetters, enteredLetter;
    public GameObject[] Health;
    public GameObject winText, loseText;
    public GameObject hangNinja;
    public GameObject restart;
    public AudioSource[] Music;
    public AudioSource loseLife;
    private Dictionary<KeyCode, bool> keys = new Dictionary<KeyCode, bool>();
    private AudioSource playMusic;
    private float time;
    private string[] words = File.ReadAllLines(@"Assets/Words.txt");
    private string chosenWord, hiddenWord;
    private int fail;
    private bool gameState = false;

    // Start is called before the first frame update
    void Start()
    {
        playMusic = Music[Random.Range(0, Music.Length)];
        playMusic.Play();
        chosenWord = words[Random.Range(0, words.Length)];
        int letterlength = chosenWord.Length;
        for (int i = 0; i < chosenWord.Length; i++)
        {
            char letter = chosenWord[i];
            if(char.IsWhiteSpace(letter))
            {
                hiddenWord += "-";
            }
            else
            {
                hiddenWord += "_";
            }
            
        }
        numberOfLetters.text = "No. of Letters in this Word : " + letterlength.ToString();
        guessWord.text = hiddenWord;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == false)
        {
            time += Time.deltaTime;
            float minutes = Mathf.Floor(time / 60);
            float seconds = Mathf.RoundToInt(time % 60);
            gameTimer.text = minutes.ToString() + " : " + seconds.ToString();
        }
        if(!playMusic.isPlaying)
        {
            playMusic = Music[Random.Range(0,Music.Length)];
            playMusic.Play();
        }

      
    }
    
    private void OnGUI()
    {
        Event presentEvent = Event.current;
        if (presentEvent.type == EventType.KeyDown && presentEvent.keyCode.ToString().Length == 1)
        {
        
            string pressedLetter = presentEvent.keyCode.ToString();
            enteredLetter.text = "You entered : " + pressedLetter;
            if (chosenWord.Contains(pressedLetter))
            {
         
                int a = chosenWord.IndexOf(pressedLetter);
                while(a!=-1)
                {
                    hiddenWord = hiddenWord.Substring(0, a) + pressedLetter + hiddenWord.Substring(a + 1);
                    Debug.Log(hiddenWord);
                    chosenWord = chosenWord.Substring(0, a) + "_" + chosenWord.Substring(a + 1);
                    Debug.Log(chosenWord);
                    a = chosenWord.IndexOf(pressedLetter);
                }
                guessWord.text = hiddenWord;
            }
            else
            {
                Health[fail].SetActive(false);
                loseLife.Play();
                fail++;
            }
            if (!hiddenWord.Contains("_"))
            {
                gameState = true;
                winText.SetActive(true);
                winText.GetComponent<AudioSource>().Play();
                restart.SetActive(true);
            }
            if(fail == 6 )
            {
                
                hangNinja.GetComponent<Rigidbody>().isKinematic = false;
                playMusic.Stop();
                gameState = true;
                restart.SetActive(true);
                loseText.SetActive(true);
                loseText.GetComponent<AudioSource>().Play();
                
            }
        }

    }
   

}
