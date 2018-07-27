using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager
{
    public class TestPath
    {
        public Path test = new Path(new List<int>(), new List<int>());

        public Path GetPath()
        {
            return test;
        }
    }
}
