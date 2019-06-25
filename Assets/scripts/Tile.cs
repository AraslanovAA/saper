using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool isMined;//заминирована или нет
    public TextMesh displayText;//текст на клетке
    public Material materialIdle;//обычная клетка
    public Material materialLightup;//наведенная
    public Material materialUncovered;//раскрытая
    public Material materialDetonated;//boom
    private MeshRenderer rend;
    private Renderer rendFlag;
    private Renderer rendText;
    public int ID;//id клетки в grid
    public int tilesPerRow;
    public string state;//текущее состояние клетки{closed,flagged, uncovered}
    public GameObject displayFlag;//получаем доступ к флажку

    public Tile tileUpper;
    public Tile tileLower;
    public Tile tileLeft;
    public Tile tileRight;

    public Tile tileUpperRight;
    public Tile tileUpperLeft;
    public Tile tileLowerRight;
    public Tile tileLowerLeft;
    public Tile[] neighbours = new Tile[8];
    public int adjacentMines = 0;

    public void LookingForNeighbours()
    {
        adjacentMines = 0;
        
        rendFlag = displayFlag.GetComponent<Renderer>();
        rendFlag.enabled = false;
        rendText = displayText.GetComponent<Renderer>();
        rendText.enabled = false;
        rend = gameObject.GetComponent<MeshRenderer>();
        //каждая клетка имеет поля для хранения ссылок на соседние клетки, добавление ссылок
        if (Bounds(Mapgenerate.tilesAll, ID + tilesPerRow)) tileUpper = Mapgenerate.tilesAll[ID + tilesPerRow];

        if (Bounds(Mapgenerate.tilesAll, ID - tilesPerRow)) tileLower = Mapgenerate.tilesAll[ID - tilesPerRow];
        if (Bounds(Mapgenerate.tilesAll, ID - 1) && ID % tilesPerRow != 0) tileLeft = Mapgenerate.tilesAll[ID - 1];
        if (Bounds(Mapgenerate.tilesAll, ID + 1) && (ID + 1) % tilesPerRow != 0) tileRight = Mapgenerate.tilesAll[ID + 1];

        if (Bounds(Mapgenerate.tilesAll, ID + tilesPerRow + 1) && (ID + 1) % tilesPerRow != 0) tileUpperRight = Mapgenerate.tilesAll[ID + tilesPerRow + 1];
        if (Bounds(Mapgenerate.tilesAll, ID + tilesPerRow - 1) && ID % tilesPerRow != 0) tileUpperLeft = Mapgenerate.tilesAll[ID + tilesPerRow - 1];
        if (Bounds(Mapgenerate.tilesAll, ID - tilesPerRow + 1) && (ID + 1) % tilesPerRow != 0) tileLowerRight = Mapgenerate.tilesAll[ID - tilesPerRow + 1];
        if (Bounds(Mapgenerate.tilesAll, ID - tilesPerRow - 1) && ID % tilesPerRow != 0) tileLowerLeft = Mapgenerate.tilesAll[ID - tilesPerRow - 1];
        neighbours[0] = tileUpper;
        neighbours[1] = tileLower;
        neighbours[2] = tileLeft;
        neighbours[3] = tileRight;
        neighbours[4] = tileUpperRight;
        neighbours[5] = tileUpperLeft;
        neighbours[6] = tileLowerRight;
        neighbours[7] = tileLowerLeft;
        for (int i = 0; i < 8; i++)
        {
            if (neighbours[i] != null)
            {
                if (neighbours[i].isMined == true)
                {
                    adjacentMines++;
                }
            }
        }
        if (adjacentMines != 0)
        {
            if (state == "uncovered")
            {
                rendText.enabled = true;
            }
            displayText.text = adjacentMines.ToString();//текст отображается, но enabled = false
        }
        else
        {
            
            displayText.text = "";
        }
        if (state == "flagged")
        {
            rendFlag.enabled = true;
        }
    }
    // Use this for initialization
    void Start () {
        state = "closed";
        LookingForNeighbours();
        
    }

    private bool Bounds(Tile[] arr, int targetID)//првоерка использования элемента, дабы индекс не вышел за границы
    {
        if (targetID < 0 || targetID >= arr.Length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void SetFlag()//снимаем/устанавливаем флажок
    {
        if (state == "closed")
        {
            state = "flagged";
            rendFlag.enabled = true;
        }
        else if (state == "flagged")
        {
            state = "closed";
            rendFlag.enabled = false;
        }
    }
    void Explode()//тупа взарвался
    {
        if (Mapgenerate.FirstStepProtection == false)
        {
            for (int i = 0; i < Mapgenerate.tilesAll.Length; i++)
            {
                if (Mapgenerate.tilesAll[i].isMined == true)
                {
                    Mapgenerate.tilesAll[i].ShowExplode();//если клетка заминирована
                }
            }
        }
        else
        {
            bool flag = false;//открыть клетку
            int I = 1;
            while(flag == false)//поставили мину на i-ую клетку
            {
                I++;
                if(Mapgenerate.tilesAll[I].isMined == false)
                {
                    Mapgenerate.tilesAll[I].isMined = true;
                    flag = true;
                }
            }
            //убираем мину с нашей клетки
            isMined = false;
            //для каждой клетки соседней с нашей пересчитать количесвто мин по соседству
            if (Bounds(Mapgenerate.tilesAll, ID + tilesPerRow)) Mapgenerate.tilesAll[ID + tilesPerRow].LookingForNeighbours();

            if (Bounds(Mapgenerate.tilesAll, ID - tilesPerRow)) Mapgenerate.tilesAll[ID - tilesPerRow].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, ID - 1) && ID % tilesPerRow != 0)  Mapgenerate.tilesAll[ID - 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, ID + 1) && (ID + 1) % tilesPerRow != 0) Mapgenerate.tilesAll[ID + 1].LookingForNeighbours();

            if (Bounds(Mapgenerate.tilesAll, ID + tilesPerRow + 1) && (ID + 1) % tilesPerRow != 0) Mapgenerate.tilesAll[ID + tilesPerRow + 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, ID + tilesPerRow - 1) && ID % tilesPerRow != 0) Mapgenerate.tilesAll[ID + tilesPerRow - 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, ID - tilesPerRow + 1) && (ID + 1) % tilesPerRow != 0)  Mapgenerate.tilesAll[ID - tilesPerRow + 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, ID - tilesPerRow - 1) && ID % tilesPerRow != 0)  Mapgenerate.tilesAll[ID - tilesPerRow - 1].LookingForNeighbours();
            LookingForNeighbours();
            //для каждой клекти соседней с той в которую поставили мину, пересчить количество мин по соседству
            if (Bounds(Mapgenerate.tilesAll, I + tilesPerRow)) Mapgenerate.tilesAll[I + tilesPerRow].LookingForNeighbours();

            if (Bounds(Mapgenerate.tilesAll, I - tilesPerRow)) Mapgenerate.tilesAll[I - tilesPerRow].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, I - 1) && I % tilesPerRow != 0) Mapgenerate.tilesAll[I - 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, I + 1) && (I + 1) % tilesPerRow != 0) Mapgenerate.tilesAll[I + 1].LookingForNeighbours();

            if (Bounds(Mapgenerate.tilesAll, I + tilesPerRow + 1) && (I + 1) % tilesPerRow != 0) Mapgenerate.tilesAll[I + tilesPerRow + 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, I + tilesPerRow - 1) && I % tilesPerRow != 0) Mapgenerate.tilesAll[I + tilesPerRow - 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, I - tilesPerRow + 1) && (I + 1) % tilesPerRow != 0) Mapgenerate.tilesAll[I - tilesPerRow + 1].LookingForNeighbours();
            if (Bounds(Mapgenerate.tilesAll, I - tilesPerRow - 1) && I % tilesPerRow != 0) Mapgenerate.tilesAll[I - tilesPerRow - 1].LookingForNeighbours();
            UncoverTile();//после чего все же раскрываем клетку
        }
    }
    private void ShowExplode()//при проигрыше клетка расскжает, чт ов ней была мина
    {
        state = "detonated";
        Mapgenerate.state = "Lose";
        rend.material = materialDetonated;
    }

    private void UncoverAdjacentTiles()//2 функции для раскрытия при нажатии на несоседствующую с минами лкетку
    {
        for(int i = 0; i < 8; i++)
        {
            if (neighbours[i] != null)
            {
                //раскрываем всех безопасных соседей (количество мин вокруг которых равно 0)
                if (!neighbours[i].isMined && neighbours[i].state == "closed" && neighbours[i].adjacentMines == 0)
                    neighbours[i].UncoverTile();

                //раскрываем всех небезопасных соседей и останавливаемся
                else if (!neighbours[i].isMined && neighbours[i].state == "closed" && neighbours[i].adjacentMines > 0)
                    neighbours[i].UncoverTileExternal();
            }
        }
    }
    public void UncoverTileExternal()
    {
        if (state != "flagged")
        {
            if (Mapgenerate.GameMod == "survive")
            {
                Mapgenerate.timer += Mapgenerate.PlusTime;
            }
            state = "uncovered";
            rendText.enabled = true;
            rend.material = materialUncovered;
            Mapgenerate.tilesClosed--;
        }
    }
    void UncoverTile()//раскрываем клетку
    {
        if (!isMined)
        {
            if (state != "flagged")
            {
                if (Mapgenerate.GameMod == "survive")
                {
                    Mapgenerate.timer += Mapgenerate.PlusTime;
                }
                state = "uncovered";
                rendText.enabled = true;
                rend.material = materialUncovered;
                Mapgenerate.tilesClosed--;
                if (adjacentMines == 0)
                {
                    Mapgenerate.FirstStepProtection = false;
                    UncoverAdjacentTiles();
                }
            }
        }
        else
            Explode();
    }
    void  OnMouseEnter()//при наведении меняется цвет клетки
    {
        if (Mapgenerate.state == "inGame")
        {
            if ((state == "closed") || (state == "flagged"))
            {
                rend.material = materialLightup;
                if (Input.GetMouseButtonDown(1))
                {
                    SetFlag();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (state != "flagged")
                    {
                        Mapgenerate.FirstPress = true;
                        UncoverTile();
                    }
                }
            }
        }
    }
    void OnMouseOver()//при удержании меняется цвет клетки
    {
        if (Mapgenerate.state == "inGame")
        {
            if ((state == "closed") || (state == "flagged"))
            {
                rend.material = materialLightup;
                if (Input.GetMouseButtonDown(1))
                {
                    SetFlag();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Mapgenerate.FirstPress = true;
                    UncoverTile();
                }
            }
        }
    }
    void OnMouseExit()//вернуть цвет
    {
        if (Mapgenerate.state == "inGame")
        {
            if (state == "closed" || state == "flagged")
                rend.material = materialIdle;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
