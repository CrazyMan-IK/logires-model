using Logires;
using Logires.Nodes;

var ticker = new Ticker();

var n1 = new NodeTrue();
var ng = new NodeGenerate(4);
var na = new NodeAnd();
var nn = new NodeNegate();
var no = new NodeOr();
var nl = new NodeLog();

nl.Logged += (value) => {
	Console.Write(value);
	Console.Write(" ");
	Console.Write(ng.Outputs.ElementAt(0));
	Console.Write(" ");
	Console.WriteLine(ticker.Ticks);
};

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
ticker.AddListener(nl);

ticker.Start();

while(true);
