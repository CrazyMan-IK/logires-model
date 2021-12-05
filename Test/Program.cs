using System.Text;
using OneOf;
using Logires;
using Logires.Pins;
using Logires.Nodes;
using Logires.Interfaces;
using BrailleCanvas;
using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using Test;

const int rows = 25;
const int columns = 76;

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
var nodes = new List<NodeView>();
var frame = new Item(CreateFrame(), Vector2.Zero, false, Constants.White);

var ticker = new Ticker(4);

var n1 = new NodeTrue();
var ng = new NodeGenerate(4);
var na = new NodeAnd();
var nn = new NodeNegate();
var no = new NodeOr();
var nl = new NodeLog();

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

var firstNode = new NodeGenerate();
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
ticker.AddListener(nl2);

ticker.Start();

foreach (var node in nodes)
{
    if (node.Node is not IHaveOutputs haveOutputs)
    {
        continue;
    }

    foreach (var output in haveOutputs.Outputs)
    {
        if (output is not BooleanPin booleanOutput)
        {
            continue;
        }

        foreach (var node2 in nodes)
        {
						if (node == node2)
						{
							continue;
						}
        	
            if (node2.Node is not IHaveInputs haveInputs)
            {
                continue;
            }

            foreach (var input in haveInputs.Inputs)
            {
                if (!output.IsConnectedWith(input))
                {
                    continue;
                }

                canvas.Append(new Line(new[] { node.Position, node2.Position }, new Ternary<Color>(() => booleanOutput.Value, Constants.Green, Constants.Red)));
            }
        }
    }
}

foreach (var node in nodes)
{
	canvas.Append(node.Figure);
}

canvas.Append(frame);

while(true)
{
	Console.WriteLine(canvas.StringValue());
	Console.SetCursorPosition(0, 0);
}

/*
var c1 = PinsConverter.GetConvertator<BooleanPin, BitPin, bool, List<bool>>();
var c2 = PinsConverter.GetConvertator<BitPin, BooleanPin, List<bool>, bool>();

Console.WriteLine(string.Join("; ", c1?.Invoke(true)));
Console.WriteLine(c2?.Invoke(new List<bool>() { false, false, true, false }));
*/
