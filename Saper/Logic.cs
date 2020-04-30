using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class Logic
    {
        internal enum FieldTypeEnum
        {
            Bomb,
            BombNumber,
            Empty
        }

        internal enum GameState
        {
            InProgress,
            Win,
            Loss
        }

        internal class Field
        {
            private FieldTypeEnum fieldType;
            private int bombCount;
            private bool covered;
            public FieldTypeEnum FieldType {
                get { return fieldType; } }
            public int BombCount {
                get { return bombCount; } }
            public bool Covered {
                get { return covered; }
                set { covered = value; }
            }

            internal Field(FieldTypeEnum fieldType, int bombCount = 0)
            {
                this.covered = true;
                this.fieldType = fieldType;
                this.bombCount = bombCount;
            }
        }

        readonly Random generator = new Random();
        private Field[,] board;
        private int fieldsToUncover;
        private GameState state;

        public int BoardWidth
        {
            get { return (int)board.GetLongLength(0); }
        }

        public int BoardHeight
        {
            get { return (int)board.GetLongLength(1); }
        }

        public GameState State
        {
            get{return state;}
        }
        public Logic(int width, int heidth, int bombCount)
        {
            this.fieldsToUncover = (width * heidth) - bombCount;
            this.board = new Field[width, heidth];
            do
            {
                int x = generator.Next(BoardWidth);
                int y = generator.Next(BoardHeight);
                if(board[x,y] == null)
                {
                    board[x, y] = new Field(FieldTypeEnum.Bomb);
                    bombCount--;
                }
            } while (bombCount > 0);
            for(int x = 0; x < BoardWidth; x++)
            {
                for(int y = 0; y < BoardHeight; y++)
                {
                    if(board[x,y] == null)
                    {
                        int localBombCount = 0;
                        for(int xx = x-1; xx <= x+1; xx++)
                        {
                            for(int yy = y-1; yy <= y+1; yy++)
                            {
                                if (xx >= 0 && xx < BoardWidth &&
                                   yy >= 0 && yy < BoardHeight &&
                                   board[xx,yy] != null && board[xx,yy].FieldType == FieldTypeEnum.Bomb)
                                {
                                    localBombCount++;
                                }
                            }
                        }
                        if(localBombCount > 0)
                        {
                            board[x, y] = new Field(FieldTypeEnum.BombNumber, localBombCount);
                        }
                        else
                        {
                            board[x, y] = new Field(FieldTypeEnum.Empty);
                        }
                    }
                }
            }
        }
        internal Field GetField(Point p)
        { return board[p.X, p.Y]; }

        internal void Uncover(Point p)
        {
            Field f = board[p.X, p.Y];
            if(f.Covered)
            {
                if(f.FieldType == FieldTypeEnum.Bomb)
                {
                    state = GameState.Loss;
                    for(int x = 0; x<BoardWidth; x++)
                    {
                        for(int y = 0; y < BoardHeight; y++)
                        { board[x, y].Covered = false; }
                    }
                }
                else if(f.FieldType == FieldTypeEnum.BombNumber)
                {
                    f.Covered = false;
                    fieldsToUncover--;
                }
                else
                {
                    f.Covered = false;
                    fieldsToUncover--;
                    for(int xx = p.X-1; xx <=p.X+1; xx++)
                    {
                        for(int yy = p.Y-1; yy <= p.Y+1; yy++)
                        {
                            if (xx >= 0 && xx < BoardWidth && yy >=0 && yy < BoardWidth)
                            {
                                Uncover(new Point(xx, yy));
                            }
                        }
                    }
                }

                if(fieldsToUncover == 0)
                { state = GameState.Win; }
            }
        }
    }
}
