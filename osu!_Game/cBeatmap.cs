using System.IO;
using System;
using System.Collections.Generic;

namespace osu__Game
{
    public class cBeatmap
    {
        public double Length;

        public string ReadFile(List<cHitObject> aHitObjects)
        {
            var lastObj = new cCircle(0, 0, 0);
            var lines = File.ReadAllLines("map/map.txt");
            var isHitObjects = false;
            var audioPath = "";
            foreach (var obj in lines)
            {
                if (obj == "[HitObjects]")
                {
                    isHitObjects = true;
                    continue;
                }

                if (!isHitObjects) continue;
                var a = obj.Split(',');
                aHitObjects.Add(new cHitObject(Convert.ToInt32(a[0]), Convert.ToInt32(a[1]), Convert.ToInt32(a[2])));
                lastObj = new cCircle(Convert.ToInt32(a[0]), Convert.ToInt32(a[1]), Convert.ToInt32(a[2]));
            }
            aHitObjects.Reverse();
            foreach (var obj in lines)
            {
                foreach (var hitObject in aHitObjects)
                {
                    if(obj.Contains("CircleSize"))
                        hitObject.SetSizeHb(obj[11] - '0');
                    if(obj.Contains("ApproachRate"))
                        hitObject.SetTimeSpanHb(obj[13] - '0');
                }
                if (obj.Contains("AudioFilename"))
                {
                    audioPath = obj.Split(' ')[1];
                }
            }

            Length += lastObj.mTime + 2000;
            return audioPath;
        }
    }
}