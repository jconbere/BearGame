using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BearGame
{
    public class Layer
    {
        List<string> _rows;

        public int NumColumns { get { return _rows != null && _rows.Count > 0 ? _rows[0].Length : 0; } }
        public int NumRows { get { return _rows.Count; } }

        public char GetTile(int column, int row)
        {
            return _rows[row][column];
        }

        public void LoadTiles(string path)
        {
			var cleanPath = path.Replace ('\\', System.IO.Path.DirectorySeparatorChar);
            using (var reader = File.OpenText(cleanPath))
            {
                _rows = new List<string>();
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    _rows.Add(line);
                }
            }
        }
    }
}
