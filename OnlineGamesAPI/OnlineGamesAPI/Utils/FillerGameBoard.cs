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

        public int GetColor(int index) {
            return board[index].color;
        }

        public int GetColor(int x, int y) {
            return board[GetIndex(x, y)].color;
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
            int team = index == 0 ? 1 : 2;
            Dictionary<int, bool> tiles = new Dictionary<int, bool>();
            board[index].color = newColor;
            tiles.Add(index, false);
            int total = 0;
            while (tiles.ContainsValue(false)) {
                List<int> toAdd = new();
                foreach (var tile in tiles) {
                    if (!tile.Value) {
                        List<int> neighbors = new List<int>() {
                            GetIndex(index, 1, 0),
                            GetIndex(index, -1, 0),
                            GetIndex(index, 0, 1),
                            GetIndex(index, 0, -1)
                        };
                        foreach (var neighbor in neighbors) {
                            if (neighbor < 0 || neighbor >= size * size) continue;

                            /*
                            if self.board[n[1]][n[0]].team == player.team:
                                self.board[n[1]][n[0]].color = player.color
                            if self.board[n[1]][n[0]].color != player.color:
                                continue
                            if n not in tiles:
                                to_add[n] = False
                             */
                            if (board[neighbor].team == team) {
                                board[neighbor].color = newColor;
                            }
                            if (board[neighbor].color != newColor) {
                                continue;
                            }
                            if (!tiles.ContainsKey(neighbor)) {
                                toAdd.Add(neighbor);
                                total++;
                            }
                        }
                        tiles[tile.Key] = true;
                    }
                }
                foreach (var tile in toAdd) {
                    Console.WriteLine("REEEEE");
                    tiles.Add(tile, false);
                }
                tiles.Add(14, false);
            }
            foreach (var tile in tiles) {
                board[tile.Key].color = newColor;
                board[tile.Key].team = team;
            }
            Console.WriteLine(total);
            Console.WriteLine(total);
            Console.WriteLine(total);
            Console.WriteLine(total);
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
