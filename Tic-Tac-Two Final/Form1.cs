using System.Data.SqlTypes;
using System.Numerics;
using System.Reflection;

namespace Tic_Tac_Two_Final
{
    public partial class TicTacTwoForm : System.Windows.Forms.Form
    {
        string player1 = ""; //Human or bot
        string player2 = ""; //Human or bot
        string gameType = ""; // Determines the type of game the round is goin to be
        int turn = 0; //Determines the turns
        int[,] board = new int[5, 5]; //Used to determine if there is a win
        string area = ""; //Determines the area (a) of the board
        int oCount = 0; //# of Os left 
        int xCount = 0; //# of Xs left
        bool piecePlaceOn = false; //Determines if a piece should even be placed on because it would place if this wasnt in place.
        bool turnLock = false; //Stops the code from playing another turn because it was doing that before for some reaosn.
        bool moveBoardOn = false; //Determines if the boxes should move the board or not
        bool winCheckEnable = false; //Stops the player from double clicking an occupied place to waste a turn.
        bool movePieceOn = false; //Determines if the pieces should move or not.
        bool placeMovedPieceOn = false; //Determines if the moved piece should be placed.
        int box = 0; //Determines which box shouldn't be highlighted when it comes to selecting where to move piece.
        bool placeblock = false;
        int[,] yBoard = new int[5, 5]; //yellow board
        bool botTurn = true;
        public TicTacTwoForm()
        {
            InitializeComponent();
            MessageBox.Show("At the start of the game, each player takes turns placing one of their pieces on any empty cell contained within the tic-tac-toe grid.\n" +
            "Once each player has placed at least two of their pieces, they may do one of three things on their turn:\n" +
            "(1) place one of their remaining pieces on an empty cell within the tic - tac - toe grid,\n" +
            "(2) move the tic - tac - toe grid such that it is centered at a cell one space horizontally, vertically, or diagonally away from the cell it was originally centered at, or\n" +
            "(3) move one of their pieces that is already on the board(regardless of whether it is within the tic - tac - toe grid) to any empty cell within the grid.\n" +
            "\n" +
            "The first player to create a horizontal, vertical, or diagonal line of their own pieces contained within the tic - tac - toe grid wins. If in a single move the grid has been moved such that it contains both a three -in-a - row of X pieces and a three-in-a - row of O pieces, then the game is a tie.", "TicTacTwo Rules");
        }

        //Code related to the bot:

        //The bot determines what to do for which game type
        //The bot then looks for a potential win for its piece
        //The bot then looks for a potential block for its piece
        //The bot then uses a random # from the first step to determine what random action to take.
        
