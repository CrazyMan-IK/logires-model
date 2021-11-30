/*import { ITickable, isHaveInputs, isHaveOutputs } from '../interfaces.js';
import { IFigure } from '../../braille-canvas/interfaces.js';
import { Vector2 } from '../../math.js';

export default abstract class Node implements ITickable {
  private _needUpdate = true;

  protected listenUpdates() {
    if (isHaveOutputs(this)) {
      for (const output of this.Outputs) {
        output.on('UpdateRequested', (ticks: number) => {
          if (this._needUpdate) {
            this._needUpdate = false;
            this.update(ticks);
          }
        });
      }
    }
  }

  public markDirty() {
    this._needUpdate = true;

    if (isHaveOutputs(this)) {
      for (const output of this.Outputs) {
        output.markDirty();
      }
    }
  }

  public tick(ticks: number): void {
    if (isHaveInputs(this)) {
      for (const input of this.Inputs) {
        input.update(ticks);
      }
    }

    if (this._needUpdate) {
      this._needUpdate = false;
      this.update(ticks);
    }

    if (isHaveOutputs(this)) {
      for (const output of this.Outputs) {
        output.update(ticks);
      }
    }
  }

  public abstract update(ticks: number): void;
}*/

using Logires.Interfaces;

namespace Logires.Nodes;

public abstract class Node : ITickable
{
	private bool _needUpdate = true;

	public void MarkDirty()
	{
		_needUpdate = true;
		
		if (this is IHaveOutputs haveOutputs)
		{
		  foreach (var output in haveOutputs.Outputs)
		  {
		    output.MarkDirty();
		  }
		}
	}

	public void Tick(long ticks)
	{
		Console.WriteLine(GetType().Name);
	
		if (this is IHaveInputs haveInputs)
		{
		  foreach (var input in haveInputs.Inputs)
		  {
		    input.Update(ticks);
		  }

		  Console.WriteLine("Have Inputs");
		  Console.WriteLine(string.Join(", ", haveInputs.Inputs));
		}
		
		if (this._needUpdate)
		{
		  _needUpdate = false;
		  Update(ticks);
		}
		
		if (this is IHaveOutputs haveOutputs)
		{
		  foreach (var output in haveOutputs.Outputs)
		  {
		    output.Update(ticks);
		  }

		  Console.WriteLine("Have Outputs");
		  Console.WriteLine(string.Join(", ", haveOutputs.Outputs));
		}
	}

	public abstract void Update(long ticks);
}
