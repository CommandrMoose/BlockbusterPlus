using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    public string pathToHTML;

    [Header("Titles")]
    public string titleToMatch;
    public string[] titleCharToRemove;
    
    [Header("Genres")]
    [Space]
    public string genreToMatch;
    public string[] generCharToRemove;

    private string output = @"E:\Projects\Blockbuster\output.json";

    // Start is called before the first frame update
    void Start()
    {
        GetDataFromHTML();
    }

    void GetDataFromHTML() 
    {
        var rawHTML =  File.ReadAllText(pathToHTML);

        var titles = new List<Title>();

        // Regex for Title
        var rxTitle = new Regex(titleToMatch, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
        // Regex for Genrers
        var rxGenres = new Regex(genreToMatch);

        var titleCollection = rxTitle.Matches(rawHTML);
        var genreCollection = rxGenres.Matches(rawHTML);


        for (int i = 0; i < titleCollection.Count; i++) 
        {
            var title = titleCollection[i];
            var titleObj = new Title();

            // Cleanup the Name of the Media
            var name = title.Value;
            foreach (var removeTerm in titleCharToRemove)
            {
                name = name.Replace(removeTerm, "");
            }
            titleObj.Name = name;
            Debug.Log(name);

            // Cleanup the Genre of the Media
            var genres = genreCollection[i].Value;

            var listOfGenres = new List<string>();
            if (genres.Contains(","))
            {
                var items = genres.Split(',');

                foreach (var word in items)
                {
                    var tempWord = word.Trim();
                    foreach (var remove in generCharToRemove)
                    {
                        tempWord = tempWord.Replace(remove, "").Trim();
                    }

                    listOfGenres.Add(tempWord);

                }
            }
            else 
            {
                var tempWord = genres.Trim();
                foreach (var remove in generCharToRemove)
                {
                    tempWord = tempWord.Replace(remove, "").Trim();
                }

                listOfGenres.Add(tempWord);
            }

            titleObj.Genres = listOfGenres;


            titles.Add(titleObj);
        }



        titles.ForEach(x => Debug.Log(x.Name));

        var outputJSON = new OutputJson() { Media = titles };

        var jsonText = JsonConvert.SerializeObject(outputJSON);

        File.WriteAllText(output, jsonText);
    }

}
