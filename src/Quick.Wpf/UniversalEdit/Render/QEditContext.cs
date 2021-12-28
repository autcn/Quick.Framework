using System;

namespace Quick
{
    public abstract class QEditContext
    {
        public Type ModelType { get; set; }
        public Type PropertyType { get; set; }

        public string PropertyName { get; set; }

        public static QEditContext CreateGeneric(Type genericType)
        {
            Type type = typeof(QEditContext<>).MakeGenericType(genericType);
            return (QEditContext)Activator.CreateInstance(type);
        }

        public abstract QEditAttribute GetAttr();
        public abstract void SetAttr(QEditAttribute editAttr);
        public void Import(QEditContext editContext)
        {
            GetAttr().Import(editContext.GetAttr());
            this.PropertyName = editContext.PropertyName;
            this.PropertyType = editContext.PropertyType;
            this.ModelType = editContext.ModelType;
        }
    }

    public class QEditContext<TAttribute> : QEditContext where TAttribute : QEditAttribute, new()
    {
        public QEditContext()
        {
            Attr = new TAttribute();
        }

        public TAttribute Attr { get; set; }

        public override QEditAttribute GetAttr()
        {
            return Attr;
        }

        public override void SetAttr(QEditAttribute editAttr)
        {
            Attr = (TAttribute)editAttr;
        }
    }
}
