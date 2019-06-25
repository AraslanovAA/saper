using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapgenerate : MonoBehaviour
{
    public static string GameMod;
    public static float timer;
    public static float PlusTime;
    public string CurrentSceneName;
    public Tile tilePrefab;
    private int numberOfTiles;//колиество клеток
    public float distanceBetweenTiles = 5.0f;//расстояние между клетками
    private int tilesPerRow;//элементов в ряду
    private int nummines;//количесвто мин в игре
    public static Tile[] tilesAll;//массив клеток
    public static string state;//отслеживаем состояние игры
    public static int tilesClosed;
    public Canvas canv;//закрыть сохранение результатов
    int allClosed;
    private int level;
    public static bool FirstStepProtection;
    public static bool FirstPress;
    private int record;
    // Use this for initialization
    void Start()
    {
        
        canv.enabled = false;
        FirstPress = false;
        FirstStepProtection = true;
        GameMod = Kommut.TypeGame;
        level = Kommut.Level;

        timer = 6;
        PlusTime = Kommut.DeltaTime;
        numberOfTiles = Kommut.numberOfTiles;
        nummines = Kommut.nummines;
        tilesPerRow = Kommut.tilesPerRow;
        tilesAll = new Tile[numberOfTiles];
        tilesClosed = numberOfTiles - nummines;
        allClosed = tilesClosed;
        //вызываем метод создания поля
        CreateTiles();
        state = "inGame";
        //CollectionPrefs.DeleteStr(GameMod);
    }
    void CheckReckord()
    {
        if(GameMod == "survive")
        {
            Kommut.TimeRecord = timer;
        }
        canv.enabled = true;
    }
    void NextLevel()
    {
        if (state == "Lose")
        {
            
            Kommut.Level = 1;
            Kommut.DeltaTime = 1;
            Kommut.numberOfTiles = 81;
            Kommut.nummines = 10;
            Kommut.tilesPerRow = 9;
        }
        if(state == "gameWon")
        {
            
            Kommut.Level++;
            Kommut.tilesPerRow = 9 + ((Kommut.Level - 1) / 2);
            Kommut.numberOfTiles = Kommut.tilesPerRow * Kommut.tilesPerRow;
            Kommut.DeltaTime = (float)(1 - (float)((int)(Kommut.Level / 5)/ 10));//каждые 5 уровней -0.1с
            float k = 1.2f;
            k += (float)(Kommut.Level / 5);
            if (Kommut.Level % 2 == 0)
                k += 0.1f;
            Kommut.nummines = (int)(Kommut.tilesPerRow * k);
            print(Kommut.nummines);
        }
    }
    void Restart()
    {

        NextLevel();
        UnityEngine.SceneManagement.SceneManager.LoadScene(CurrentSceneName);
        // Application.LoadLevel(Application.loadedLevel);
    }
    void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width - 200, Screen.height - 50, 200, 50), "В меню"))
        {
            state = "Lose";
            NextLevel();
            UnityEngine.SceneManagement.SceneManager.LoadScene("SingleMenu");
        }
        if (GameMod == "classic")
        {
            if (state == "inGame")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "уровень: " + level.ToString() + "\nещё открыть: " + tilesClosed);
            }
            else if (state == "Lose")
            {
                CheckReckord();
                GUI.Box(new Rect(10, 10, 200, 50), "уровень: " + level.ToString() + "\nВы проиграли");
                
                if (GUI.Button(new Rect(10, 70, 200, 50), "Restart"))
                    Restart();
            }
            else if (state == "gameWon")
            {
                GUI.Box(new Rect(10, 10, 200, 50), "уровень: " + level.ToString() + "\nВы выиграли");

                if (GUI.Button(new Rect(10, 70, 200, 50), "Next"))
                    Restart();
            }
        }
        if ((GameMod == "survive")||(GameMod == "swim"))
        {
            if (state == "inGame")
            {
                GUI.Box(new Rect(10, 10, 200, 70), "уровень: " + level.ToString() + "\nещё открыть: " + tilesClosed + "\n время: " + timer);
            }
            else if (state == "Lose")
            {
                GUI.Box(new Rect(10, 10, 200, 70), "уровень: " + level.ToString() + "\nВы проиграли" + "\n время: " + timer);

                if (GUI.Button(new Rect(10, 90, 200, 50), "Restart"))
                    Restart();
            }
            else if (state == "gameWon")
            {
                GUI.Box(new Rect(10, 10, 200, 70), "уровень: " + level.ToString() + "nВы выиграли!" + "\n время: " + timer);
               
                if (GUI.Button(new Rect(10, 90, 200, 50), "Next"))
                    Restart();
            }
        }

        //прикрутить дробь с allClosed
        //GUI.Box(new Rect(10, 10, 100, 50), tilesClosed.ToString(),state);
        //прикрутить сюда И о количесвте флажков и мб таймер?
    }
    // Update is called once per frame
    void ReMined()//в основная функиця режима игры "плавающие мины"
    {
        int detected = 0;
        for (int i = 0; i < tilesAll.Length; i++)
        {
            if ((tilesAll[i].isMined == true) && (tilesAll[i].state == "flagged"))
            {
                detected++;
            }
            if ((tilesAll[i].isMined == true) && (tilesAll[i].state == "closed"))
            {
                tilesAll[i].isMined = false;
            }
        }
        //detected - количество правильно определённых мин
        int needMine = nummines - detected;
        int k, j;
        bool flag;
        while(needMine != 0)
        {
            k = (int)Random.Range(0, numberOfTiles);
            j = 0;
            flag = false;
            int i = 0;
            while (!flag)
            {
                if((tilesAll[i].state == "closed")&&(tilesAll[i].isMined == false)){
                    j++;
                }
                if(j == k)
                {
                    if ((tilesAll[i].state == "closed") && (tilesAll[i].isMined == false))
                    {
                        tilesAll[i].isMined = true;
                        flag = true;
                        needMine--;
                    }
                }
                i++;
                if(i == numberOfTiles) { i = 0; }
            }

        }//вроде как вернули все мины на случайные места, теперь надо пересчитать все числа
        for(int elem =0;elem < numberOfTiles; elem++)
        {
            tilesAll[elem].LookingForNeighbours();
        }
    }
    void Update()
    {
        if ((timer > 0) && (state == "inGame") && (FirstPress == true) && (GameMod != "classic"))
        {
            timer -= Time.deltaTime;
        }
        if ((timer <= 0) && (state == "inGame") && (FirstPress == true) && (GameMod == "survive"))
        {
            timer = 0;
            state = "Lose";
        }
        if ((timer <= 0) && (state == "inGame") && (FirstPress == true) && (GameMod == "swim"))
        {
            timer = 6;
            ReMined();
        }
        if ((tilesClosed <= 0) &&(GameMod !="swim1"))
        {
            tilesClosed = 0;
            FinishGame();
        }
        if (state == "Lose") { 
            CheckReckord();
            //ну шо вот тут гг
        }

    }
    void FinishGame()
    {
        state = "gameWon";
        //отмечаем все оставшиеся мины, если все клетки без мин уже раскрыты
        for (int i = 0; i < tilesAll.Length; i++)
        {
            if ((tilesAll[i].isMined == true) && (tilesAll[i].state == "closed"))
            {
                tilesAll[i].SetFlag();
            }
        }
    }
    void CreateTiles()
    {
        float xOffset = 0.0f;
        float yOffset = 0.0f;

        for (int tilesCreated = 0; tilesCreated < numberOfTiles; tilesCreated++)
        {
            xOffset += distanceBetweenTiles;
            if (tilesCreated % tilesPerRow == 0)
            {
                yOffset += distanceBetweenTiles;
                xOffset = 0;
            }
            Tile newtile = Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), transform.rotation);
            newtile.ID = tilesCreated;//раздаём порядковые номера
            newtile.tilesPerRow = tilesPerRow;
            tilesAll[tilesCreated] = newtile;

        }
        Assignmines();
    }

    void Assignmines()//рандомим мины
    {
        int k;
        int elem;
        bool flag;
        int j;
        for (int i = 0; i < numberOfTiles; i++)
        {
            tilesAll[i].isMined = false;
        }
        for (int i = 0; i < nummines; i++)
        {
            elem = 0;
            j = 0;
            k = (int)Random.Range(0, numberOfTiles);
            flag = false;
            while (flag == false)//в цикле ищем искомую клетку как номер свободной перескакивая через занятые
            {
                if (tilesAll[j].isMined == false)//если мины нет, то учитываем, если есть то в нахождении искомой клетки не учавстувует
                {
                    if (elem == k)
                    {
                        tilesAll[j].isMined = true;
                        flag = true;
                    }
                    else
                    {
                        elem++;
                    }
                }
                j++;
                if (j == numberOfTiles) { j = 0; }
            }
        }
    }
}

