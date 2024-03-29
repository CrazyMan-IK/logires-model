using System.Text;
using System.Diagnostics;
using Mindmagma.Curses;
using BrailleCanvas;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Extensions;
using BrailleCanvas.Figures;
using BrailleCanvas.Models;
using Logires;
using Logires.Interfaces;
using Logires.Nodes;
using Logires.Pins;
using Test;

int columns = Console.WindowWidth;
int rows = (int)Math.Round(Console.WindowWidth * 0.31);
float hcolumns = columns / 2.0f;
float hrows = rows / 2.0f;

string CreateFrame()
{
    var builder = new StringBuilder(columns * rows);

    var xCount = columns - 2;
    var yCount = rows - 2;

    builder.Append('+');
    builder.Append(new string('-', xCount));
    builder.Append("+\n");

    var empty = new string('\u2800', xCount);
    for (int i = 0; i < yCount; i++)
    {
        builder.Append(i % 10);
        //builder.Append('|');
        builder.Append(empty);
        builder.Append("|\n");
        //builder.Append($"{i % 10}\n");
    }

    builder.Append('+');
    builder.Append(new string('-', xCount));
    builder.Append("+\n");

    return builder.ToString();
}

Console.Write(new string('\n', Console.WindowHeight));
Console.SetCursorPosition(0, 0);
//OnExit();

Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;
AppDomain.CurrentDomain.ProcessExit += (s, e) => OnExit();
Console.CancelKeyPress += (s, e) => OnExit();

static void OnExit()
{
		//Console.WriteLine(Console.WindowHeight);
		/*Console.Write(new string('\n', Console.WindowHeight));
		Console.SetCursorPosition(0, 0);*/
		Console.Clear();

		Console.SetCursorPosition(0, 0);//-Console.WindowHeight);

    Console.CursorVisible = true;
}

var canvas = new Canvas(new Vector2Int(columns, rows));
var sch1 = new SchemeView(new Vector2(0, 0), new Vector2(columns, rows / 2.0f));
var sch2 = new SchemeView(new Vector2(0, 0), new Vector2(columns, rows));
var frame = new Item(CreateFrame(), Vector2.Zero, false, Constants.White);
var fps = new Item("AVG 0 (0 ms)", Vector2.One, false, Constants.White);

var ticker = new Ticker(4);

/*var n1 = new NodeTrue();
var ng = new NodeGenerate(4);
var na = new NodeAnd();
var nn = new NodeNegate();
var no = new NodeOr();

var rg = new RangeGetter();
var imn = new IteratorMoveNext();*/

var ng1 = new NodeGenerate(1);
var ng2 = new NodeGenerate(2);
var ng3 = new NodeGenerate(4);
var ng4 = new NodeGenerate(2);

var bm1 = new BitMerger();
var bm2 = new BitMerger();

var nl1 = new NodeLog();
var nl2 = new NodeLog();

var ri1 = new RemoteInput();

var ro1 = new RemoteOutput();

/*var sch2 = new Scheme(new List<Node>() {
	ng1,
	ng2,
	ng3,

	bm1,
	bm2,

	nl1
});*/

//var sch1 = new Scheme(null);
//sch1.Add(sch2);

//sch2.Add(sch1);

/*nl.Logged += (value) => {
	Console.Write(value);
	Console.Write(" ");
	Console.Write(ng.Outputs.ElementAt(0));
	Console.Write(" ");
	Console.WriteLine(ticker.Ticks);
};*/

/*nodes.Add(new NodeView(n1, new Vector2(4, 2), Constants.White));
nodes.Add(new NodeView(ng, new Vector2(4, 10), Constants.White));
nodes.Add(new NodeView(na, new Vector2(16, 6), Constants.White));
nodes.Add(new NodeView(nn, new Vector2(28, 6), Constants.White));
nodes.Add(new NodeView(no, new Vector2(40, 6), Constants.White));
nodes.Add(new NodeView(nl, new Vector2(56, 6), Constants.White));

n1.Outputs.ElementAt(0).Connect(na.Inputs.ElementAt(0));
ng.Outputs.ElementAt(0).Connect(na.Inputs.ElementAt(1));
na.Outputs.ElementAt(0).Connect(nn.Inputs.ElementAt(0));
nn.Outputs.ElementAt(0).Connect(no.Inputs.ElementAt(0));
no.Outputs.ElementAt(0).Connect(nl.Inputs.ElementAt(0));

ticker.AddListener(n1);
ticker.AddListener(ng);
ticker.AddListener(na);
ticker.AddListener(nn);
ticker.AddListener(no);
ticker.AddListener(nl);*/

