using System;

namespace Bridge
{
    public sealed class ObjectLiteralAttribute : Attribute
    {
        public ObjectLiteralAttribute() { }
        public ObjectLiteralAttribute(ObjectInitializationMode initializationMode) { }
        public ObjectLiteralAttribute(ObjectCreateMode createMode) { }
        public ObjectLiteralAttribute(ObjectInitializationMode initializationMode, ObjectCreateMode createMode) { }
    }
    public enum ObjectInitializationMode
    {
        Ignore = 0,
        Initializer = 1,
        DefaultValue = 2
    }
    public enum ObjectCreateMode
    {
        Plain = 0,
        Constructor = 1
    }
    public class EnumAttribute : Attribute
    {
        public EnumAttribute(Emit emit) { }
    }
    public enum Emit
    {
        Name = 1,
        Value = 2,
        StringName = 3,
        StringNamePreserveCase = 4,
        StringNameLowerCase = 5,
        StringNameUpperCase = 6,
        NamePreserveCase = 7,
        NameLowerCase = 8,
        NameUpperCase = 9
    }
    public sealed class InlineConstAttribute : Attribute
    {
    }
    public sealed class NonScriptableAttribute : Attribute
    {
    }
}