using UnityEngine;
using System.Collections;
using UniRx;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _PlayerInput;
    [SerializeField]
    private Vector2 _BoardSize = new Vector2(4, 4);
    [SerializeField]
    private GameObject _CubePrefab;

	void Start ()
    {
	
	}
}