/*nodes.Add(new NodeView(rg, new Vector2(4, 6), Constants.Purple));
nodes.Add(new NodeView(imn, new Vector2(16, 6), Constants.Purple));

var ii1 = new IntegerPin(false);
var ii2 = new IntegerPin(false);

ii2.Value = 10;

ii1.Connect(rg.Inputs.ElementAt(0));
ii2.Connect(rg.Inputs.ElementAt(1));
rg.Outputs.ElementAt(0).Connect(imn.Inputs.ElementAt(0));

ticker.AddListener(rg);
ticker.AddListener(imn);*/

//*/
sch1.Add(new NodeView(ng1, new Vector2(16, 3), Constants.Purple));
sch1.Add(new NodeView(ng3, new Vector2(16, 11), Constants.Purple));
sch1.Add(new NodeView(bm1, new Vector2(28, 7), Constants.Purple));
sch1.Add(new NodeView(bm2, new Vector2(40, 11), Constants.Purple));
//sch1.Add(new NodeView(nl1, new Vector2(56, 11), Constants.Purple));
//*/
sch2.Add(new NodeView(ng2, new Vector2(20, 12), Constants.Purple));
sch2.Add(new NodeView(sch1.Scheme, new Vector2(38, 12), Constants.Yellow));
sch2.Add(new NodeView(nl1, new Vector2(56, 12), Constants.Purple));

sch2.Add(new NodeView(ro1, new Vector2(20, 4), Constants.Purple));
sch2.Add(new NodeView(nl2, new Vector2(31.25f, 4), Constants.Purple));
sch2.Add(new NodeView(ng4, new Vector2(44.75f, 4), Constants.Purple));
sch2.Add(new NodeView(ri1, new Vector2(56, 4), Constants.Purple));

ro1.Outputs.ElementAt(4).Connect(nl2.Inputs.ElementAt(0));
ng4.Outputs.ElementAt(0).Connect(ri1.Inputs.ElementAt(0));

ng1.Outputs.ElementAt(0).Connect(bm1.Inputs.ElementAt(0));
//ng2.Outputs.ElementAt(0).Connect(bm1.Inputs.ElementAt(1));
bm1.Outputs.ElementAt(0).Connect(bm2.Inputs.ElementAt(0));
ng3.Outputs.ElementAt(0).Connect(bm2.Inputs.ElementAt(1));
//bm2.Outputs.ElementAt(0).Connect(nl1.Inputs.ElementAt(0));

var sin = sch1.AddInput((BitPin)bm1.Inputs.ElementAt(1));
sin.Connect(ng2.Outputs.ElementAt(0));

var sout = sch1.AddOutput((BitPin)bm2.Outputs.ElementAt(0));
sout.Connect(nl1.Inputs.ElementAt(0));

/*ticker.AddListener(ng1);
ticker.AddListener(ng2);
ticker.AddListener(ng3);
ticker.AddListener(bm1);
ticker.AddListener(bm2);
ticker.AddListener(nl);*/
ticker.AddListener(sch1);
ticker.AddListener(sch2);
//ticker.AddListener(nl2);

/*var firstNode = new NodeGenerate();
OneOf<NodeNegate, NodeGenerate> lastNode = firstNode;

nodes.Add(new NodeView(firstNode, new Vector2(5, 3), Constants.White));
ticker.AddListener(firstNode);

var negates = new List<NodeNegate>();
for (int i = 0; i < 4; i++)
{
	for (int j = 0; j < 4; j++)
	{
		if (i + j == 0)
		{
			continue;
		}

		if (j == 0)
		{
			lastNode = new NodeGenerate(i + 1);
		}
		else
		{
			var currentNode = new NodeNegate();
			var ho = lastNode.Match<IHaveOutputs>(x => x, x => x);
			
			ho.Outputs.ElementAt(0).Connect(currentNode.Inputs.ElementAt(0));
			
			lastNode = currentNode;
			
			if (j == 3)
			{
				negates.Add(currentNode);
			}
		}

		var n = lastNode.Match<Node>(x => x, x => x);

		var halfW = 4;
		var halfH = 2;
		nodes.Add(new NodeView(n, new Vector2(j * (halfW * 3) + halfW + 1, i * (halfH * 3) + halfH + 1), Constants.White));
		ticker.AddListener(n);
	}
}

var na1 = new NodeAnd();
var na2 = new NodeAnd();
var nl1 = new NodeLog();
var nl2 = new NodeLog();

na1.Inputs.ElementAt(0).Connect(negates[0].Outputs.ElementAt(0));
na1.Inputs.ElementAt(1).Connect(negates[1].Outputs.ElementAt(0));
na2.Inputs.ElementAt(0).Connect(negates[2].Outputs.ElementAt(0));
na2.Inputs.ElementAt(1).Connect(negates[3].Outputs.ElementAt(0));

nl1.Inputs.ElementAt(0).Connect(na1.Outputs.ElementAt(0));
nl2.Inputs.ElementAt(0).Connect(na2.Outputs.ElementAt(0));

nodes.Add(new NodeView(na1, new Vector2(53, 6), Constants.White));
nodes.Add(new NodeView(na2, new Vector2(53, 18), Constants.White));
nodes.Add(new NodeView(nl1, new Vector2(65, 6), Constants.White));
nodes.Add(new NodeView(nl2, new Vector2(65, 18), Constants.White));

ticker.AddListener(na1);
ticker.AddListener(na2);
ticker.AddListener(nl1);
ticker.AddListener(nl2);*/