        public void bot() //CODE FOR THE BOT CODE FOR THE BOT
        {
            Random rng = new Random();
            int output = rng.Next(1, 26);
            int move = 0;

            //1 = place
            //2 = move board
            //3 = move piece

            if (gameType == "humanbot")
            {
                if (turn >= 5)
                {
                    if (oCount != 0)
                    {
                        move = rng.Next(1, 4);
                    }
                    else
                    {
                        move = rng.Next(2, 4);
                    }
                }
                else
                {
                    move = 1;
                }
            } //humanbot
            else if (gameType == "bothuman")
            {
                if (turn >= 4)
                {
                    if (xCount != 0)
                    {
                        move = rng.Next(1, 4);
                    }
                    else
                    {
                        move = rng.Next(2, 4);
                    }
                }
                else
                {
                    move = 1;
                }
            } //bothuman
            else
            {
                if (turn >= 4)
                {
                    if (turn % 2 == 0)
                    {
                        if (xCount != 0)
                        {
                            move = rng.Next(1, 4);
                        }
                        else
                        {
                            move = rng.Next(2, 4);
                        }
                    }
                    else
                    {
                        if (oCount != 0)
                        {
                            move = rng.Next(1, 4);
                        }
                        else
                        {
                            move = rng.Next(2, 4);
                        }
                    }
                }
                else
                {
                    move = 1;
                }
            } //botbot


            botTurn = true;
            blockarea("win"); //Checks for a win
            if (botTurn == true)
            {
                blockarea("block");
            } //This sees if there is a block
            if (botTurn == true)
            {
                if (move == 1) //places //place
                {
                    placeableArea();
                    area = boardarea();
                    placeblock = false;
                    while (placeblock == false)
                    {
                        output = rng.Next(1, 26);
                        for (int i = 1; i < 26; i++)
                        {
                            if (output == i && board[(i - 1) / 5, (i - 1) % 5] == 0 && isBoxEnabled(i) == true)
                            {
                                placeblock = true;
                                boardXandY(output);
                                deleteXandO();
                                addnum(output);
                            }
                        }
                    }
                }
                else if (move == 2) //moves board
                {
                    if (area == "tl")
                    {
                        int[] boardselect = { 2, 4, 5 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 3)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "tm")
                    {
                        int[] boardselect = { 1, 3, 4, 5, 6 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 5)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "tr")
                    {
                        int[] boardselect = { 2, 5, 6 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 3)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "ml")
                    {
                        int[] boardselect = { 1, 2, 5, 7, 8 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 5)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "m")
                    {
                        int[] boardselect = { 1, 2, 3, 4, 6, 7, 8, 9 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 8)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "mr")
                    {
                        int[] boardselect = { 2, 3, 5, 8, 9 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 5)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "bl")
                    {
                        int[] boardselect = { 4, 5, 8 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 3)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "bm")
                    {
                        int[] boardselect = { 4, 5, 6, 7, 9 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 5)]);
                        area = boardarea();
                        winCheck();
                    }
                    else if (area == "br")
                    {
                        int[] boardselect = { 5, 6, 8 };
                        boardInvisible();
                        botMoveBoard(boardselect[rng.Next(0, 3)]);
                        area = boardarea();
                        winCheck();
                    }
                } //moves board
                else
                {
                    while (findXandO(rng.Next(1, 26)) == false)
                    {
                    }
                    clearBox();
                    area = boardarea();
                    placeableArea();
                    yellowBoard();
                    bool movepieceblock = false;
                    while (movepieceblock == false)
                    {
                        output = rng.Next(1, 26);
                        for (int i = 1; i < 26; i++)
                        {
                            if (i == output && i != box && yBoard[(output - 1) / 5, (output - 1) % 5] == 2 && movepieceblock == false)
                            {
                                movepieceblock = true;
                                boardXandY(output);
                                addnum(output);
                            }
                        }
                    }
                } //place moved piece
            } //This plays a random move
        }

        //Bot block
        //Goes through a check through winning and losing
        //If it detects a win, it will find a piece that is not in the winning straight, column, row, and place it in the correct location
        public bool placeBlock(string code) //determines if placing is viable
        {
            if (turn % 2 == 0)
            {
                if (xCount != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (oCount != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void winCon(int n, string code) //determines win condition
        {
            botTurn = false;
            if (placeBlock(code) == true)
            {
                boardXandY(n);
                deleteXandO();
                addnum(n);
            }
            else if (code == "win")
            {
                Random rng = new Random();
                while (findXandO(rng.Next(1, 26)) == false)
                {
                }
                clearBox();
                boardXandY(n);
                addnum(n);
            }
            else
            {
                Random rng = new Random();
                while (findXandO(rng.Next(1, 26)) == false)
                {
                }
                clearBox();
                boardXandY(n);
                addnum(n);
            }
        }

        public void blockarea(string code)
        {
            area = boardarea();
            if (area == "tl")
            {
                block(0, 0, code);
            }
            else if (area == "tm")
            {
                block(0, 1, code);
            }
            else if (area == "tr")
            {
                block(0, 2, code);
            }
            else if (area == "ml")
            {
                block(1, 0, code);
            }
            else if (area == "m")
            {
                block(1, 1, code);
            }
            else if (area == "mr")
            {
                block(1, 2, code);
            }
            else if (area == "bl")
            {
                block(2, 0, code);
            }
            else if (area == "bm")
            {
                block(2, 1, code);
            }
            else if (area == "br")
            {
                block(2, 2, code);
            }
        }

        public void block(int i, int j, string code) //Code to block
        {
            int p = 0;
            botTurn = true;
            if (code == "block")
            {
                if (turn % 2 == 0) //blocks
                {
                    p = -1;
                }
                else
                {
                    p = 1;
                }
            }
            else
            {
                if (turn % 2 == 0) //wins
                {
                    p = 1;
                }
                else
                {
                    p = -1;
                }
            }

            string total = "";
            for (int r = i; r < 3 + i; r++) //Checks rows
            {
                total = "";
                for (int c = j; c < 3 + j; c++)
                {
                    total += board[r, c].ToString();
                }
                if (total == "0" + p + "" + p)
                {
                    winCon((r * 5) + j + 1, code);
                }
                else if (total == "" + p + "0" + p)
                {
                    winCon((r * 5) + j + 2, code);
                }
                else if (total == "" + p + "" + p + "0")
                {
                    winCon((r * 5) + j + 3, code);
                }
            }
            if (botTurn != false)
            {
                for (int c = j; c < 3 + j; c++) //Checks columns
                {
                    total = "";
                    for (int r = i; r < 3 + i; r++)
                    {
                        total += board[r, c].ToString();
                    }
                    if (total == "0" + p.ToString() + p.ToString()) //1,1
                    {
                        winCon((i * 5) + c + 1, code);
                    }
                    else if (total == p.ToString() + "0" + p.ToString())
                    {
                        winCon((i * 5) + c + 6, code);
                    }
                    else if (total == p.ToString() + p.ToString() + "0")
                    {
                        winCon((i * 5) + c + 11, code);
                    }
                }
            }
            if (botTurn != false)
            {
                if (board[i, j].ToString() + board[i + 1, j + 1].ToString() + board[i + 2, j + 2] == "0" + p.ToString() + p.ToString())
                {
                    winCon((i * 5) + j + 1, code);
                }
                else if (board[i, j].ToString() + board[i + 1, j + 1].ToString() + board[i + 2, j + 2] == p.ToString() + "0" + p.ToString())
                {
                    winCon((i * 5) + j + 7, code);
                }
                else if (board[i, j].ToString() + board[i + 1, j + 1].ToString() + board[i + 2, j + 2] == p.ToString() + p.ToString() + "0")
                {
                    winCon((i * 5) + j + 13, code);
                }
                else if (board[i + 2, j].ToString() + board[i + 1, j + 1].ToString() + board[i, j + 2] == "0" + p.ToString() + p.ToString())
                {
                    winCon((i * 5) + j + 11, code);
                }
                else if (board[i + 2, j].ToString() + board[i + 1, j + 1].ToString() + board[i, j + 2] == p.ToString() + "0" + p.ToString())
                {
                    winCon((i * 5) + j + 7, code);
                }
                else if (board[i + 2, j].ToString() + board[i + 1, j + 1].ToString() + board[i, j + 2] == p.ToString() + p.ToString() + "0")
                {
                    winCon((i * 5) + j + 3, code);
                }
            }
        }

        public void yellowBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    yBoard[i, j] = 0;
                }
            }
            for (int i = 1; i < 26; i++)
            {
                if (Box1.BackColor == Color.Yellow && i == 1)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box2.BackColor == Color.Yellow && i == 2)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box3.BackColor == Color.Yellow && i == 3)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box4.BackColor == Color.Yellow && i == 4)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box5.BackColor == Color.Yellow && i == 5)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box6.BackColor == Color.Yellow && i == 6)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box7.BackColor == Color.Yellow && i == 7)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box8.BackColor == Color.Yellow && i == 8)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box9.BackColor == Color.Yellow && i == 9)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box10.BackColor == Color.Yellow && i == 10)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box11.BackColor == Color.Yellow && i == 11)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box12.BackColor == Color.Yellow && i == 12)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box13.BackColor == Color.Yellow && i == 13)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box14.BackColor == Color.Yellow && i == 14)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box15.BackColor == Color.Yellow && i == 15)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box16.BackColor == Color.Yellow && i == 16)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box17.BackColor == Color.Yellow && i == 17)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box18.BackColor == Color.Yellow && i == 18)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box19.BackColor == Color.Yellow && i == 19)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box20.BackColor == Color.Yellow && i == 20)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box21.BackColor == Color.Yellow && i == 21)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box22.BackColor == Color.Yellow && i == 22)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box23.BackColor == Color.Yellow && i == 23)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box24.BackColor == Color.Yellow && i == 24)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
                else if (Box25.BackColor == Color.Yellow && i == 25)
                {
                    yBoard[(i - 1) / 5, (i - 1) % 5] = 2;
                }
            }
        }

        public bool isBoxEnabled(int n) // Determines if a box is on or not based on #
        {
            if (n == 1)
            {
                if (Box1.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 2)
            {
                if (Box2.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 3)
            {
                if (Box3.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 4)
            {
                if (Box4.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 5)
            {
                if (Box5.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 6)
            {
                if (Box6.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 7)
            {
                if (Box7.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 8)
            {
                if (Box8.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 9)
            {
                if (Box9.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 10)
            {
                if (Box10.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 11)
            {
                if (Box11.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 12)
            {
                if (Box12.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 13)
            {
                if (Box13.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 14)
            {
                if (Box14.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 15)
            {
                if (Box15.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 16)
            {
                if (Box16.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 17)
            {
                if (Box17.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 18)
            {
                if (Box18.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 19)
            {
                if (Box19.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 20)
            {
                if (Box20.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 21)
            {
                if (Box21.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 22)
            {
                if (Box22.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 23)
            {
                if (Box23.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 24)
            {
                if (Box24.Enabled == true)
                {
                    return true;
                }
            }
            else if (n == 25)
            {

            }
            if (Box25.Enabled == true)
            {
                return true;
            }
            return false;
        }


        //Code related to the board:



        //Code related to the game moves:



        //Code related to the game functionality:

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void botMoveBoard(int n) //Makes at least one board visible
        {
            //Boards: From left to right from top to bottom
            if (n == 1)
            {
                tlBoard.Visible = true;
            }
            else if (n == 2)
            {
                tmBoard.Visible = true;
            }
            else if (n == 3)
            {
                trBoard.Visible = true;
            }
            else if (n == 4)
            {
                mlBoard.Visible = true;
            }
            else if (n == 5)
            {
                mBoard.Visible = true;
            }
            else if (n == 6)
            {
                mrBoard.Visible = true;
            }
            else if (n == 7)
            {
                blBoard.Visible = true;
            }
            else if (n == 8)
            {
                bmBoard.Visible = true;
            }
            else if (n == 9)
            {
                brBoard.Visible = true;
            }
        }

        public void boardInvisible() //Makes all board invisible
        {
            tlBoard.Visible = false;
            tmBoard.Visible = false;
            trBoard.Visible = false;
            mlBoard.Visible = false;
            mBoard.Visible = false;
            mrBoard.Visible = false;
            blBoard.Visible = false;
            bmBoard.Visible = false;
            brBoard.Visible = false;
        }

        public bool findXandO(int n) //For bot. If the box has an X or O, it removes the num on board, remove the image, and stop the loop
        {
            int p = 0;
            if (turn % 2 == 0)
            {
                p = 1;
            }
            else
            {
                p = -1;
            }
            if (board[(n - 1) / 5, (n - 1) % 5] == p)
            {
                board[(n - 1) / 5, (n - 1) % 5] = 0;
                box = n;
                clearBox();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void boardXandY(int n) // For bot. Adds an X or O based on the # and turn
        {
            if (n == 1)
            {
                if (turn % 2 == 0)
                {
                    Box1.Image = Properties.Resources.X;
                }
                else
                {
                    Box1.Image = Properties.Resources.O;
                }
            }
            else if (n == 2)
            {
                if (turn % 2 == 0)
                {
                    Box2.Image = Properties.Resources.X;
                }
                else
                {
                    Box2.Image = Properties.Resources.O;
                }
            }
            else if (n == 3)
            {
                if (turn % 2 == 0)
                {
                    Box3.Image = Properties.Resources.X;
                }
                else
                {
                    Box3.Image = Properties.Resources.O;
                }
            }
            else if (n == 4)
            {
                if (turn % 2 == 0)
                {
                    Box4.Image = Properties.Resources.X;
                }
                else
                {
                    Box4.Image = Properties.Resources.O;
                }
            }
            else if (n == 5)
            {
                if (turn % 2 == 0)
                {
                    Box5.Image = Properties.Resources.X;
                }
                else
                {
                    Box5.Image = Properties.Resources.O;
                }
            }
            else if (n == 6)
            {
                if (turn % 2 == 0)
                {
                    Box6.Image = Properties.Resources.X;
                }
                else
                {
                    Box6.Image = Properties.Resources.O;
                }
            }
            else if (n == 7)
            {
                if (turn % 2 == 0)
                {
                    Box7.Image = Properties.Resources.X;
                }
                else
                {
                    Box7.Image = Properties.Resources.O;
                }
            }
            else if (n == 8)
            {
                if (turn % 2 == 0)
                {
                    Box8.Image = Properties.Resources.X;
                }
                else
                {
                    Box8.Image = Properties.Resources.O;
                }
            }
            else if (n == 9)
            {
                if (turn % 2 == 0)
                {
                    Box9.Image = Properties.Resources.X;
                }
                else
                {
                    Box9.Image = Properties.Resources.O;
                }
            }
            else if (n == 10)
            {
                if (turn % 2 == 0)
                {
                    Box10.Image = Properties.Resources.X;
                }
                else
                {
                    Box10.Image = Properties.Resources.O;
                }
            }
            else if (n == 11)
            {
                if (turn % 2 == 0)
                {
                    Box11.Image = Properties.Resources.X;
                }
                else
                {
                    Box11.Image = Properties.Resources.O;
                }
            }
            else if (n == 12)
            {
                if (turn % 2 == 0)
                {
                    Box12.Image = Properties.Resources.X;
                }
                else
                {
                    Box12.Image = Properties.Resources.O;
                }
            }
            else if (n == 13)
            {
                if (turn % 2 == 0)
                {
                    Box13.Image = Properties.Resources.X;
                }
                else
                {
                    Box13.Image = Properties.Resources.O;
                }
            }
            else if (n == 14)
            {
                if (turn % 2 == 0)
                {
                    Box14.Image = Properties.Resources.X;
                }
                else
                {
                    Box14.Image = Properties.Resources.O;
                }
            }
            else if (n == 15)
            {
                if (turn % 2 == 0)
                {
                    Box15.Image = Properties.Resources.X;
                }
                else
                {
                    Box15.Image = Properties.Resources.O;
                }
            }
            else if (n == 16)
            {
                if (turn % 2 == 0)
                {
                    Box16.Image = Properties.Resources.X;
                }
                else
                {
                    Box16.Image = Properties.Resources.O;
                }
            }
            else if (n == 17)
            {
                if (turn % 2 == 0)
                {
                    Box17.Image = Properties.Resources.X;
                }
                else
                {
                    Box17.Image = Properties.Resources.O;
                }
            }
            else if (n == 18)
            {
                if (turn % 2 == 0)
                {
                    Box18.Image = Properties.Resources.X;
                }
                else
                {
                    Box18.Image = Properties.Resources.O;
                }
            }
            else if (n == 19)
            {
                if (turn % 2 == 0)
                {
                    Box19.Image = Properties.Resources.X;
                }
                else
                {
                    Box19.Image = Properties.Resources.O;
                }
            }
            else if (n == 20)
            {
                if (turn % 2 == 0)
                {
                    Box20.Image = Properties.Resources.X;
                }
                else
                {
                    Box20.Image = Properties.Resources.O;
                }
            }
            else if (n == 21)
            {
                if (turn % 2 == 0)
                {
                    Box21.Image = Properties.Resources.X;
                }
                else
                {
                    Box21.Image = Properties.Resources.O;
                }
            }
            else if (n == 22)
            {
                if (turn % 2 == 0)
                {
                    Box22.Image = Properties.Resources.X;
                }
                else
                {
                    Box22.Image = Properties.Resources.O;
                }
            }
            else if (n == 23)
            {
                if (turn % 2 == 0)
                {
                    Box23.Image = Properties.Resources.X;
                }
                else
                {
                    Box23.Image = Properties.Resources.O;
                }
            }
            else if (n == 24)
            {
                if (turn % 2 == 0)
                {
                    Box24.Image = Properties.Resources.X;
                }
                else
                {
                    Box24.Image = Properties.Resources.O;
                }
            }
            else if (n == 25)
            {
                if (turn % 2 == 0)
                {
                    Box25.Image = Properties.Resources.X;
                }
                else
                {
                    Box25.Image = Properties.Resources.O;
                }
            }
        }

        public void clearBox() // Clears a Box Image based on the int box num
        {
            if (box == 1)
            {
                Box1.Image = null;
            }
            else if (box == 2)
            {
                Box2.Image = null;
            }
            else if (box == 3)
            {
                Box3.Image = null;
            }
            else if (box == 4)
            {
                Box4.Image = null;
            }
            else if (box == 5)
            {
                Box5.Image = null;
            }
            else if (box == 6)
            {
                Box6.Image = null;
            }
            else if (box == 7)
            {
                Box7.Image = null;
            }
            else if (box == 8)
            {
                Box8.Image = null;
            }
            else if (box == 9)
            {
                Box9.Image = null;
            }
            else if (box == 10)
            {
                Box10.Image = null;
            }
            else if (box == 11)
            {
                Box11.Image = null;
            }
            else if (box == 12)
            {
                Box12.Image = null;
            }
            else if (box == 13)
            {
                Box13.Image = null;
            }
            else if (box == 14)
            {
                Box14.Image = null;
            }
            else if (box == 15)
            {
                Box15.Image = null;
            }
            else if (box == 16)
            {
                Box16.Image = null;
            }
            else if (box == 17)
            {
                Box17.Image = null;
            }
            else if (box == 18)
            {
                Box18.Image = null;
            }
            else if (box == 19)
            {
                Box19.Image = null;
            }
            else if (box == 20)
            {
                Box20.Image = null;
            }
            else if (box == 21)
            {
                Box21.Image = null;
            }
            else if (box == 22)
            {
                Box22.Image = null;
            }
            else if (box == 23)
            {
                Box23.Image = null;
            }
            else if (box == 24)
            {
                Box24.Image = null;
            }
            else if (box == 25)
            {
                Box25.Image = null;
            }
        }

        public void optionsDisable() //Disables and makes options invisible
        {
            optionsLabel.Visible = false;
            placeButton.Visible = false;
            moveBoardButton.Visible = false;
            movePieceButton.Visible = false;
            placeButton.Enabled = false;
            moveBoardButton.Enabled = false;
            movePieceButton.Enabled = false;
        }

        public void optionsEnable() //Enables and makes options visible
        {
            optionsLabel.Visible = true;
            placeButton.Visible = true;
            placeButton.Enabled = true;
            movePieceButton.Visible = true;
            movePieceButton.Enabled = true;
            moveBoardButton.Visible = true;
            moveBoardButton.Enabled = true;
        }

        public void addnum(int n)
        {
            botTurn = false;
            int row = n - 1;
            row /= 5;
            int col = n - 1;
            col %= 5;
            if (turn % 2 == 0)
            {
                board[row, col] = 1;
            }
            else
            {
                board[row, col] = -1;
            }
            winCheck();
        } //Adds 1 or -1 to the board in order to make it match the visual board

        public void deleteXandO() //Takes off a 1 on either X or O
        {
            if (turn % 2 == 0)
            {
                xCount--;
            }
            else
            {
                oCount--;
            }
        }

        public void resetboard() //resets board
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    board[i, j] = 0;
                    yBoard[i, j] = 0;
                }
            }
        }

        public void boxDisable() //Disable boxes
        {
            Box1.Enabled = false;
            Box2.Enabled = false;
            Box3.Enabled = false;
            Box4.Enabled = false;
            Box5.Enabled = false;
            Box6.Enabled = false;
            Box7.Enabled = false;
            Box8.Enabled = false;
            Box9.Enabled = false;
            Box10.Enabled = false;
            Box11.Enabled = false;
            Box12.Enabled = false;
            Box13.Enabled = false;
            Box14.Enabled = false;
            Box15.Enabled = false;
            Box16.Enabled = false;
            Box17.Enabled = false;
            Box18.Enabled = false;
            Box19.Enabled = false;
            Box20.Enabled = false;
            Box21.Enabled = false;
            Box22.Enabled = false;
            Box23.Enabled = false;
            Box24.Enabled = false;
            Box25.Enabled = false;
        }

        public void imageRemove() //Removes image from boxes
        {
            Box1.Image = null;
            Box2.Image = null;
            Box3.Image = null;
            Box4.Image = null;
            Box5.Image = null;
            Box6.Image = null;
            Box7.Image = null;
            Box8.Image = null;
            Box9.Image = null;
            Box10.Image = null;
            Box11.Image = null;
            Box12.Image = null;
            Box13.Image = null;
            Box14.Image = null;
            Box15.Image = null;
            Box16.Image = null;
            Box17.Image = null;
            Box18.Image = null;
            Box19.Image = null;
            Box20.Image = null;
            Box21.Image = null;
            Box22.Image = null;
            Box23.Image = null;
            Box24.Image = null;
            Box25.Image = null;
        }

        public void yellowRemove() //Remove yellow from boxes
        {
            Box1.BackColor = Color.Transparent;
            Box2.BackColor = Color.Transparent;
            Box3.BackColor = Color.Transparent;
            Box4.BackColor = Color.Transparent;
            Box5.BackColor = Color.Transparent;
            Box6.BackColor = Color.Transparent;
            Box7.BackColor = Color.Transparent;
            Box8.BackColor = Color.Transparent;
            Box9.BackColor = Color.Transparent;
            Box10.BackColor = Color.Transparent;
            Box11.BackColor = Color.Transparent;
            Box12.BackColor = Color.Transparent;
            Box13.BackColor = Color.Transparent;
            Box14.BackColor = Color.Transparent;
            Box15.BackColor = Color.Transparent;
            Box16.BackColor = Color.Transparent;
            Box17.BackColor = Color.Transparent;
            Box18.BackColor = Color.Transparent;
            Box19.BackColor = Color.Transparent;
            Box20.BackColor = Color.Transparent;
            Box21.BackColor = Color.Transparent;
            Box22.BackColor = Color.Transparent;
            Box23.BackColor = Color.Transparent;
            Box24.BackColor = Color.Transparent;
            Box25.BackColor = Color.Transparent;
        }

        private void gameReset()
        {
            resetboard();
            boardInvisible();
            imageRemove();
            boxDisable();
            yellowRemove();
            optionsDisable();

            box = 0;
            turn = 0;
            oCount = 4;
            xCount = 4;
            gameType = "";
            oTextBox.Text = "-";
            xTextBox.Text = "-";

            moveBoardOn = false;
            placeMovedPieceOn = false;
            movePieceOn = false;

            player1CheckBox.Visible = true;
            player2CheckBox.Visible = true;
            player1CheckBox.Enabled = true;
            player2CheckBox.Enabled = true;
            player1Label.Visible = true;
            player2Label.Visible = true;
            startButton.Visible = true;
            startButton.Enabled = true;
            mBoard.Visible = true;
        } //reset the gameboard for the next round to start

        private void startButton_Click(object sender, EventArgs e)
        {
            if (player1 != "" && player2 != "")
            {
                player1CheckBox.Visible = false;
                player2CheckBox.Visible = false;
                player1CheckBox.Enabled = false;
                player2CheckBox.Enabled = false;
                player1Label.Visible = false;
                player2Label.Visible = false;
                if (player1 == "human" && player2 == "human")
                {
                    gameType = "humanhuman"; //player 1: human player 2: human
                }
                else if (player1 == "human" && player2 == "bot")
                {
                    gameType = "humanbot"; //player 1: human player 2: bot
                }
                else if (player1 == "bot" && player2 == "human")
                {
                    gameType = "bothuman"; //player 1: bot player 2: human
                }
                else
                {
                    gameType = "botbot"; //player 1: bot player 2: bot
                }
                turnLock = false;
                moveBoardOn = false;
                startButton.Visible = false;
                startButton.Enabled = false;
                optionsDisable();
                turn = 0;
                oCount = 4;
                xCount = 4;
                moveTurn();
            }
            else if (player1CheckBox.CheckedItems.Count == 0)
            {
                textDisplay.Text = "Error! Check a box for player 1.";
            }
            else if (player2CheckBox.CheckedItems.Count == 0)
            {
                textDisplay.Text = "Error! Check a box for player 2.";
            }
            else if (player1CheckBox.CheckedItems.Count == 2)
            {
                textDisplay.Text = "Error! Check only one box for player 1.";
            }
            else
            {
                textDisplay.Text = "Error! Check only one box for player 2.";
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void moveTurn()
        {
            xTextBox.Text = xCount.ToString();
            oTextBox.Text = oCount.ToString();
            if (gameType == "humanhuman") //Moves purely for humans
            {
                if (turn % 2 == 0)
                {
                    if (turn < 4)
                    {
                        placePiece();
                    }
                    else
                    {
                        optionsEnable();
                        textDisplay.Text = "X's Turn! Pick An Option";
                    }
                }
                else
                {
                    if (turn < 5)
                    {
                        placePiece();
                    }
                    else
                    {
                        optionsEnable();
                        textDisplay.Text = "O's Turn! Pick An Option";
                    }
                }
            }
            else if (gameType == "humanbot")
            {
                if (turn % 2 == 0)
                {
                    if (turn < 4)
                    {
                        placePiece();
                    }
                    else
                    {
                        optionsEnable();
                        textDisplay.Text = "X's Turn! Pick An Option";
                    }
                }
                else
                {
                    bot();
                }
            }
            else if (gameType == "bothuman")
            {
                if (turn % 2 == 0)
                {
                    bot();
                }
                else
                {
                    if (turn < 5)
                    {
                        placePiece();
                    }
                    else
                    {
                        optionsEnable();
                        textDisplay.Text = "O's Turn! Pick An Option";
                    }
                }
            }
            else
            {
                bot();
            }
        } //Declares whose turn it is and what they can do.

        public void placePiece()
        {
            piecePlaceOn = true;
            area = boardarea();
            if (turn % 2 == 0)
            {
                if (turn < 4)
                {
                    textDisplay.Text = "X's turn!";
                }
                else
                {
                    textDisplay.Text = "Place a piece, X!";
                }
            }
            else
            {
                if (turn < 5)
                {
                    textDisplay.Text = "O's turn!";
                }
                else
                {
                    textDisplay.Text = "Place a piece, O!";
                }
            }
        } //Place piece code

        public string boardarea()
        {
            //determines which part of the board is visible (In order to place Xs and Os in
            area = ""; //area
            if (tlBoard.Visible == true)
            {
                area = "tl";
                Box1.Enabled = true;
                Box2.Enabled = true;
                Box3.Enabled = true;
                Box6.Enabled = true;
                Box7.Enabled = true;
                Box8.Enabled = true;
                Box11.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;

            }
            else if (tmBoard.Visible == true)
            {
                area = "tm";
                Box2.Enabled = true;
                Box3.Enabled = true;
                Box4.Enabled = true;
                Box7.Enabled = true;
                Box8.Enabled = true;
                Box9.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
            }
            else if (trBoard.Visible == true)
            {
                area = "tr";
                Box3.Enabled = true;
                Box4.Enabled = true;
                Box5.Enabled = true;
                Box8.Enabled = true;
                Box9.Enabled = true;
                Box10.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box15.Enabled = true;
            }
            else if (mlBoard.Visible == true)
            {
                area = "ml";
                Box6.Enabled = true;
                Box7.Enabled = true;
                Box8.Enabled = true;
                Box11.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box16.Enabled = true;
                Box17.Enabled = true;
                Box18.Enabled = true;
            }
            else if (mBoard.Visible == true)
            {
                area = "m";
                Box7.Enabled = true;
                Box8.Enabled = true;
                Box9.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box17.Enabled = true;
                Box18.Enabled = true;
                Box19.Enabled = true;
            }
            else if (mrBoard.Visible == true)
            {
                area = "mr";
                Box8.Enabled = true;
                Box9.Enabled = true;
                Box10.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box15.Enabled = true;
                Box18.Enabled = true;
                Box19.Enabled = true;
                Box20.Enabled = true;
            }
            else if (blBoard.Visible == true)
            {
                area = "bl";
                Box11.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box16.Enabled = true;
                Box17.Enabled = true;
                Box18.Enabled = true;
                Box21.Enabled = true;
                Box22.Enabled = true;
                Box23.Enabled = true;
            }
            else if (bmBoard.Visible == true)
            {
                area = "bm";
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box17.Enabled = true;
                Box18.Enabled = true;
                Box19.Enabled = true;
                Box22.Enabled = true;
                Box23.Enabled = true;
                Box24.Enabled = true;
            }
            else if (brBoard.Visible == true)
            {
                area = "br";
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box15.Enabled = true;
                Box18.Enabled = true;
                Box19.Enabled = true;
                Box20.Enabled = true;
                Box23.Enabled = true;
                Box24.Enabled = true;
                Box25.Enabled = true;
            }
            return area;
        } //Finds where the board is at which enables certain boxes.

        public void totalCheck(int i, int j) //Checks if row = +-3
        {
            area = boardarea();
            bool xWin = false;
            bool oWin = false;
            turnLock = false;
            int total = 0;
            for (int r = i; r < 3 + i; r++) //Checks rows
            {
                total = 0;
                for (int c = j; c < 3 + j; c++)
                {
                    total += board[r, c];
                }
                if (total == 3)
                {
                    xWin = true;
                }
                if (total == -3)
                {
                    oWin = true;
                }
            }
            for (int c = j; c < 3 + j; c++) //Checks columns
            {
                total = 0;
                for (int r = i; r < 3 + i; r++)
                {
                    total += board[r, c];
                }
                if (total == 3)
                {
                    xWin = true;
                }
                if (total == -3)
                {
                    oWin = true;
                }
            }

            if (board[i, j] + board[i + 1, j + 1] + board[i + 2, j + 2] == 3)
            {
                xWin = true;
            }
            if (board[i, j] + board[i + 1, j + 1] + board[i + 2, j + 2] == -3)
            {
                oWin = true;
            }
            if (board[i + 2, j] + board[i + 1, j + 1] + board[i, j + 2] == 3)
            {
                xWin = true;
            }
            if (board[i + 2, j] + board[i + 1, j + 1] + board[i, j + 2] == -3)
            {
                oWin = true;
            }
            if (oWin == true || xWin == true)
            {
                turnLock = true;
                winner(xWin, oWin);
                oWin = false;
                xWin = false;
            }
            else if (turnLock == false)
            {
                nextTurnPrep();
            }
        }

        public void winCheck() //Starts the win check by looking for which area to focus on
        {
            area = boardarea();
            if (area == "tl")
            {
                totalCheck(0, 0);
            }
            else if (area == "tm")
            {
                totalCheck(0, 1);
            }
            else if (area == "tr")
            {
                totalCheck(0, 2);
            }
            else if (area == "ml")
            {
                totalCheck(1, 0);
            }
            else if (area == "m")
            {
                totalCheck(1, 1);
            }
            else if (area == "mr")
            {
                totalCheck(1, 2);
            }
            else if (area == "bl")
            {
                totalCheck(2, 0);
            }
            else if (area == "bm")
            {
                totalCheck(2, 1);
            }
            else if (area == "br")
            {
                totalCheck(2, 2);
            }
        }

        private void winner(bool x, bool o)
        {
            int finalTurns = turn + 1;
            if (x == true && o == false)
            {
                textDisplay.Text = "X wins!";
                MessageBox.Show("X won this round of Tic-Tac-Two!\nX moves left: " + xCount + "\nO moves left: " + oCount + "\nAmount of turns: " + finalTurns, "X wins!");
            }
            else if (x == false && o == true)
            {
                textDisplay.Text = "O wins!";
                MessageBox.Show("O won this round of Tic-Tac-Two!\nO moves left: " + oCount + "\nX moves left: " + xCount + "\nAmount of turns: " + finalTurns, "O wins!");
            }
            else
            {
                textDisplay.Text = "X and O tied!";
                MessageBox.Show("X and O tied in this round of Tic-Tac-Two!\nO moves left: " + oCount + "\nX moves left: " + xCount + "\nAmount of turns: " + finalTurns, "A tie!");
            }
            gameReset();
        } //Declares the winner of the game.

        public void nextTurnPrep() // Prepares for the next round.
        {
            box = 0;
            turn++;
            boxDisable();
            yellowRemove();
            moveBoardOn = false;
            piecePlaceOn = false;
            movePieceOn = false;
            placeMovedPieceOn = false;
            moveTurn();
        }

        private void mBoard_Click(object sender, EventArgs e)
        {

        } // useless

        private void Box1_Click(object sender, EventArgs e)
        {
            boxClick(1);
        } //Box 1

        private void Box2_Click(object sender, EventArgs e)
        {
            boxClick(2);
        } //Box 2

        private void Box3_Click(object sender, EventArgs e)
        {
            boxClick(3);
        } //Box 3
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            boxClick(4);
        } //Box 4

        private void Box5_Click(object sender, EventArgs e)
        {
            boxClick(5);
        } //Box 5

        private void Box6_Click(object sender, EventArgs e)
        {
            boxClick(6);
        } //Box 6

        private void Box7_Click(object sender, EventArgs e)
        {
            boxClick(7);
        } //Box 7

        private void Box8_Click(object sender, EventArgs e)
        {
            boxClick(8);
        } //Box 8

        private void Box9_Click(object sender, EventArgs e)
        {
            boxClick(9);
        } //Box 9

        private void Box10_Click(object sender, EventArgs e)
        {
            boxClick(10);
        } //Box 10

        private void Box11_Click(object sender, EventArgs e)
        {
            boxClick(11);
        } //Box 11

        private void Box12_Click(object sender, EventArgs e)
        {
            boxClick(12);
        } //Box 12

        private void Box13_Click_1(object sender, EventArgs e) //Box 13
        {
            boxClick(13);
        }

        private void Box14_Click(object sender, EventArgs e)
        {
            boxClick(14);
        } //Box 14

        private void Box15_Click(object sender, EventArgs e)
        {
            boxClick(15);
        } //Box 15

        private void Box16_Click(object sender, EventArgs e)
        {
            boxClick(16);
        } //Box 16

        private void Box17_Click(object sender, EventArgs e)
        {
            boxClick(17);
        } //Box 17

        private void Box18_Click(object sender, EventArgs e)
        {
            boxClick(18);
        } //Box 18

        private void Box19_Click(object sender, EventArgs e)
        {
            boxClick(19);
        } //Box 19

        private void Box20_Click(object sender, EventArgs e)
        {
            boxClick(20);
        } //Box 20

        private void Box21_Click(object sender, EventArgs e)
        {
            boxClick(21);
        } //Box 21

        private void Box22_Click(object sender, EventArgs e)
        {
            boxClick(22);
        } //Box 22

        private void Box23_Click(object sender, EventArgs e)
        {
            boxClick(23);
        } //Box 23

        private void Box24_Click(object sender, EventArgs e)
        {
            boxClick(24);
        } //Box 24

        private void Box25_Click(object sender, EventArgs e) //box 25
        {
            boxClick(25);
        } //Box 25

        private void boxClick(int n) //This makes doing the whole click function less line consuming
        {
            if (placeMovedPieceOn == true) //places the piece that is moved
            {
                boardXandY(n);
                boardNum(n);
                winCheckEnable = true;
            }
            else if (movePieceOn == true && placeMovedPieceOn == false) //piece to be moved
            {
                box = n;
                findXandO(n);
                movePiece();
            }
            else if (moveBoardOn == true) //moves tbhe board
            {
                boardInvisible();
                int count = 0;
                for (int i = 7; i < 20; i++)
                {
                    if (i <= 9 || i >= 12 && i <= 14 || i >= 17 && i <= 19)
                    {
                        count++;
                        if (i == n)
                        {
                            botMoveBoard(count);
                            winCheckEnable = true;
                        }
                    }
                }
            }
            else if (board[(n - 1) / 5, (n - 1) % 5] == 0 && piecePlaceOn == true) //places a piece
            {
                boardNum(n);
                boardXandY(n);
                deleteXandO();
                winCheckEnable = true;

            }
            if (winCheckEnable == true)
            {
                winCheckEnable = false;
                winCheck();
            }
        }

        private void boardNum(int n) //Different from the bot board num as that it doesn't redirect to wincheck
        {
            int row = n - 1;
            row /= 5;
            int col = n - 1;
            col %= 5;
            if (turn % 2 == 0)
            {
                board[row, col] = 1;
            }
            else
            {
                board[row, col] = -1;
            }
        }

        private void placeButton_Click(object sender, EventArgs e)
        {
            if (placeButton.Enabled == true)
            {
                if (turn % 2 == 0)
                {
                    if (xCount != 0)
                    {
                        optionsDisable();
                        placePiece();
                    }
                    else
                    {
                        textDisplay.Text = "Error! Invalid move (X has no more pieces)";
                    }
                }
                else
                {
                    if (oCount != 0)
                    {
                        optionsDisable();
                        placePiece();
                    }
                    else
                    {
                        textDisplay.Text = "Error! Invalid move (O has no more pieces)";
                    }
                }
            }
        } //Place piece button check

        private void oTextBox_TextChanged(object sender, EventArgs e)
        {
            //Useless O TextBox
        }

        private void moveBoardButton_Click(object sender, EventArgs e) //Move board code:
        {
            if (moveBoardButton.Enabled == true)
            {
                optionsDisable();
                if (turn % 2 == 0)
                {
                    textDisplay.Text = "Where do you want to move the board, X?";
                    moveBoard();
                }
                else
                {
                    textDisplay.Text = "Where do you want to move the board, O?";
                    moveBoard();
                }
            }
        }

        public void moveBoard()
        {
            moveBoardOn = true;
            area = boardarea();
            boxDisable();
            if (area == "tl")
            {
                Box8.BackColor = Color.Blue;
                Box12.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box8.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;
            }
            if (area == "tm")
            {
                Box7.BackColor = Color.Blue;
                Box9.BackColor = Color.Blue;
                Box12.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box14.BackColor = Color.Blue;
                Box7.Enabled = true;
                Box9.Enabled = true;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
            }
            if (area == "tr")
            {
                Box8.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box14.BackColor = Color.Blue;
                Box8.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
            }
            if (area == "ml")
            {
                Box7.BackColor = Color.Blue;
                Box8.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box17.BackColor = Color.Blue;
                Box18.BackColor = Color.Blue;
                Box7.Enabled = true;
                Box8.Enabled = true;
                Box13.Enabled = true;
                Box17.Enabled = true;
                Box18.Enabled = true;
            }
            if (area == "m")
            {
                Box7.BackColor = Color.Blue;
                Box8.BackColor = Color.Blue;
                Box9.BackColor = Color.Blue;
                Box12.BackColor = Color.Blue;
                Box14.BackColor = Color.Blue;
                Box17.BackColor = Color.Blue;
                Box18.BackColor = Color.Blue;
                Box19.BackColor = Color.Blue;
                Box7.Enabled = true;
                Box8.Enabled = true;
                Box9.Enabled = true;
                Box12.Enabled = true;
                Box14.Enabled = true;
                Box17.Enabled = true;
                Box18.Enabled = true;
                Box19.Enabled = true;
            }
            if (area == "mr")
            {
                Box8.BackColor = Color.Blue;
                Box9.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box18.BackColor = Color.Blue;
                Box19.BackColor = Color.Blue;
                Box8.Enabled = true;
                Box9.Enabled = true;
                Box13.Enabled = true;
                Box18.Enabled = true;
                Box19.Enabled = true;
            }
            if (area == "bl")
            {
                Box12.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box18.BackColor = Color.Blue;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box18.Enabled = true;
            }
            if (area == "bm")
            {
                Box12.BackColor = Color.Blue;
                Box13.BackColor = Color.Blue;
                Box14.BackColor = Color.Blue;
                Box17.BackColor = Color.Blue;
                Box19.BackColor = Color.Blue;
                Box12.Enabled = true;
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box17.Enabled = true;
                Box19.Enabled = true;
            }
            if (area == "br")
            {
                Box13.BackColor = Color.Blue;
                Box14.BackColor = Color.Blue;
                Box18.BackColor = Color.Blue;
                Box13.Enabled = true;
                Box14.Enabled = true;
                Box18.Enabled = true;
            }
        }

        private void movePieceButton_Click(object sender, EventArgs e) //Determines which player wants to place
        {
            movePieceOn = true;
            optionsDisable();
            if (turn % 2 == 0)
            {
                textDisplay.Text = "X! Select a piece to move.";
            }
            else
            {
                textDisplay.Text = "O! Select a piece to move.";
            }
            moveablePieces();
        }

        public void moveablePieces()
        {
            int p = 0;
            if (turn % 2 == 0)
            {
                p = 1;
            }
            else
            {
                p = -1;
            }
            if (board[0, 0] == p)
            {
                Box1.BackColor = Color.Yellow;
                Box1.Enabled = true;
            }
            if (board[0, 1] == p)
            {
                Box2.BackColor = Color.Yellow;
                Box2.Enabled = true;
            }
            if (board[0, 2] == p)
            {
                Box3.BackColor = Color.Yellow;
                Box3.Enabled = true;
            }
            if (board[0, 3] == p)
            {
                Box4.BackColor = Color.Yellow;
                Box4.Enabled = true;
            }
            if (board[0, 4] == p)
            {
                Box5.BackColor = Color.Yellow;
                Box5.Enabled = true;
            }
            if (board[1, 0] == p)
            {
                Box6.BackColor = Color.Yellow;
                Box6.Enabled = true;
            }
            if (board[1, 1] == p)
            {
                Box7.BackColor = Color.Yellow;
                Box7.Enabled = true;
            }
            if (board[1, 2] == p)
            {
                Box8.BackColor = Color.Yellow;
                Box8.Enabled = true;
            }
            if (board[1, 3] == p)
            {
                Box9.BackColor = Color.Yellow;
                Box9.Enabled = true;
            }
            if (board[1, 4] == p)
            {
                Box10.BackColor = Color.Yellow;
                Box10.Enabled = true;
            }
            if (board[2, 0] == p)
            {
                Box11.BackColor = Color.Yellow;
                Box11.Enabled = true;
            }
            if (board[2, 1] == p)
            {
                Box12.BackColor = Color.Yellow;
                Box12.Enabled = true;
            }
            if (board[2, 2] == p)
            {
                Box13.BackColor = Color.Yellow;
                Box13.Enabled = true;
            }
            if (board[2, 3] == p)
            {
                Box14.BackColor = Color.Yellow;
                Box14.Enabled = true;
            }
            if (board[2, 4] == p)
            {
                Box15.BackColor = Color.Yellow;
                Box15.Enabled = true;
            }
            if (board[3, 0] == p)
            {
                Box16.BackColor = Color.Yellow;
                Box16.Enabled = true;
            }
            if (board[3, 1] == p)
            {
                Box17.BackColor = Color.Yellow;
                Box17.Enabled = true;
            }
            if (board[3, 2] == p)
            {
                Box18.BackColor = Color.Yellow;
                Box18.Enabled = true;
            }
            if (board[3, 3] == p)
            {
                Box19.BackColor = Color.Yellow;
                Box19.Enabled = true;
            }
            if (board[3, 4] == p)
            {
                Box20.BackColor = Color.Yellow;
                Box20.Enabled = true;
            }
            if (board[4, 0] == p)
            {
                Box21.BackColor = Color.Yellow;
                Box21.Enabled = true;
            }
            if (board[4, 1] == p)
            {
                Box22.BackColor = Color.Yellow;
                Box22.Enabled = true;
            }
            if (board[4, 2] == p)
            {
                Box23.BackColor = Color.Yellow;
                Box23.Enabled = true;
            }
            if (board[4, 3] == p)
            {
                Box24.BackColor = Color.Yellow;
                Box24.Enabled = true;
            }
            if (board[4, 4] == p)
            {
                Box25.BackColor = Color.Yellow;
                Box25.Enabled = true;
            }
        } //Highlights which pieces are movable

        public void movePiece() //This is to move the piece the player select around
        {
            yellowRemove();
            movePieceOn = false;
            placeMovedPieceOn = true;
            if (turn % 2 == 0)
            {
                textDisplay.Text = "Where do you want to move the piece, X?";
            }
            else
            {
                textDisplay.Text = "Where do you want to move the piece, O?";
            }
            placeableArea();
        }

        public void placeableArea()
        {
            area = boardarea();
            if (tlBoard.Visible == true)
            {
                Box1.BackColor = Color.Yellow;
                Box2.BackColor = Color.Yellow;
                Box3.BackColor = Color.Yellow;
                Box6.BackColor = Color.Yellow;
                Box7.BackColor = Color.Yellow;
                Box8.BackColor = Color.Yellow;
                Box11.BackColor = Color.Yellow;
                Box12.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;

            }
            if (tmBoard.Visible == true)
            {
                Box2.BackColor = Color.Yellow;
                Box3.BackColor = Color.Yellow;
                Box4.BackColor = Color.Yellow;
                Box7.BackColor = Color.Yellow;
                Box8.BackColor = Color.Yellow;
                Box9.BackColor = Color.Yellow;
                Box12.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box14.BackColor = Color.Yellow;
            }
            if (trBoard.Visible == true)
            {
                Box3.BackColor = Color.Yellow;
                Box4.BackColor = Color.Yellow;
                Box5.BackColor = Color.Yellow;
                Box8.BackColor = Color.Yellow;
                Box9.BackColor = Color.Yellow;
                Box10.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box14.BackColor = Color.Yellow;
                Box15.BackColor = Color.Yellow;
            }
            if (mlBoard.Visible == true)
            {
                Box6.BackColor = Color.Yellow;
                Box7.BackColor = Color.Yellow;
                Box8.BackColor = Color.Yellow;
                Box11.BackColor = Color.Yellow;
                Box12.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box16.BackColor = Color.Yellow;
                Box17.BackColor = Color.Yellow;
                Box18.BackColor = Color.Yellow;
            }
            if (mBoard.Visible == true)
            {
                Box7.BackColor = Color.Yellow;
                Box8.BackColor = Color.Yellow;
                Box9.BackColor = Color.Yellow;
                Box12.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box14.BackColor = Color.Yellow;
                Box17.BackColor = Color.Yellow;
                Box18.BackColor = Color.Yellow;
                Box19.BackColor = Color.Yellow;
            }
            if (mrBoard.Visible == true)
            {
                Box8.BackColor = Color.Yellow;
                Box9.BackColor = Color.Yellow;
                Box10.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box14.BackColor = Color.Yellow;
                Box15.BackColor = Color.Yellow;
                Box18.BackColor = Color.Yellow;
                Box19.BackColor = Color.Yellow;
                Box20.BackColor = Color.Yellow;
            }
            if (blBoard.Visible == true)
            {
                Box11.BackColor = Color.Yellow;
                Box12.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box16.BackColor = Color.Yellow;
                Box17.BackColor = Color.Yellow;
                Box18.BackColor = Color.Yellow;
                Box21.BackColor = Color.Yellow;
                Box22.BackColor = Color.Yellow;
                Box23.BackColor = Color.Yellow;
            }
            if (bmBoard.Visible == true)
            {
                Box12.BackColor = Color.Yellow;
                Box13.BackColor = Color.Yellow;
                Box14.BackColor = Color.Yellow;
                Box17.BackColor = Color.Yellow;
                Box18.BackColor = Color.Yellow;
                Box19.BackColor = Color.Yellow;
                Box22.BackColor = Color.Yellow;
                Box23.BackColor = Color.Yellow;
                Box24.BackColor = Color.Yellow;
            }
            if (brBoard.Visible == true)
            {
                Box13.BackColor = Color.Yellow;
                Box14.BackColor = Color.Yellow;
                Box15.BackColor = Color.Yellow;
                Box18.BackColor = Color.Yellow;
                Box19.BackColor = Color.Yellow;
                Box20.BackColor = Color.Yellow;
                Box23.BackColor = Color.Yellow;
                Box24.BackColor = Color.Yellow;
                Box25.BackColor = Color.Yellow;
            }
            if (board[0, 0] != 0 || box == 1 || isBoxEnabled(1) == false)
            {
                Box1.BackColor = Color.Transparent;
                Box1.Enabled = false;
            }
            if (board[0, 1] != 0 || box == 2 || isBoxEnabled(2) == false)
            {
                Box2.BackColor = Color.Transparent;
                Box2.Enabled = false;
            }
            if (board[0, 2] != 0 || box == 3 || isBoxEnabled(3) == false)
            {
                Box3.BackColor = Color.Transparent;
                Box3.Enabled = false;
            }
            if (board[0, 3] != 0 || box == 4 || isBoxEnabled(4) == false)
            {
                Box4.BackColor = Color.Transparent;
                Box4.Enabled = false;
            }
            if (board[0, 4] != 0 || box == 5 || isBoxEnabled(5) == false)
            {
                Box5.BackColor = Color.Transparent;
                Box5.Enabled = false;
            }
            if (board[1, 0] != 0 || box == 6 || isBoxEnabled(6) == false)
            {
                Box6.BackColor = Color.Transparent;
                Box6.Enabled = false;
            }
            if (board[1, 1] != 0 || box == 7 || isBoxEnabled(7) == false)
            {
                Box7.BackColor = Color.Transparent;
                Box7.Enabled = false;
            }
            if (board[1, 2] != 0 || box == 8 || isBoxEnabled(8) == false)
            {
                Box8.BackColor = Color.Transparent;
                Box8.Enabled = false;
            }
            if (board[1, 3] != 0 || box == 9 || isBoxEnabled(9) == false)
            {
                Box9.BackColor = Color.Transparent;
                Box9.Enabled = false;
            }
            if (board[1, 4] != 0 || box == 10 || isBoxEnabled(10) == false)
            {
                Box10.BackColor = Color.Transparent;
                Box10.Enabled = false;
            }
            if (board[2, 0] != 0 || box == 11 || isBoxEnabled(11) == false)
            {
                Box11.BackColor = Color.Transparent;
                Box11.Enabled = false;
            }
            if (board[2, 1] != 0 || box == 12 || isBoxEnabled(12) == false)
            {
                Box12.BackColor = Color.Transparent;
                Box12.Enabled = false;
            }
            if (board[2, 2] != 0 || box == 13 || isBoxEnabled(13) == false)
            {
                Box13.BackColor = Color.Transparent;
                Box13.Enabled = false;
            }
            if (board[2, 3] != 0 || box == 14 || isBoxEnabled(14) == false)
            {
                Box14.BackColor = Color.Transparent;
                Box14.Enabled = false;
            }
            if (board[2, 4] != 0 || box == 15 || isBoxEnabled(15) == false)
            {
                Box15.BackColor = Color.Transparent;
                Box15.Enabled = false;
            }
            if (board[3, 0] != 0 || box == 16 || isBoxEnabled(16) == false)
            {
                Box16.BackColor = Color.Transparent;
                Box16.Enabled = false;
            }
            if (board[3, 1] != 0 || box == 17 || isBoxEnabled(17) == false)
            {
                Box17.BackColor = Color.Transparent;
                Box17.Enabled = false;
            }
            if (board[3, 2] != 0 || box == 18 || isBoxEnabled(18) == false)
            {
                Box18.BackColor = Color.Transparent;
                Box18.Enabled = false;
            }
            if (board[3, 3] != 0 || box == 19 || isBoxEnabled(19) == false)
            {
                Box19.BackColor = Color.Transparent;
                Box19.Enabled = false;
            }
            if (board[3, 4] != 0 || box == 20 || isBoxEnabled(20) == false)
            {
                Box20.BackColor = Color.Transparent;
                Box20.Enabled = false;
            }
            if (board[4, 0] != 0 || box == 21 || isBoxEnabled(21) == false)
            {
                Box21.BackColor = Color.Transparent;
                Box21.Enabled = false;
            }
            if (board[4, 1] != 0 || box == 22 || isBoxEnabled(22) == false)
            {
                Box22.BackColor = Color.Transparent;
                Box22.Enabled = false;
            }
            if (board[4, 2] != 0 || box == 23 || isBoxEnabled(23) == false)
            {
                Box23.BackColor = Color.Transparent;
                Box23.Enabled = false;
            }
            if (board[4, 3] != 0 || box == 24 || isBoxEnabled(24) == false)
            {
                Box24.BackColor = Color.Transparent;
                Box24.Enabled = false;
            }
            if (board[4, 4] != 0 || box == 25 || isBoxEnabled(25) == false)
            {
                Box25.BackColor = Color.Transparent;
                Box25.Enabled = false;
            }
        } // Colors the areas that are placeable

        private void player1CheckBox_SelectedIndexChanged(object sender, EventArgs e) //Checkbox for player 1
        {
            if (player1CheckBox.CheckedItems.Count == 1)
            {
                if (player1CheckBox.GetItemCheckState(0) == CheckState.Checked)
                {
                    player1 = "human";
                    textDisplay.Text = "Player 1: Human";
                }
                if (player1CheckBox.GetItemCheckState(1) == CheckState.Checked)
                {
                    player1 = "bot";
                    textDisplay.Text = "Player 1: Bot";
                }
            }
            else
            {
                player1 = "";
                textDisplay.Text = "Player 1: N/A";
            }
        }

        private void player2CheckBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (player2CheckBox.CheckedItems.Count == 1)
            {
                if (player2CheckBox.GetItemCheckState(0) == CheckState.Checked)
                {
                    player2 = "human";
                    textDisplay.Text = "Player 2: Human";
                }
                if (player2CheckBox.GetItemCheckState(1) == CheckState.Checked)
                {
                    player2 = "bot";
                    textDisplay.Text = "Player 2: Bot";
                }
            }
            else
            {
                player2 = "";
                textDisplay.Text = "Player 2: N/A";
            }
        }

        private void blBoard_Click(object sender, EventArgs e)
        {
            //useless
        }
    }
}