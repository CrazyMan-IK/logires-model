using Logires;
using Logires.Nodes;

var ticker = new Ticker();

var n1 = new NodeTrue();
var nn = new NodeNegate();

n1.Outputs.First().Connect(nn.Inputs.First());

ticker.AddListener(n1);
ticker.AddListener(nn);

ticker.Start();

while(true);
