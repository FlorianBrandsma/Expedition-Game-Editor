//using UnityEngine;
//using System.Collections.Generic;

///// <summary>
///// Object graphics sorted by id, so that each unique object graphic will be listed together
///// </summary>
//public class ObjectGraphicType
//{
//    public ObjectGraphicData data = new ObjectGraphicData();

//    public List<ObjectGraphic> objectGraphicPool = new List<ObjectGraphic>();
//}

//public class PoolManager
//{
//    //List of objects, identified by data (table and id mainly)
//    //So one list for Polearms (the item), one list for rocks (the object)
//    //Polearm refers to a model. This model may be used for other items as well.
//    //Create a list for each model

//    public List<GameElement> gameElementPool = new List<GameElement>();
//    public List<UIElement> UIElementPool = new List<UIElement>();
//    public List<ObjectGraphicType> objectGraphicList = new List<ObjectGraphicType>();

//    public void Test()
//    {
//        for(int i = 0; i < 5; i++)
//        {
//            ObjectGraphicType new_object_type = new ObjectGraphicType();

//            new_object_type.data.id = i;

//            for (int j = 0; j < 5; j++)
//            {
//                ObjectGraphic new_graphic = Object.Instantiate(Resources.Load<ObjectGraphic>("Objects/Item/0"));

//                new_graphic.data.id = j;
//                new_graphic.data.type = i;

//                new_object_type.objectGraphicPool.Add(new_graphic);
//            }

//            objectGraphicList.Add(new_object_type);
//        }

//        //foreach(ObjectGraphicType type in objectGraphicList)
//        //{
//        //    foreach(ObjectGraphic graphic in type.objectGraphicPool)
//        //    {
//        //        Debug.Log("Type " + type.data.id + ", graphic " + graphic.data.id);
//        //    }
//        //}
//    }
//}
