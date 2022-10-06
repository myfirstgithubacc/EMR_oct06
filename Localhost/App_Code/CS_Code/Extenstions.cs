
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

/// <summary>
/// Extiension metohod to find any constrol on given form.
/// </summary>
public static class Extenstions
{
    /// <summary>
    /// To find contorls by its given type.
    /// <para>By Vikash Kumar</para>
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IEnumerable<Control> FindAll(this ControlCollection collection)
    {
        foreach (Control item in collection)
        {
            yield return item;

            if (item.HasControls())
            {
                foreach (var subItem in item.Controls.FindAll())
                {
                    yield return subItem;
                }
            }
        }
    }

    /// <summary>
     /// Generic method to find contorls by its given type.
     /// <para>By Vikash Kumar</para>
     /// </summary>
     /// <param name="collection"></param>
     /// <returns></returns>
    public static IEnumerable<T> FindAll<T>(this ControlCollection collection) where T : Control
    {
        return collection.FindAll().OfType<T>();
    }
}