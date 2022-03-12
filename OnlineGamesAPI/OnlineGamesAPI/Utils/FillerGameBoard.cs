namespace OnlineGamesAPI.Utils {
    [Serializable]
    public class FillerGameBoard {
        public class Tile {
            public int team;
            public int color;
            public Tile(int team, int color) {
                this.team = team;
                this.color = color;
            }
        }
        public int size;
        public List<Tile> board;

        public int GetColor(int x, int y) {
            return board[GetIndex(x, y)].color;
        }
        public int GetColor(int index) {
            return board[index].color;
        }

        int GetIndex(int index, int xOff, int yOff) {
            return GetIndex(index % size, index / size, xOff, yOff);
        }
        int GetIndex(int x, int y, int xOff = 0, int yOff = 0) {
            if (x + xOff < 0 || y + yOff < 0 || x + xOff >= size || y + yOff >= size) {
                Console.WriteLine("Out of bounds!");
                Console.WriteLine($"X: {x} Y: {y} XOff: {xOff} YOff: {yOff}");
                return -1;
            }
            return ((y + yOff) * size) + (x + xOff);
        }

        int GetTeam(int x, int y) {
            return GetTeam(y * size + x);
        }
        int GetTeam(int index) {
            return board[index].team;
        }

        public int ExecuteMove(int x, int y, int newColor) {
            return ExecuteMove(y * size + x, newColor);
        }

        public int ExecuteMove(int index, int newColor) {
            int team = GetTeam(index);
            if (board[index].team != team) {
                Console.WriteLine("Seems to be operating on the wrong team...");
                return 0;
            }
            board[index].color = newColor;

            Dictionary<int, bool> tiles = new();
            tiles.Add(index, false);

            // While the dict contains unchecked values
            while (tiles.ContainsValue(false)) {
                // Create a dict to store the newly checked tiles
                Dictionary<int, bool> toAdd = new();
                // Loop through the unchecked tiles
                foreach (var tile in tiles.Keys.ToList()) {
                    // If the tile has already been checked, skip it
                    if (tiles[tile] == true) continue;

                    Dictionary<int, bool> neighbors = GetNeighbors(tile, ref tiles, ref toAdd);
                    foreach (var neighbor in neighbors) {
                        toAdd[neighbor.Key] = neighbor.Value;
                    }
                }
                foreach (var item in toAdd) {
                    tiles[item.Key] = item.Value;
                }
            }

            // Now we have all connected tiles
            // modify the board
            foreach (var tile in tiles) {
                board[tile.Key].team = team;
                board[tile.Key].color = newColor;
            }

            return 0;
        }
        Dictionary<int, bool> GetNeighbors(int index, ref Dictionary<int, bool> tiles, ref Dictionary<int, bool> toAdd) {
            Dictionary<int, bool> neighbors = new();
            List<int> neighborIndices = new List<int>() {
                GetIndex(index, 1, 0),
                GetIndex(index, -1, 0),
                GetIndex(index, 0, 1),
                GetIndex(index, 0, -1)
            };
            int color = GetColor(index);

            for (int i = 0; i < neighborIndices.Count; i++) {
                if (neighborIndices[i] < 0) continue;

                if (color == GetColor(neighborIndices[i]) || GetTeam(index) == board[neighborIndices[i]].team) { // color == GetColor(neighborIndices[i])
                    if (!tiles.ContainsKey(neighborIndices[i])) {
                        neighbors.Add(neighborIndices[i], false);
                        board[neighborIndices[i]].color = color;
                        board[neighborIndices[i]].team = GetTeam(index);
                    }
                }
            }
            neighbors[index] = true;

            return neighbors;
        }
    }
}
