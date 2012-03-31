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

        public int Width { get { return _rows != null && _rows.Count > 0 ? _rows[0].Length : 0; } }
        public int Height { get { return _rows.Count; } }

        protected char GetTile(int column, int row)
        {
            return _rows[row][column];
        }

        protected void LoadTiles(string path)
        {
            using (var reader = File.OpenText(path))
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
