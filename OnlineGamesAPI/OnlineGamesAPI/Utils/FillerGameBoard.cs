namespace OnlineGamesAPI.Utils {
    [Serializable]
    public class FillerGameBoard {
        public int size;
        public List<int> board;

        public int GetColor(int index) {
            return board[index];
        }

        public int GetColor(int x, int y) {
            return board[GetIndex(x, y)];
        }

        int GetIndex(int index, int xOff, int yOff) {
            return GetIndex(index % size, index / size, xOff, yOff);
        }

        int GetIndex(int x, int y, int xOff = 0, int yOff = 0) {
            return ((y + yOff) * size) + (x + xOff);
        }

        public int ExecuteMove(int x, int y, int newColor) {
            return ExecuteMove(y * size + x, newColor);
        }

        public int ExecuteMove(int index, int newColor) {
            /*
            #         x  y   checked
            tiles = {(x, y): False}
            while False in tiles.values():
                to_add = {}
                for tile in tiles:
                    if not tiles[tile]:
                        self.get_neighbors(tile[0], tile[1], tiles, to_add, player)
                tiles.update(to_add)
            return tiles
            */

            Dictionary<int, bool> tiles = new Dictionary<int, bool>();
            board[index] = newColor;
            tiles.Add(index, false);
            while (tiles.ContainsValue(false)) {
                Dictionary<int, bool> toAdd = new Dictionary<int, bool>();
                foreach (var tile in tiles) {
                    if (!tile.Value) {
                        // add neighbors to the toAdd dict
                        GetNeighbors(tile.Key, ref toAdd);
                    }
                }
                toAdd.ToList().ForEach(x => tiles.Add(x.Key, x.Value));
            }
            foreach (var tile in tiles) {
                board[tile.Key] = newColor;
            }
            return 0;
        }

        void GetNeighbors(int index, ref Dictionary<int, bool> toAdd) {
            /*
             * def get_neighbors(self, x, y, tiles, to_add, player):
        neighbors = [
            self.get_adj(x, y, 0, 1),
            self.get_adj(x, y, 0, -1),
            self.get_adj(x, y, 1, 0),
            self.get_adj(x, y, -1, 0)
        ]

        for n in neighbors:
            if n:
                if self.board[n[1]][n[0]].team == player.team:
                    self.board[n[1]][n[0]].color = player.color
                if self.board[n[1]][n[0]].color != player.color:
                    continue
                if n not in tiles:
                    to_add[n] = False
        tiles[(x, y)] = True
            */
            List<int> neighbors = new List<int>() {
                GetIndex(index, 1, 0),
                GetIndex(index, -1, 0),
                GetIndex(index, 0, 1),
                GetIndex(index, 0, -1)
            };
            int color = GetColor(index);
            foreach (var neighbor in neighbors) {
                if (neighbor < 0 || neighbor >= size * size) continue;

                if (GetColor(neighbor) == color) {
                    if (!toAdd.ContainsKey(neighbor)) {
                        toAdd.Add(neighbor, false);
                    }
                }
            }
            toAdd[index] = true;
        }
    }
}
