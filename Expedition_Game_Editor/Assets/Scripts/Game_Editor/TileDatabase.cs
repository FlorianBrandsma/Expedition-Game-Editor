using UnityEngine;
using System.Collections;

public class TileDatabase
{
    static public int[] tileSides = new int[4];

    static public void SetTile(int tileNumber)
    {
        switch(tileNumber)
        {
            case 0:

                tileSides[0] = 0;
                tileSides[1] = 0;
                tileSides[2] = 0;
                tileSides[3] = 0;

                break;

            case 1:

                tileSides[0] = 0;
                tileSides[1] = 0;
                tileSides[2] = 0;
                tileSides[3] = 0;

                break;

            case 2:

                tileSides[0] = 0;
                tileSides[1] = 0;
                tileSides[2] = 1;
                tileSides[3] = 0;

                break;

            case 3:

                tileSides[0] = 1;
                tileSides[1] = 0;
                tileSides[2] = 0;
                tileSides[3] = 0;

                break;

            case 4:

                tileSides[0] = 0;
                tileSides[1] = 2;
                tileSides[2] = 0;
                tileSides[3] = 0;

                break;

            case 5:

                tileSides[0] = 0;
                tileSides[1] = 0;
                tileSides[2] = 2;
                tileSides[3] = 0;

                break;

            case 6:

                tileSides[0] = 0;
                tileSides[1] = 1;
                tileSides[2] = 2;
                tileSides[3] = 0;

                break;

            case 7:

                tileSides[0] = 1;
                tileSides[1] = 2;
                tileSides[2] = 0;
                tileSides[3] = 0;

                break;
        }
    }
}
