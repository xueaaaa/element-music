namespace ElementMusic.Models.MusixmatchAPI
{
    internal static class LyricsParser
    {
        public static List<Lyrics> Parse(string[] input)
        {
            List<Lyrics> lyr = new List<Lyrics>();  

            foreach (var line in input)
            {
                if(line == string.Empty) continue;

                var parts = line.Split('[', ']');
                var timeMark = parts[1].Split(':', '.');

                Lyrics lyrics = new()
                {
                    TimeMark = new TimeOnly(0, Convert.ToInt32(timeMark[0]), Convert.ToInt32(timeMark[1]), Convert.ToInt32(timeMark[2])),
                    Content = parts[2]
                };

                lyr.Add(lyrics);
            }

            return lyr;
        }
    }
}