//var schV = new SchemeView(sch2);
//canvas.Append(schV.Visual);

/*foreach (var node in nodes)
{
    canvas.Append(node.Visual);
}*/

var p1 = new Vector2(10, 12);
var p2 = new RefVector2();
var p3 = new RefVector2();
var p4 = new Vector2(columns - 10, rows - 12);
var p5 = new Vector2(columns - 10, rows - 2);
var p6 = new Vector2(columns - 40, rows - 4);

var b1 = new Bezier(new IReadOnlyVector2<float>[] { p1, p2, p3 }, Constants.Red);
var b2 = new Bezier(new IReadOnlyVector2<float>[] { p1, p2, p3, p4 }, Constants.Green);
var b3 = new Bezier(new IReadOnlyVector2<float>[] { p1, p2, p3, p4, p5 }, Constants.Blue);
var b4 = new Bezier(new IReadOnlyVector2<float>[] { p1, p2, p3, p4, p5, p6 }, Constants.White);

float t = 0;
void UpdatePoints(float deltaTime)
{
	t += deltaTime;

	p2.X = 25 + MathF.Sin(t + MathF.PI) * 10;
	p2.Y = hrows + MathF.Cos(t + MathF.PI) * hrows;
	p3.X = 40 + MathF.Sin(t) * 20;
	p3.Y = hrows + MathF.Cos(t) * hrows;

	//b1.SetPoints(new IReadOnlyVector2<float>[] { p1, p2, p3 });
	//b2.SetPoints(new IReadOnlyVector2<float>[] { p1, p2, p3, p4 });
	//b3.SetPoints(new IReadOnlyVector2<float>[] { p1, p2, p3, p4, p5 });
	//b4.SetPoints(new IReadOnlyVector2<float>[] { p1, p2, p3, p4, p5, p6 });
}

UpdatePoints(0);

////canvas.Append(sch1.Visual);
canvas.Append(sch2.Visual);
canvas.Append(b1);
canvas.Append(b2);
canvas.Append(b3);
canvas.Append(b4);
////canvas.Append(new Line(new IReadOnlyVector2<float>[] { new Vector2(0, 14), new Vector2(columns, 14) }, Constants.White));
canvas.Append(frame);
canvas.Append(fps);

//Console.WriteLine(canvas.StringValue());
//Console.SetCursorPosition(-1, -1);

Console.Clear();
var timer = Stopwatch.StartNew();
var frames = 0;
var total = 0.0f;
while (true)
{
    var elapsed = timer.ElapsedTicks;
    var deltaTime = elapsed * 1.0f / Stopwatch.Frequency;

    ticker.Update(deltaTime);
    UpdatePoints(deltaTime);
    timer.Restart();

    total += deltaTime;
    frames++;

    if (total >= 1)
    {
        var totalDelta = total / frames;
		
        fps.Text = $"AVG {MathExtensions.Round(1 / totalDelta)} ({MathExtensions.Round(totalDelta * 1000, 2)} ms)";

        total = 0;
        frames = 0;
    }

    Console.WriteLine(canvas.StringValue());
    Console.SetCursorPosition(0, 0);
}

/*
var c1 = PinsConverter.GetConvertator<BooleanPin, BitPin, bool, List<bool>>();
var c2 = PinsConverter.GetConvertator<BitPin, BooleanPin, List<bool>, bool>();

Console.WriteLine(string.Join("; ", c1?.Invoke(true)));
Console.WriteLine(c2?.Invoke(new List<bool>() { false, false, true, false }));
*/
