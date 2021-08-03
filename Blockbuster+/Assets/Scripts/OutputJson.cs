using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputJson {
    public List<Title> Media { get; set; }

}

public class Title 
{
    public string Name { get; set; }
    public List<string> Genres { get; set; }
}
