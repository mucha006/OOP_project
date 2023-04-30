using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Reflection.Emit;

namespace game_2048
{
    public class game
    {
        int[,] array;
        int size = 4;
        Random rnd;
        public int score = 0;
        private int bestScore = 0;
        private string bestPlayer = "";
        public string nickname;

        public game()
        {
            array = new int[size, size];
            rnd = new Random();
        }

        // Set color by field value
        public Color GetColor(int x, int y)
        {
            switch (array[x, y])
            {
                case 0:
                    return Colors.White;
                case 2:
                    return Colors.Aqua;
                case 4:
                    return Colors.SkyBlue;
                case 8:
                    return Colors.CornflowerBlue;
                case 16:
                    return Colors.DeepSkyBlue;
                case 32:
                    return Colors.SteelBlue;
                case 64:
                    return Colors.Blue;
                case 128:
                    return Colors.MediumBlue;
                case 256:
                    return Colors.DarkBlue;
                case 512:
                    return Colors.LightGreen;
                case 1024:
                    return Colors.SeaGreen;
                case 2048:
                    return Colors.Green;
                default:
                    return Colors.DarkGreen;
            }
        }

        // Returns the box value and set it to the label
        public string getContent(int x, int y)
        {
            if (array[x, y] == 0)
                return " ";
            return array[x, y].ToString();
        }

        public int getScore()
        {
            return score;
        }

        public void setScoreToZero()
        {
            score = 0;
        }

        // Writes the best score to a file and then returns
        public int getBestScore()
        {
            bestPlayer = nickname;
            if (score > bestScore)
            {
                bestScore = score;
                bestPlayer = nickname;
                using (StreamWriter sw = new StreamWriter("bestscore.txt"))
                {
                    sw.WriteLine(bestPlayer);
                    sw.WriteLine(bestScore);
                }
            }
            if (File.Exists("bestscore.txt"))
            {
                using (StreamReader sr = new StreamReader("bestscore.txt"))
                {
                    bestPlayer = sr.ReadLine();
                    bestScore = int.Parse(sr.ReadLine());
                }
            }
            return bestScore;
        }

        // Best player is overwritten in the function getBestScore
        public string getBestPlayer()
        {
            return bestPlayer;
        }

        public string getNickname()
        {
            return nickname;
        }

        // It is called when the new game button is clicked and resets the entire field and generates 2 new random boxes
        public void newGame()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    array[x, y] = 0;
            RandomAdd();
            RandomAdd();
        }

        // It reacts to a key press and compares whether the field changed after the key press, if so, it generates a new random box
        public void Shift(Key k)
        {
            int[,] arr = new int[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    arr[x, y] = array[x, y];
            switch (k)
            {
                case Key.Left:
                    ShiftLeft();
                    break;
                case Key.Right:
                    ShiftRight();
                    break;
                case Key.Up:
                    ShiftUp();
                    break;
                case Key.Down:
                    ShiftDown();
                    break;
            }
            if (!compare(array, arr))
                RandomAdd();
        }

        // A function that compares whether a field has changed after a key has been pressed
        private bool compare(int[,] A, int[,] B)
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (A[x, y] != B[x, y])
                        return false;
            return true;
        }

