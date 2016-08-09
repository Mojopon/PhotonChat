using UnityEngine;
using System.Collections;
using UniRx;

public struct Inputs
{
    public ArrowInput direction;
}


public enum ArrowInput
{
    None,
    Left,
    Up,
    Right,
    Down,
}

public class PlayerInput : MonoBehaviour
{
    public IObservable<Inputs> InputsObservable { get { return inputStream.AsObservable();  } }
    private ISubject<Inputs> inputStream = new Subject<Inputs>();

	void Start ()
    {
        Observable.EveryUpdate()
                  .Select(x => OnArrowPressed())
                  .Subscribe(x => inputStream.OnNext(new Inputs() { direction = x }))
                  .AddTo(gameObject);
	}

    ArrowInput OnArrowPressed()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return ArrowInput.Left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return ArrowInput.Up;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return ArrowInput.Right;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return ArrowInput.Down;
        }

        return ArrowInput.None;
    }
}
