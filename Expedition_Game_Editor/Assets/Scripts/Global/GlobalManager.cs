using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public enum ProgramType
    {
        Editor,
        Game
    }

    static public bool loaded;

    static public ProgramType programType;

    private void Awake()
    {
        loaded = true;

        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        Fixtures.LoadFixtures();

        SceneManager.LoadScene((int)programType);
    }
}
