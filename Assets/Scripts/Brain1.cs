using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain1 : MonoBehaviour, IBrain
{


    public string Name { get; set; }
    public string Type { get; set; }
    public double Mutationrate { get; set; }
    public int CurrentMoves { get; set; }
    public int TotalMoves { get; set; }

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentMoves >= TotalMoves ) return;

    }

    public void Mutate()
    {

    }
}
