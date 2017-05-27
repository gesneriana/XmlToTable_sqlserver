using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Xml;
using System.Collections.Generic;

public partial class UserDefinedFunctions
{

    /// <summary>
    /// 读取xml,转换为table
    /// </summary>
    /// <param name="xml">数据库转换xml文档对象</param>
    /// <param name="root">需要读取的根节点的名称</param>
    /// <returns></returns>
    [SqlFunction(FillRowMethodName = "FillTableRow", TableDefinition = "id int,name nvarchar(20),value nvarchar(500)")]
    public static System.Collections.IEnumerable XmlToTable(SqlXml xml, string root)
    {
        List<xmlToTable.paraVlues> pvList = new List<xmlToTable.paraVlues>();
        int count = 1;
        if (!xml.IsNull)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xml.Value);
                var list = doc.GetElementsByTagName(root);
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].HasChildNodes)
                        {
                            for (int j = 0; j < list[i].ChildNodes.Count; j++)
                            {
                                var node = list[i].ChildNodes[j];
                                var pv = new xmlToTable.paraVlues() { id = count++, name = node.Name, value = node.InnerText };
                                pvList.Add(pv);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pvList.Add(new xmlToTable.paraVlues() { id = count++, name = "error", value = ex.Message });
            }
        }
        // 在此处放置代码
        return pvList;
    }

    /// <summary>
    /// 填充临时表的方法
    /// </summary>
    /// <param name="obj">遍历的每个对象,例如返回值为 List<xmlToTable.paraVlues>, 那么obj的类型是 xmlToTable.paraVlues</param>
    /// <param name="id">表的id</param>
    /// <param name="name">表的name字段</param>
    /// <param name="value">表的value字段</param>
    private static void FillTableRow(object obj, ref SqlInt32 id, ref SqlString name, ref SqlString value)
    {
        if (obj != null)
        {
            var pv = (xmlToTable.paraVlues)obj;
            if (pv != null)
            {
                id = pv.id;
                name = pv.name;
                value = pv.value;
            }
        }
    }
}