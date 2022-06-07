# Linq plus plus

lightweight extensions to make your every day life easier


`Dictionary<string, object> ToDictionary(this object source)`

this method converts any objects to a dictionary, where the property name is key. use with care bcz of boxing/unboxing.

`bool HasContent(this string str)`

check if string is null or empty or whith space.

`bool IfNull(this object obj, Func<bool> nullPart, Func<bool> notNullPart)`

check if an object is null and then apply the condition, much like IF(_,+,*) in vb.

use it in expressions because null propagation is nut supported there

`dynamic DeselectObject<TSource, Tkey>(this TSource source, Expression<Func<TSource, Tkey>> deselect)`

introduce some field to not includ in final result. for example you can: userObj.deselect((user n)=> n.password) to exclude only password from result.

this method return a dynamic object, much usefull in KISS apis.

`IEnumerable<dynamic> Deselect<TSource, Tkey>(this IEnumerable<TSource> source, Expression<Func<TSource, Tkey>> deselect)`

much like the other one above, but on collections.



### Disclaimer

the term linq plus plus has chosen to resemble how this package is easy to use and fimiliar.

there is no relation between this package and official linq library ;)

