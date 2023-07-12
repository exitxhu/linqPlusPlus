using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace linqPlusPlus;

public static class deLinq
{
    public static dynamic DeselectObject<TSource, Tkey>(this TSource source, Expression<Func<TSource, Tkey>> deselect)
    {
        var result = deselect.Body is MemberExpression mem
            ? source.DeselectMemberObject(mem)
            : deselect.Body is NewExpression nex
                ? source.DeselectAnynomousObject(nex)
                : new InvalidDataException("Expression type is not valid in this context");
        return result;
    }
    public static IEnumerable<dynamic> Deselect<TSource, Tkey>(this IEnumerable<TSource> source, Expression<Func<TSource, Tkey>> deselect)
    {
        var result = deselect.Body is MemberExpression mem
            ? source.DeselectMember(mem)
            : deselect.Body is NewExpression nex
                ? source.DeselectAnynomous(nex)
                : null;
        if (result is null)
            new InvalidDataException("Expression type is not valid in this context");
        return result!;
    }
    static dynamic DeselectMemberObject<TSource>(this TSource source, MemberExpression exp)
    {
        var des = exp.Member.Name;
        var obj = new ExpandoObject();
        var dic = source.ToDictionary();
        dic.Remove(des);
        var objKvp = (ICollection<KeyValuePair<string, object>>)obj!;

        foreach (var kvp in dic)
        {
            objKvp.Add(kvp);
        }
        dynamic result = obj;

        return result;
    }
    static dynamic DeselectAnynomousObject<TSource>(this TSource source, NewExpression exp)
    {
        var des = exp.Members.Select(n => n.Name);
        var obj = new ExpandoObject();
        var dic = source.ToDictionary();
        foreach (var d in des)
            dic.Remove(d);
        var objKvp = (ICollection<KeyValuePair<string, object>>)obj!;

        foreach (var kvp in dic)
        {
            objKvp.Add(kvp);
        }
        dynamic result = obj;

        return result;
    }
    static IEnumerable<dynamic> DeselectMember<TSource>(this IEnumerable<TSource> source, MemberExpression exp)
    {
        var des = exp.Member.Name;
        var result = new List<dynamic>();
        foreach (var item in source)
        {

            var dic = item.ToDictionary();
            dic.Remove(des);
            var obj = new ExpandoObject();
            var objKvp = (ICollection<KeyValuePair<string, object>>)obj!;

            foreach (var kvp in dic)
            {
                objKvp.Add(kvp);
            }
            result.Add(obj);
        }

        return result;
    }
    static IEnumerable<dynamic> DeselectAnynomous<TSource>(this IEnumerable<TSource> source, NewExpression exp)
    {
        var des = exp.Members.Select(n => n.Name);
        var result = new List<dynamic>();
        foreach (var item in source)
        {
            var dic = item.ToDictionary();
            foreach (var d in des)
                dic.Remove(d);
            var obj = new ExpandoObject();
            var objKvp = (ICollection<KeyValuePair<string, object>>)obj!;

            foreach (var kvp in dic)
            {
                objKvp.Add(kvp);
            }
            result.Add(obj);
        }
        return result;
    }
}
