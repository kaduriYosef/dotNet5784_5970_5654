using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

using System.Data.Common;
using System.Xml.Linq;
using DalApi;
using DO;

internal class DependencyImplementation : IDependency
{
    readonly string s_Dependency_xml = "dependency";
    
    public int Create(Dependency item)
    {
        
        int id = Config.NextDependencyId;
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        XElement xDependency = new XElement(s_Dependency_xml,
                            new XElement("Id",id),
                            new XElement("DependentTask",(item.DependentTask)?? null),
                            new XElement("DependentOnTask", item.DependsOnTask)?? null);
        rootDependency.Add(xDependency);
        XMLTools.SaveListToXMLElement(rootDependency, s_Dependency_xml);
        return id;
    }

    public void Delete(int id)
    {
        if (Read(id) != null)
        {
            XElement rootDependency = XMLTools.LoadListFromXMLElement(s_Dependency_xml);

            (from depend in rootDependency.Elements()
             where (int?)depend.Element("Id") == id
             select depend).FirstOrDefault()?.Remove();

            XMLTools.SaveListToXMLElement(rootDependency, s_Dependency_xml);
        }
        else
            throw new DalDoesNotExistException(s_Dependency_xml);
    }

    public bool DoesExist(int dependent_id, int dependsOn_id)
    {
        XElement rootDependencies = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        return rootDependencies.Any(dep => dep.DependentTask == dependent_id && dep.DependsOnTask == dep.DependsOnTask);

    }

    public Dependency? Read(int id)
    {
        XElement rootDependencies = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        return (from depend in rootDependencies.Elements()
                where (int?)depend.Element("Id") == id
                select xmlToDependency(depend)).FirstOrDefault() ?? throw new DalDoesNotExistException($"Id: {id}, not exist");
                ;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement rootDependencies = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        if (filter == null) return null;
        return rootDependencies.FirstOrDefault(dep => filter(dep));

    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootDependencies = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        List<XElement> dependListXml = rootDependencies.Elements().ToList();
        List<Dependency?> depends = new List<Dependency?>();
        foreach (var depend in dependListXml) 
        { 
            depends.Add(xmlToDependency(depend));
        }
        //if (filter == null)
        //    return rootDependencies.Select(item => item);
        //else
        //    return rootDependencies.Where(filter);
    }

    public void Update(Dependency item)
    {
        XElement Dependencies = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        int index = Dependencies.FindIndex(dep => dep.Id == item.Id);

        if (index != -1)
        {
            Dependencies.RemoveAt(index);
            Dependencies.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"An Dependency with id={item.Id} does not exist.\n");
        }
        XMLTools.SaveListToXMLElement(rootDependency, s_Dependency_xml);
    }
    public Dependency xmlToDependency(XElement item)
    {

        //if (Read((int?)item.Element("Id")))
        //{
            return new Dependency(
                 Id: (int)item.Element("Id"),
                 DependentTask: (int?)item.Element("DependentTask"),
                 DependsOnTask: (int?)item.Element("DEpendentOnTask")
                 );

        //}
        //return;
    }
}
