using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMTC.Model
{
    public class Match
    {
        List<MatchMap> Maps;
        string RedTeam;
        string BlueTeam;

        public Match(string blueTeam, string redTeam)
        {
            this.Maps = new List<MatchMap>();
            this.RedTeam = redTeam;
            this.BlueTeam = blueTeam;
        }

        public void FillMaps(JArray mJSON)
        {
            for (int i = 0; i < mJSON.Count; i++){
                JArray mapJSON = APIAccessor.RetrieveMapDataAsync(mJSON[i]["beatmap_id"].ToString()).Result;
                string mapID = mJSON[i]["beatmap_id"].ToString();
                string setID = MapIDFromJArray(mapJSON);
                string mapName = MapNameFromJArray(mapJSON, mapID);
                MatchMap map = new MatchMap(mapID, setID, mapName);
                    JArray scoreArray = (JArray)mJSON[i]["scores"];
                    for (int k = 0; k < scoreArray.Count; k++)
                    {
                        int score = Int32.Parse(scoreArray[k]["score"].ToString());
                        if (scoreArray[k]["team"].ToString().Equals("1"))
                        {
                            if(scoreArray[k]["pass"].ToString().Equals("0"))
                            {                               
                                map.AddScore(new PlayerScore(score, Team.Blue, false));
                            }
                            else
                            {
                                map.AddScore(new PlayerScore(score, Team.Blue, true));
                            }
                        }

                        else
                        {
                            if (scoreArray[k]["pass"].ToString().Equals("0"))
                            {
                                map.AddScore(new PlayerScore(score, Team.Red, false));
                            }
                            else
                            {
                                map.AddScore(new PlayerScore(score, Team.Red, true));
                            }
                        }
                    }
                
                map.ComputeTeamScores();
                Maps.Add(map);
            }
        }

        public string MapIDFromJArray(JArray mapJSON)
        {
            return mapJSON[0]["beatmapset_id"].ToString();
        }

        public string MapNameFromJArray(JArray mapJSON, string mapID)
        {
            for (int i=0; i<mapJSON.Count; i++)
            {
                if (mapJSON[i]["beatmap_id"].ToString() == mapID)
                {
                    return mapJSON[i]["artist"].ToString() + " - " + mapJSON[i]["title"].ToString() + " [" + mapJSON[i]["version"].ToString() + "]";
                }
            }
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            int set = 1;
            int blueWins = 0;
            int redWins = 0;
            string matchTable = "";


            //Add all map results
            foreach (MatchMap map in Maps)
            {
                //Add new table header for new set
                if (redWins == 0 && blueWins == 0)
                {
                    matchTable = matchTable + "#Set " + set.ToString() + "\n \n";

                    matchTable = matchTable + "Mod|Map | " + BlueTeam + " | x | x | " + RedTeam + " | Point Difference\n"
                        + "---|---|-----------|------------|------------|-----------|----------------\n";
                }
                //Add mods
                matchTable = matchTable + map.Mod.ToString() + "|";  
                //Add map name and link
                matchTable = matchTable + "[" + map.mapName + "](" + map.mapLink + ")| ";
                //Add scores and winner
                if (map.BlueScore > map.RedScore)
                {
                    matchTable = matchTable + "**" + map.BlueScore.ToString("#,##0") + "** | ✓ |  | " + map.RedScore.ToString("#,##0") + "| ";
                    int scoreDifference = map.BlueScore - map.RedScore;
                    matchTable = matchTable + BlueTeam + " wins by " + scoreDifference.ToString("#,##0");
                    blueWins++;
                }
                else
                {
                    matchTable = matchTable + map.BlueScore.ToString("#,##0") + " |  | ✓ | **" + map.RedScore.ToString("#,##0") + "**| ";
                    int scoreDifference = map.RedScore - map.BlueScore;
                    matchTable = matchTable + RedTeam + " wins by " + scoreDifference.ToString("#,##0");
                    redWins++;
                }
                matchTable = matchTable + "\n";

                if(redWins==4 || blueWins == 4)
                {
                    set++;
                    redWins = 0;
                    blueWins = 0;
                }
            }
            return matchTable;
        }
    }
}
