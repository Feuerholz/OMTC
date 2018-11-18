using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMP.Model;
using OMP;

namespace OMTC
{
    public static class CorsaceOpen2018ThreadCreator
    {
        public static string CreateThreadText(Match match)
        {
            int set = 1;
            int mapsleft = 7;
            int blueWins = 0;
            int redWins = 0;
            string matchTable = "";

            matchTable = matchTable + AddThreadStart(match);

            //Add all map results
            foreach (MatchMap map in match.Maps)
            {
                //Add ban template and table header for new set
                if (redWins == 0 && blueWins == 0)
                {
                    matchTable = matchTable + "---\n";
                    matchTable = matchTable + "#Bans | Set " + set + "\n\n";

                    matchTable = matchTable + "######" + match.BlueTeam + "\n";
                    matchTable = matchTable + "**NM** | ARTIST - TITLE [DIFFICULTY]\n\n";
                    matchTable = matchTable + "**NM** | ARTIST - TITLE [DIFFICULTY]\n\n";

                    matchTable = matchTable + "######" + match.RedTeam + "\n";
                    matchTable = matchTable + "**NM** | ARTIST - TITLE [DIFFICULTY]\n\n";
                    matchTable = matchTable + "**NM** | ARTIST - TITLE [DIFFICULTY]\n\n";

                    matchTable = matchTable + "---\n\n";


                    matchTable = matchTable + "#Set " + set.ToString() + "\n \n";

                    matchTable = matchTable + "Mod|Map | " + match.BlueTeam + " | " + match.Sets.ElementAt(set - 1).BlueScore + " | " + match.Sets.ElementAt(set - 1).RedScore + " | " + match.RedTeam + " | Point Difference\n"
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
                    matchTable = matchTable + match.BlueTeam + " wins by " + scoreDifference.ToString("#,##0");
                    blueWins++;
                }
                else
                {
                    matchTable = matchTable + map.BlueScore.ToString("#,##0") + " |  | ✓ | **" + map.RedScore.ToString("#,##0") + "**| ";
                    int scoreDifference = map.RedScore - map.BlueScore;
                    matchTable = matchTable + match.RedTeam + " wins by " + scoreDifference.ToString("#,##0");
                    redWins++;
                }
                matchTable = matchTable + "\n";
                mapsleft--;

                if (redWins == 4 || blueWins == 4)
                {
                    while (mapsleft > 1)
                    {
                        matchTable = matchTable + "---|---|-----------|-|-|------------|------------\n";
                        mapsleft--;
                    }
                    if (mapsleft == 1)
                    {
                        matchTable = matchTable + "TB|---|-----------|-|-|------------|------------\n";
                    }
                    set++;
                    redWins = 0;
                    blueWins = 0;
                    mapsleft = 7;
                }
            }
            return matchTable;
        }

        public static string AddThreadStart(Match match)
        {
            string output = "";

            //bold name of winner team
            if (match.RedScore > match.BlueScore)
            {
                output += "##" + match.BlueTeam + " | " + match.BlueScore + "-" + match.RedScore + " | " + "**" + match.RedTeam + "**" + "\n";
            }
            else
            {
                output += "##" + "**" + match.BlueTeam + "**" + " | " + match.BlueScore + "-" + match.RedScore + " | " + match.RedTeam + "\n";
            }

            //Add set scores
            int i = 0;
            foreach (Set set in match.Sets)
            {
                i++;
                output = output + "Set " + i + ": " + set.BlueScore + "-" + set.RedScore + " \n\n";
            }

            output = output + "[Match Page](https://open.corsace.io/match/bracket/XX) \n\n";             //add template for match page link

            //add dividers and static links
            output = output + "---\n";
            output = output + "Link: [Corsace Open](https://open.corsace.io/)\n\n";
            output = output + "Link: [Corsace Open 2018 Livestream](https://www.twitch.tv/corsace/)\n\n";
            output = output + "Link: [Corsace Open 2018 Matches Schedule](https://open.corsace.io/schedule)\n\n";
            output = output + "Link: [Corsace Open 2018 Bracket](https://challonge.com/corsace18)\n\n";

            //add dividers and team links
            output = output + "---\n";
            output = output + "Link: [" + match.BlueTeam + "'s Roster](https://open.corsace.io/team/" + match.BlueTeam.Replace(' ', '-') + ")\n\n";
            output = output + "Link: [" + match.RedTeam + "'s Roster](https://open.corsace.io/team/" + match.RedTeam.Replace(' ', '-') + ")\n\n";

            //add dividers, mp link, and chat log link template
            output = output + "---\n";
            output = output + "Link: [MP Link](https://osu.ppy.sh/community/matches/" + match.MatchID + "/)\n\n";
            output = output + "Link: [Chat Log](https://pastebin.com/XXXXXXXXX)\n\n";

            return output;
        }
    }
}
