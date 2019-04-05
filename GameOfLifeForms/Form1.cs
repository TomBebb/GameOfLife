using GameOfLife;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLifeForms
{
    public partial class Form1 : Form, IDisposable
    {
        private Timer genTimer = new System.Windows.Forms.Timer();
        private Graphics graphics;
        private ConwaysGame game;
        private SolidBrush aliveBrush, deadBrush, justAliveBrush, justDeadBrush;
        private bool paused = false;
        private bool mouseDown = false;
        public Form1()
        {
            InitializeComponent();
            var gridSize = 50;
            var seed = new Grid<CellState>(gridSize, gridSize);
            var rng = new Random();
            foreach (var cellPos in seed)
                seed[cellPos] = rng.NextDouble() > 0.5 ? CellState.Alive : CellState.Dead;

            game = new ConwaysGame(seed);

            aliveBrush = new SolidBrush(Color.Green);
            deadBrush = new SolidBrush(Color.Red);
            justAliveBrush = new SolidBrush(Color.White);
            justDeadBrush = new SolidBrush(Color.Black);

            genTimer.Tick += new EventHandler(HandeGenerationTimerEvent);
            genTimer.Interval = 50;
            genTimer.Start();

            Click += Form1_Click;
            MouseDown += Form1_MouseDown;
            MouseUp += Form1_MouseUp;
            MouseMove += Form1_MouseMove;
            KeyDown += Form1_KeyDown;
        }

        private int calcCellSize()
        {
            var cellWidth = Width / game.CurrentGen.Columns;
            var cellHeight = Height / game.CurrentGen.Rows;

            return Math.Min(cellWidth, cellHeight);
        }
        private void HandeGenerationTimerEvent(object sender, EventArgs e)
        {
            if (!paused)
                game.DoGeneration();
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            var args = (MouseEventArgs) e;
            
            if (args.Button == MouseButtons.Left)
            {
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs args)
        {
            if (!mouseDown) return;

            var location = args.Location;
            var cellSize = calcCellSize();
            var cellPos = new CellPos(location.Y / cellSize, location.X / cellSize);

            var grid = game.CurrentGen;
            var targetState = args.Button == MouseButtons.Left ? CellState.Alive : CellState.Dead;
            if (grid.ContainsPos(cellPos))
                grid[cellPos] = targetState;
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e) =>
            mouseDown = false;
        private void Form1_MouseDown(object sender, MouseEventArgs e) =>
            mouseDown = true;

        private void Reset()
        {
            foreach (var cellPos in game.CurrentGen)
                game.CurrentGen[cellPos] = CellState.Dead;
        }
        private void SetupExploder()
        {
            Reset();
            var grid = game.CurrentGen;
            var centre = new CellPos(grid.Rows / 2, grid.Columns / 2);
            // top
            grid[centre] = CellState.Alive;
            grid[new CellPos(centre.Row + 1, centre.Column)] = CellState.Alive;
            grid[new CellPos(centre.Row + 1, centre.Column - 1)] = CellState.Alive;
            grid[new CellPos(centre.Row + 1, centre.Column + 1)] = CellState.Alive;
            grid[new CellPos(centre.Row + 2, centre.Column - 1)] = CellState.Alive;
            grid[new CellPos(centre.Row + 2, centre.Column + 1)] = CellState.Alive;
            // bottom
            grid[new CellPos(centre.Row + 3, centre.Column)] = CellState.Alive;
        }
        private void SetupSpaceship()
        {
            Reset();
            var grid = game.CurrentGen;
            var centre = new CellPos(grid.Rows / 2, grid.Columns / 2);
            // bottom row
            // centre is bottom left
            grid[centre] = CellState.Alive;
            grid[new CellPos(centre.Row, centre.Column + 3)] = CellState.Alive;
            // almost bottom row
            grid[new CellPos(centre.Row - 1, centre.Column + 4)] = CellState.Alive;
            // almost bottom row
            grid[new CellPos(centre.Row - 1, centre.Column + 4)] = CellState.Alive;
            // almost bottom row
            grid[new CellPos(centre.Row - 2, centre.Column + 4)] = CellState.Alive;
            grid[new CellPos(centre.Row - 2, centre.Column)] = CellState.Alive;
            // top row
            grid[new CellPos(centre.Row - 3, centre.Column + 1)] = CellState.Alive;
            grid[new CellPos(centre.Row - 3, centre.Column + 2)] = CellState.Alive;
            grid[new CellPos(centre.Row - 3, centre.Column + 3)] = CellState.Alive;
            grid[new CellPos(centre.Row - 3, centre.Column + 4)] = CellState.Alive;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                paused = !paused;
            if (e.KeyCode == Keys.R)
            {
                Reset();
            }
            if (e.KeyCode == Keys.E)
            {
                SetupExploder();
            }
            if (e.KeyCode == Keys.S)
            {
                SetupSpaceship();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var bufferedContext = BufferedGraphicsManager.Current;

            var buffer = bufferedContext.Allocate(CreateGraphics(), DisplayRectangle);

            var graphics = buffer.Graphics;

            var currentGrid = game.CurrentGen;
            var lastGrid = game.LastGen;

            var cellSize = calcCellSize();

            foreach (var cellPos in currentGrid)
            {
                var rect = new Rectangle(cellPos.Column * cellSize, cellPos.Row * cellSize, cellSize, cellSize);
                SolidBrush brush;
                if (currentGrid[cellPos] == CellState.Alive)
                    brush = lastGrid[cellPos] == CellState.Dead ? justAliveBrush : aliveBrush;
                else
                    brush = lastGrid[cellPos] == CellState.Alive ? justDeadBrush : deadBrush;
                
                graphics.FillRectangle(brush, rect);
            }
            buffer.Render();
            buffer.Dispose();
        }
        void IDisposable.Dispose()
        {

            aliveBrush.Dispose();
            deadBrush.Dispose();
            justAliveBrush.Dispose();
            justDeadBrush.Dispose();
        }
    }
}
