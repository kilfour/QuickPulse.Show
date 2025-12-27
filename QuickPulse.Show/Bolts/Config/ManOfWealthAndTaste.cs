using System.Linq.Expressions;
using System.Reflection;
using QuickPulse.Arteries;
using QuickPulse.Instruments;
using QuickPulse.Show.Reflects;

namespace QuickPulse.Show.Bolts;

public class ManOfWealthAndTaste
{
    private readonly Puzzles puzzles = new();

    public ManOfWealthAndTaste ToPrettyPrint()
        => Chain.It(() => puzzles.PrettyPrint = true, this);

    public ManOfWealthAndTaste ToAddSomeClass()
        => Chain.It(() => puzzles.WithClass = true, this);

    public ManOfWealthAndTaste ToSelfReference<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.SelfReferencingFormatter(formatter), this);

    public ManOfWealthAndTaste ToInline<T>()
        => Chain.It(() => puzzles.InlineType(typeof(T)), this);

    public ManOfWealthAndTaste ToUseNonLinearTime(bool noSeconds = false)
        => Chain.It(() => puzzles.Registry.UsingWibblyWobbly(noSeconds), this);

    public ManOfWealthAndTaste ToReplace<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.Registry.Register(formatter), this);

    public ManOfWealthAndTaste ToReplaceAll(Func<Type, bool> predicate, Func<object, string> formatter)
        => Chain.It(() => puzzles.RegisterFormatter(predicate, formatter), this);

    public ManOfWealthAndTaste ToSubstituteWithPropertyNamed<T>(string propertyName)
        => Chain.It(() => puzzles.RegisterFormatter(a => a.HasPropertyNamed<T>(propertyName),
            a => GetValueFor(a, propertyName)), this);

    private static object GetValueFor(object target, string propertyName)
        => target.GetType()
            .GetProperty(propertyName)!
            .GetValue(target)!;

    public ManOfWealthAndTaste ToRegisterSystemType<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.RegisterSystemTypeFormatter(formatter), this);

    public ManOfWealthAndTaste To<T>(Action<Troubadour<T>> customize)
        => Chain.It(() => customize(new Troubadour<T>(this, puzzles)), this);

    public ManOfWealthAndTaste ToPostProcess<T>(Func<string, string> postProcesser)
        => Chain.It(() => puzzles.RegisterPostProcesser<T>(postProcesser), this);

    public ManOfWealthAndTaste ToPrefix<T>(string prefix)
        => Chain.It(() => puzzles.RegisterPrefix<T>(prefix), this);

    public ManOfWealthAndTaste ToIgnore<T, TProp>(Expression<Func<T, TProp>> expr)
    {
        var member = AsMemberInfo(expr);
        if (member is FieldInfo field) { puzzles.RegisterFieldToIgnore<T>(field); }
        if (member is PropertyInfo prop) { puzzles.RegisterPropertyToIgnore<T>(prop); }
        return this;
    }

    private static MemberInfo AsMemberInfo<TTarget, TMember>(Expression<Func<TTarget, TMember>> expression)
    {
        if (expression.Body is MemberExpression memberExpr)
        {
            return memberExpr.Member;
        }

        if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            return unaryMember.Member;
        }

        throw new ArgumentException($"Expression '{expression}' does not refer to a field or property.");
    }
    //.ToPrefix<List<Product>>(Environment.NewLine)

    //.ToIgnore<Product>(a => a.Id)
    public string IntroduceThis<T>(T obj)
        => Signal.From(The.Tsar(
            new Ministers()
            {
                FieldsToIgnore = puzzles.FieldsToIgnore,
                PropertiesToIgnore = puzzles.PropertiesToIgnore,
                Registry = puzzles.Registry,
                TypeRegistry = puzzles.TypeRegistry,
                SystemTypeRegistry = puzzles.SystemTypeRegistry,
                WithClass = puzzles.WithClass,
                SelfReferencingRegistry = puzzles.SelfReferencingRegistry,
                InlinedTypes = puzzles.InlinedTypes,
                Formatters = puzzles.Formatters,
                PostProcessRegistry = puzzles.PostProcessRegistry,
                PrefixRegistry = puzzles.PrefixRegistry

            }, puzzles.PrettyPrint))
            .SetArtery(Text.Capture())
            .Pulse(obj!)
            .GetArtery<StringSink>()
            .Content();

}
