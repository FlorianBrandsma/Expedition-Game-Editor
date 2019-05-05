using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDataController
{
    ICollection DataList                { get; set; }
    Enums.DataType DataType             { get; }
    SearchParameters SearchParameters   { get; set; }

    void InitializeController();
    void GetData(List<int> idList);
    void GetData(SearchData searchData);
    void ReplaceData(IEnumerable elementData);
}
