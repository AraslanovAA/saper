using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionPrefs
{
    public static void DeleteStr(string key)
    {
        int count = PlayerPrefs.GetInt(key + ".Count", 0);
        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey(key + "[" + i + "]");
        }
    }
    public static void SetStrings(string key, string[] collection)
    {
        //удалить старый рейтинг
        int count = PlayerPrefs.GetInt(key + ".Count",0); 
        for (int i =0; i < count; i++)
        {
            PlayerPrefs.DeleteKey(key + "[" + i + "]");
        }
        //сохранить новый рейтинг
        PlayerPrefs.SetInt(key + ".Count", collection.Length);
        for(int i = 0; i < collection.Length; i++)
        {
            PlayerPrefs.SetString(key + "[" + i + "]", collection[i]);
        }
    }
    public static string[] GetStrings(string key)
    {
        int count = PlayerPrefs.GetInt(key + ".Count", 0);
        string[] array = new string[count];
        for(int i =0; i< count; i++)
        {
            array[i] = PlayerPrefs.GetString(key + "[" + i + "]");
            
        }
        return array;
    }
    public static void AddResult(string name, string key, int level, float timer = 0)
    {
        string[] ArrRes = GetStrings(key);
        if (ArrRes != null)
        {
            if((key == "classic")||(key =="swim"))
            {
                int position = -1; 
                for(int i = 0;i< ArrRes.Length; i++)
                {
                    if (ArrRes[i] != "")
                    {
                        string[] vararr = ArrRes[i].Split(' ');
                        if (level < int.Parse(vararr[vararr.Length - 1]))
                        //if( level < ArrRes.Length -1)
                        {
                            position = i;
                        }
                    }
                }
                position++;
                string[] NewRes = new string[ArrRes.Length + 1];
                bool flag = false;
                for(int i = 0;i < NewRes.Length; i++)
                {
                    if(i == position)
                    {
                        NewRes[i] = name + " " + level.ToString();
                        flag = true;
                    }
                    else
                    {
                        if(flag == false)//ещё не надо +1 делать так как не впихивали ничего
                        {
                            NewRes[i] = ArrRes[i];
                        }
                        else
                        {
                            NewRes[i] = ArrRes[i - 1];
                        }
                    }
                }
                SetStrings(key, NewRes);
            }
            if (key == "survive")
            {
                int position = -1;
                for (int i = 0; i < ArrRes.Length; i++)
                {
                    if (ArrRes[i] != "")
                    {
                        string[] vararr = ArrRes[i].Split(' ');
                        if (level < int.Parse(vararr[vararr.Length - 1]))
                        //if( level < ArrRes.Length -1)
                        {
                                position = i;
                        }
                    }
                }
                position++;
                for(int i = position; i < ArrRes.Length; i++)
                {
                    if (ArrRes[i] != "")
                    {
                        string[] vararr = ArrRes[i].Split(' ');
                        if (level == int.Parse(vararr[vararr.Length - 1]))
                        {
                            if(timer < float.Parse(vararr[vararr.Length - 2]))
                            {
                                position = i;
                            }
                        }
                    }
                }
                position++;
                string[] NewRes = new string[ArrRes.Length + 1];
                bool flag = false;
                for (int i = 0; i < NewRes.Length; i++)
                {
                    if (i == position)
                    {
                        NewRes[i] = name + " " +  timer.ToString() + " " +level.ToString();
                        flag = true;
                    }
                    else
                    {
                        if (flag == false)//ещё не надо +1 делать так как не впихивали ничего
                        {
                            if (i < ArrRes.Length)
                            {
                                NewRes[i] = ArrRes[i];
                            }
                            else
                            {
                                NewRes[i] = "";
                            }
                        }
                        else
                        {
                            NewRes[i] = ArrRes[i - 1];
                        }
                    }
                }
                SetStrings(key, NewRes);
            }
        }
        else
        {// первая запись
            string[] Res = new string[1];
            if(key == "survive")
            {
                Res[0] = name + " " + timer.ToString() + " " + level.ToString();
            }
            if (key == "classic")
            {
                Res[0] = name + " "  + level.ToString();
            }
            SetStrings(key, Res);
        }
    }
}
