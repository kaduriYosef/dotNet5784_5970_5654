using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

using System.Data.Common;
using DalApi;
using DO;

internal class DependencyImplementation : IDependency
{
    readonly string s_Dependency_xml = "dependency";

    public int Create(Dependency item)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        int id = Config.NextDependencyId;
        Dependency new_item = item with { Id = id };
        Dependencies.Add(new_item);
        XMLTools.SaveListToXMLSerializer(Dependencies, s_Dependency_xml);
        return id;
    }

    public void Delete(int id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        int index = Dependencies.FindIndex((en) => en.Id == id);
        if (index != -1)
           Dependencies.RemoveAt(index);
        else
            throw new DalDoesNotExistException($"there isn't an Dependency with Id={id}.\n");
        XMLTools.SaveListToXMLSerializer(Dependencies, s_Dependency_xml);
    }

    public bool DoesExist(int dependent_id, int dependsOn_id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        return Dependencies.Any(dep => dep.DependentTask == dependent_id && dep.DependsOnTask == dep.DependsOnTask);

    }

    public Dependency? Read(int id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        return Dependencies.FirstOrDefault(dep => dep.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        if (filter == null) return null;
        return Dependencies.FirstOrDefault(dep => filter(dep));

    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
        if (filter == null)
            return Dependencies.Select(item => item);
        else
            return Dependencies.Where(filter);
    }

    public void Update(Dependency item)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_Dependency_xml);
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
        XMLTools.SaveListToXMLSerializer(Dependencies, s_Dependency_xml);
    }
}