        private void ShiftRight()
        {
            for (int x = size - 1; x >= 0; x--)
                for (int y = size - 1; y > 0; y--)
                {
                    if (array[x, y] == 0)
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[x, i] != 0)
                            {
                                array[x, y] = array[x, i];
                                array[x, i] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[x, i] == array[x, y])
                            {
                                array[x, y] += array[x, i];
                                score += array[x, i];
                                array[x, i] = 0;
                                break;
                            }
                    }
                }
        }

        private void ShiftDown()
        {
            for (int x = size - 1; x >= 0; x--)
                for (int y = size - 1; y > 0; y--) 
                {
                    if (array[y, x] == 0)
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[i, x] != 0)
                            {
                                array[y, x] = array[i, x];
                                array[i, x] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y - 1; i >= 0; i--)
                            if (array[i, x] == array[y, x])
                            {
                                array[y, x] += array[i, x];
                                score += array[i, x];
                                array[i, x] = 0;
                                break;
                            }
                    }
                }
        }

        private void ShiftUp()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size - 1; y++)
                {
                    if (array[y, x] == 0)
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[i, x] != 0)
                            {
                                array[y, x] = array[i, x];
                                array[i, x] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[i, x] == array[y, x])
                            {
                                array[y, x] += array[i, x];
                                score += array[i, x];
                                array[i, x] = 0;
                                break;
                            }
                    }
                }
        }

        private void ShiftLeft()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size - 1; y++)
                {
                    if (array[x, y] == 0)
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[x, i] != 0)
                            {
                                array[x, y] = array[x, i];
                                array[x, i] = 0;
                                break;
                            }
                    }
                    else
                    {
                        for (int i = y + 1; i < size; i++)
                            if (array[x, i] == array[x, y])
                            {
                                array[x, y] += array[x, i];
                                score += array[x, i];
                                array[x, i] = 0;
                                break;
                            }
                    }
                }
        }

        private bool Finish()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (array[x, y] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Add a new box to the free space
        public void RandomAdd()
        {
            int x, y;
            if (Finish())
            {
                MessageBox.Show("GAME OVER!");
            }
            do
            {
                x = rnd.Next(size);
                y = rnd.Next(size);
            }
            while (array[x, y] != 0);
            {
                array[x, y] = 2;
            }
        }

        // Saves the current player's progress to a file when the application is closed
        public void saveActualScore()
        {
            // Loading the current player's achievement into a list, there are 2 lists, one for names and the other for whole rows
            List<string> players = new List<string>();
            List<string> scores = new List<string>();
            if (File.Exists("actualScore.txt"))
            {
                using (StreamReader sr = new StreamReader("actualScore.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length > 2) 
                        {
                            players.Add(parts[0]);
                            scores.Add(line);
                        }
                    }
                }
            }

            // Loading a new player achievement
            string scoreLine = nickname;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    scoreLine += "," + array[x, y];
                }
            }

            // Writing a new player achievement to the file, if there was a player with the same name before, it will overwrite it
            bool scoreWritten = false;
            using (StreamWriter sw = new StreamWriter("actualScore.txt"))
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] == nickname)
                    {
                        scores[i] = scoreLine;
                        scoreWritten = true;
                        break;
                    }
                }

                if (!scoreWritten)
                {
                    players.Add(nickname);
                    scores.Add(scoreLine);
                }

                for (int i = 0; i < players.Count; i++)
                {
                    sw.WriteLine(scores[i]);
                }
            }
        }

        // A function for loading the player's achievement from the last game, called by pressing the continue in game button
        public void loadActualScore()
        {
            if (File.Exists("actualScore.txt"))
            {
                List<string> players = new List<string>();
                using (StreamReader sr = new StreamReader("actualScore.txt"))
                {
                    string line;
                    // Load all the games stored in the file into the list
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        players.Add(parts[0]);
                    }
                    // If he finds a match with a player on the list, read his player's achievment as he finished
                    foreach (string s in players)
                    {
                        if (s == nickname)
                        {
                            sr.BaseStream.Position = 0;
                            int i = 1;
                            int index = players.IndexOf(nickname);
                            for (int z = 0; z <= index; z++)
                            {
                                line = sr.ReadLine();
                            }
                            string[] parts = line.Split(',');
                            for (int x = 0; x < size; x++)
                            {
                                for (int y = 0; y < size; y++)
                                {
                                    int j = int.Parse(parts[i]);
                                    array[x, y] = j;
                                    i++;
                                }
                            }

                        }
                    }
                }
            }
        }

        // A function that checks if the given player is already in the actualScore. Used to disable or enable the continue in game button
        public bool isPlayerInList()
        {
            if (File.Exists("actualScore.txt"))
            {
                List<string> players = new List<string>();
                using (StreamReader sr = new StreamReader("actualScore.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        players.Add(parts[0]);
                    }
                    foreach (string s in players)
                    {
                        if (s == nickname)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        // Load leaderboard from file
        public List<playerScore> loadLeaderboard()
        {
            List<playerScore> leaderboard = new List<playerScore>();
            if (File.Exists("leaderboard.txt"))
            {
                using (StreamReader sr = new StreamReader("leaderboard.txt"))
                {
                    int rank = 1;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('.', ':', ';', ' ');
                        leaderboard.Add(new playerScore { Rank = rank, Name = parts[4], Score = int.Parse(parts[8]) });
                        rank++;
                    }
                }
            }
            return leaderboard;
        }

        // Saves the top 10 players by score to a file
        public void saveScore(playerScore score)
        {
            List<playerScore> leaderboard = loadLeaderboard();
            bool playerExists = false;
            // It checks if there is a player with the same name in the leaderboard, if so and has a lower score, it overwrites it
            foreach (playerScore existingScore in leaderboard)
            {
                if (existingScore.Name == score.Name)
                {
                    playerExists = true;
                    if (score.Score > existingScore.Score)
                    {
                        existingScore.Score = score.Score;
                    }
                    break;
                }
            }          
            if (!playerExists)
            {
                leaderboard.Add(score);
            }
            leaderboard = leaderboard.OrderByDescending(s => s.Score).ToList();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }
            leaderboard = leaderboard.Take(10).ToList();
            // Writing to a file
            using (StreamWriter sw = new StreamWriter("leaderboard.txt"))
            {
                foreach (playerScore s in leaderboard)
                {
                    sw.WriteLine($"{s.Rank}. PLAYER: {s.Name}; SCORE: {s.Score}");
                }
            }
        }

        // Writes to the leaderboard label from the file
        public void writeLeadeboardToLabel(System.Windows.Controls.Label labLeaderboard)
        {
            playerScore leaderboard;
            leaderboard = new playerScore { Name = nickname, Score = score };
            saveScore(leaderboard);
            if (File.Exists("leaderboard.txt"))
            {
                using (StreamReader sr = new StreamReader("leaderboard.txt"))
                {
                    string line;
                    labLeaderboard.Content = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        labLeaderboard.Content += line + Environment.NewLine;
                    }
                }
            }
        }     
    }
}
