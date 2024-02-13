using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Code.SyntaxBuilders;

namespace GymLog.Application.Aspects;

public class ToStringAttribute : TypeAspect
{
    [Introduce(Name = "ToString", WhenExists = OverrideStrategy.Override)]
    public string ToFormattedString()
    {
        InterpolatedStringBuilder builder = new();

        builder.AddText(meta.Target.Type.Name);

        foreach (IProperty property in meta.Target.Type.Properties)
        {
            builder.AddText(" - ");
            builder.AddText(property.Name);
            builder.AddText(" = { ");
            builder.AddExpression(property.Value);
            builder.AddText(" = } ");
        }

        return builder.ToValue();
    }
}