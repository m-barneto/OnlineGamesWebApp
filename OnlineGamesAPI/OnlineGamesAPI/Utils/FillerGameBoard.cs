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

        public FillerGameBoard(int size) {
            /*
             * # Make sure the player starting pieces arent the same color
        while self.board[0][0].color == self.board[self.size - 1][self.size - 1].color:
            self.board[0][0].color = self.rand_color()

        # Make sure surrounding tiles aren't the same color as player 1's starting piece
        while self.board[0][0].color == self.board[0][1].color:
            self.board[0][1].color = self.rand_color()
        while self.board[0][0].color == self.board[1][0].color:
            self.board[1][0].color = self.rand_color()
        # Make sure the surrounding tiles around player 1 aren't the same color as each other
        while self.board[0][1].color == self.board[1][0].color:
            self.board[0][1].color = self.rand_color()

        # Make sure surrounding tiles aren't the same color as player 2's starting piece
        while self.board[self.size - 1][self.size - 1].color == self.board[self.size - 1][self.size - 2].color:
            self.board[self.size - 1][self.size - 2].color = self.rand_color()
        while self.board[self.size - 1][self.size - 1].color == self.board[self.size - 2][self.size - 1].color:
            self.board[self.size - 2][self.size - 1].color = self.rand_color()
        # Make sure the surrounding tiles around player 2 aren't the same color as each other
        while self.board[self.size - 1][self.size - 2].color == self.board[self.size - 2][self.size - 1].color:
            self.board[self.size - 1][self.size - 2].color = self.rand_color()

        for y in range(self.size):
            for x in range(self.size):
                # region poop
                if y == 0 and x == 0:
                    continue
                if y == 1 and x == 0:
                    continue
                if y == 0 and x == 1:
                    continue
                if y == self.size - 1 and x == self.size - 1:
                    continue
                if y == self.size - 2 and x == self.size - 1:
                    continue
                if y == self.size - 1 and x == self.size - 2:
                    continue
                # endregion
                neighbors = [
                    self.get_adj(x, y, 0, 1),
                    self.get_adj(x, y, 0, -1),
                    self.get_adj(x, y, 1, 0),
                    self.get_adj(x, y, -1, 0)
                ]
                ex = []
                for n in neighbors:
                    if n:
                        ex.append(self.data.filler_emotes[self.board[n[1]][n[0]].color])
                self.board[y][x].color = self.get_rand_except(ex)

             */
            this.size = size;
            board = new List<Tile>();
            for (int i = 0; i < size * size; i++) {
                board.Add(new Tile(0, Random.Shared.Next(0, 5)));
            }
            
            board[0].team = 1;
            board[board.Count - 1].team = 2;
            

            // 
            while (GetColor(0, 0) == GetColor(size - 1, size - 1)) {
                board[0].color = Random.Shared.Next(0, 5);
            }
            /**
            # Make sure surrounding tiles aren't the same color as player 1's starting piece
            while self.board[0][0].color == self.board[0][1].color:
                self.board[0][1].color = self.rand_color()
            while self.board[0][0].color == self.board[1][0].color:
                self.board[1][0].color = self.rand_color()
            # Make sure the surrounding tiles around player 1 aren't the same color as each other
            while self.board[0][1].color == self.board[1][0].color:
                self.board[0][1].color = self.rand_color()
            **/
            // Make sure surrounding tiles aren't the same color as player 1's starting piece
            while (board[0].color == board[1].color) {
                board[1].color = Random.Shared.Next(0, 5);
            }
            while (board[0].color == board[size + 1].color) {
                board[size + 1].color = Random.Shared.Next(0, 5);
            }
            // Make sure the surrounding tiles around player 1 aren't the same color as each other
            while (board[1].color == board[size + 1].color && board[1].color == board[0].color) {
                board[1].color = Random.Shared.Next(0, 5);
            }

            // Do the same for player 2
            /**
            Make sure surrounding tiles aren't the same color as player 2's starting piece
            while self.board[self.size - 1][self.size - 1].color == self.board[self.size - 1][self.size - 2].color:
                self.board[self.size - 1][self.size - 2].color = self.rand_color()
            while self.board[self.size - 1][self.size - 1].color == self.board[self.size - 2][self.size - 1].color:
                self.board[self.size - 2][self.size - 1].color = self.rand_color()
            # Make sure the surrounding tiles around player 2 aren't the same color as each other
            while self.board[self.size - 1][self.size - 2].color == self.board[self.size - 2][self.size - 1].color:
                self.board[self.size - 1][self.size - 2].color = self.rand_color()
            **/
            // Double check the indices here
            while (board[board.Count - 1].color == board[board.Count - 2].color) {
                board[board.Count - 2].color = Random.Shared.Next();
            }
            while (board[board.Count - 1].color == board[board.Count - size - 1].color) {
                board[board.Count - size - 1].color = Random.Shared.Next();
            }
            // Shouldnt this be made sure with a check for players key index? since it could be randomly set to the same color on accident
            while (board[board.Count - 2].color == board[board.Count - size - 1].color) {
                board[board.Count - 2].color = Random.Shared.Next();
            }

            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    if (x == 0 && y == 0 || x == 0 && y == 1 || x == 1 && y == 0) {
                        Console.WriteLine($"Skipped ({x},{y})");
                        continue;
                    }
                    if (x == size - 1 && y == size - 1 || x == size - 1 && y == size - 2 || x == size - 2 && y == size - 1) {
                        Console.WriteLine($"Skipped ({x},{y})");
                        continue;
                    }
                    List<int> neighborColors = new() {
                        GetColor(x, y, 0, 1),
                        GetColor(x, y, 0,-1),
                        GetColor(x, y, 1, 0),
                        GetColor(x, y,-1, 0)
                    };
                    board[GetIndex(x, y)].color = GetRandExcept(neighborColors);
                }
            }
        }

        int GetRandExcept(int e) {
            int val = Random.Shared.Next(0, 5);
            while (e == val) {
                val = Random.Shared.Next(0, 5);
            }
            return val;
        }
        int GetRandExcept(List<int> list) {
            int val = Random.Shared.Next(0, 5);
            while (list.Contains(val)) {
                val = Random.Shared.Next(0, 5);
            }
            return val;
        }

        public int GetColorByIndex(int index, int xOff = 0, int yOff = 0) {
            return GetColor(index % size, index / size, xOff, yOff);
        }
        public int GetColor(int x, int y, int xOff = 0, int yOff = 0) {
            if (x + xOff < 0 || y + yOff < 0 || x + xOff >= size || y + yOff >= size) {
                return -1;
            }
            return board[((y + yOff) * size) + (x + xOff)].color;
        }


        int GetIndex(int index, int xOff, int yOff) {
            return GetIndex(index % size, index / size, xOff, yOff);
        }
        int GetIndex(int x, int y, int xOff = 0, int yOff = 0) {
            if (x + xOff < 0 || y + yOff < 0 || x + xOff >= size || y + yOff >= size) {
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
            int color = GetColorByIndex(index);

            for (int i = 0; i < neighborIndices.Count; i++) {
                if (neighborIndices[i] < 0) continue;

                if (color == GetColorByIndex(neighborIndices[i]) || GetTeam(index) == board[neighborIndices[i]].team) { // color == GetColor(neighborIndices[i])
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
