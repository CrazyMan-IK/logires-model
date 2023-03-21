using System.Text;
using System.Diagnostics;
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

const int rows = 25;
const int columns = 76;

static string CreateFrame()
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
        builder.Append('|');
        builder.Append(empty);
        builder.Append("|\n");
    }

    builder.Append('+');
    builder.Append(new string('-', xCount));
    builder.Append("+\n");

    return builder.ToString();
}

Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;
AppDomain.CurrentDomain.ProcessExit += (s, e) => OnExit();
Console.CancelKeyPress += (s, e) => OnExit();

static void OnExit()
{
    Console.CursorVisible = true;
}

var canvas = new Canvas(new Vector2Int(columns, rows));
var sch1 = new SchemeView(new Vector2(0, 0), new Vector2(columns, rows / 2.0f));
var sch2 = new SchemeView(new Vector2(0, 0), new Vector2(columns, rows));
var frame = new Item(CreateFrame(), Vector2.Zero, false, Constants.White);
var fps = new Item("AVG 0 (0 ms)", Vector2.One, false, Constants.White);

var ticker = new Ticker(60);

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

var bm1 = new BitMerger();
var bm2 = new BitMerger();

var nl1 = new NodeLog();
//var nl2 = new NodeLog();

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

ng1.Outputs.ElementAt(0).Connect(bm1.Inputs.ElementAt(0));
//ng2.Outputs.ElementAt(0).Connect(bm1.Inputs.ElementAt(1));
bm1.Outputs.ElementAt(0).Connect(bm2.Inputs.ElementAt(0));
ng3.Outputs.ElementAt(0).Connect(bm2.Inputs.ElementAt(1));
//bm2.Outputs.ElementAt(0).Connect(nl1.Inputs.ElementAt(0));

var sin = sch1.AddInput<List<bool>>((BitPin)bm1.Inputs.ElementAt(1));
sin.Connect(ng2.Outputs.ElementAt(0));

var sout = sch1.AddOutput<List<bool>>((BitPin)bm2.Outputs.ElementAt(0));
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

//canvas.Append(sch1.Visual);
canvas.Append(sch2.Visual);
//canvas.Append(new Line(new IReadOnlyVector2<float>[] { new Vector2(0, 14), new Vector2(columns, 14) }, Constants.White));
canvas.Append(frame);
canvas.Append(fps);

Console.Clear();
var timer = Stopwatch.StartNew();
var frames = 0;
var total = 0.0f;
while (true)
{
		var deltaTime = timer.ElapsedMilliseconds / 1000f;

    ticker.Update(deltaTime);
    timer.Restart();

    total += deltaTime;
    frames++;

		while (total >= 1)
		{
				var totalDelta = total / frames;
		
    		fps.Text = $"AVG {MathExtensions.Round(1 / totalDelta)} ({MathExtensions.Round(totalDelta * 1000, 2)} ms)";

    		total -= 1;
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
