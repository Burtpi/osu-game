using System.IO;
using System;
using System.Collections.Generic;

namespace osu__Game
{
    public class cBeatmap
    {
        public double Length;

        public string ReadFile(List<cHitObject> aHitObjects, int aMapId)
        {
            var mapNumber = 0;
            var intValue = false;
            var lastObj = new cCircle(0, 0, 0);
            var dirs = Directory.GetDirectories("map/", "*", SearchOption.TopDirectoryOnly);
            var filePaths = Directory.GetFiles(dirs[aMapId-1], "*.osu", SearchOption.TopDirectoryOnly);
            for (var i = 0; i < filePaths.Length; i++)
                Console.WriteLine($"{i+1}. {filePaths[i][(filePaths[i].Split()[0].Length + 1)..]}");
            while (mapNumber > filePaths.Length || mapNumber <= 0 || intValue == false)
                intValue = int.TryParse(Console.ReadLine(), out mapNumber);
            File.Move(filePaths[mapNumber-1], Path.ChangeExtension(filePaths[mapNumber-1], ".txt"));
            filePaths = Directory.GetFiles(dirs[aMapId-1], "*.txt", SearchOption.TopDirectoryOnly);
            Console.WriteLine(filePaths.Length);
            var lines = File.ReadAllLines(filePaths[0]);
            File.Delete(filePaths[0]);
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
                    audioPath = obj[(obj.Split()[0].Length + 1)..];
            }

            Length += lastObj.mTime + 2000;
            return audioPath;
        }
    }
}