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
    readonly string s_Dependency_xml = "dependencys";

    public int Create(Dependency item)
    {

        int id = Config.NextDependencyId;
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        XElement xDependency = new XElement(s_Dependency_xml,
                            new XElement("Id", id),
                            new XElement("DependentTask", (item.DependentTask) ?? null),
                            new XElement("DependentOnTask", item.DependsOnTask) ?? null);
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

        var elems = (from item in rootDependencies.Elements("Dependency")
                    select xmlToDependency(item))
                    .ToList();

        return elems.Any( x => x.DependentTask== dependent_id && x.DependsOnTask == dependsOn_id);
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
        if (filter == null) return null;
        else
        return XMLTools.LoadListFromXMLElement(s_Dependency_xml).Elements()
                    .Select(dep => xmlToDependency(dep)).FirstOrDefault(filter!);


    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_Dependency_xml).Elements()
                    .Select(dep => xmlToDependency(dep));
        else
            return XMLTools.LoadListFromXMLElement(s_Dependency_xml).Elements()
                    .Select(dep => xmlToDependency(dep)).Where(filter!);

    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"the dependency with id: {item.Id} not exist");

        Delete(item.Id);
        XElement rootDependencies = XMLTools.LoadListFromXMLElement(s_Dependency_xml);
        rootDependencies.Add(item);

        XMLTools.SaveListToXMLElement(rootDependencies, s_Dependency_xml);
    }
    public DO.Dependency? xmlToDependency(XElement item)
    {
        return new Dependency(
             Id: item.ToIntNullable("Id") ?? throw new convertExeption("can't convert it"),
             DependentTask: (int?)item.Element("DependentTask") ?? null,
             DependsOnTask: (int?)item.Element("DependentOnTask") ?? null
             );
    }
}
