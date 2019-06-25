using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public string level;
    public string GameMod;
    public InputField InputName;
    public Text first;
    public Text second;
    public Text third;
    public Text fourth;
    public Text fifth;
    public Canvas canv;
    public Canvas canv1;
    public Canvas canv2;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void TurnF()
    {
        canv1.gameObject.SetActive(true);
        canv2.gameObject.SetActive(false);
    }
    public void TurnS()
    {
        canv1.gameObject.SetActive(false);
        canv2.gameObject.SetActive(true);
    }
    public void ShowStat()
    {
        string[] show = new string[5];
        for(int i =0; i < 5; i++)
        {
            show[i] = "";
        }
        string[] arr = CollectionPrefs.GetStrings(GameMod);
        if (arr != null)
        {
            int k = 0;
            for(int i = 0; i < arr.Length; i++)
            {
                if((arr[i] != "")&&(arr[i] !=" "))
                {
                    bool flag = false;
                    //foreach (char item in arr[i])
                    //{
                    //    if (item != ' ')
                    //        flag = true;
                    //}
                    if ((k < 5)&&(flag == false))
                    {
                        show[k] = arr[i];
                        k++;
                    }
                }
            }
            if(show[0]!= null)
            {
                print("'" + show[0] + "'");
                first.text = "1. " +show[0];
            }
            if (show[1] != null)
            {
                second.text = "2. " + show[1];
            }
            if (show[2] != null)
            {
                third.text = "3. " + show[2];
            }
            if (show[3] != null)
            {
                fourth.text = "4. " + show[3];
            }
            if (show[4] != null)
            {
                fifth.text = "5. " + show[4];
            }
        }
    }
    public void Statistic()
    {
        CollectionPrefs.AddResult(InputName.text, Kommut.TypeGame, Kommut.Level, Kommut.TimeRecord);
        string[] array = CollectionPrefs.GetStrings(Kommut.TypeGame);
        //CollectionPrefs.DeleteStr("classic");
        
        ContinueGame();
    }
    public void ContinueGame()
    {
        canv.enabled = false;
        canv.gameObject.SetActive(false);
    }
	public void StartGame()
	{
        if(level == "single1")
        {
            Kommut.TypeGame = GameMod;
            
        }
        SceneManager.LoadScene(level);
		//Application.LoadLevel (level);
	}
	public void ExitGame()
	{
		Application.Quit ();
	}
	//Настройки
	public void LowQuality()
	{
		QualitySettings.SetQualityLevel (0,true);
	}
	public void MediumQuality()
	{
		QualitySettings.SetQualityLevel (2,true);
	}
	public void UltraQuality()
	{
		QualitySettings.SetQualityLevel (4,true);
	}
}