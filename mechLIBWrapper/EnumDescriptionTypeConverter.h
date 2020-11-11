using namespace System::ComponentModel;
using namespace System::Globalization;
using namespace System::Reflection;

public ref class EnumDescriptionTypeConverter : EnumConverter
{
public:
	EnumDescriptionTypeConverter(System::Type^ _type) :EnumConverter(_type)
	{
	};
	Object^ ConvertTo(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value, System::Type^ destinationType) override
	{
		if (destinationType->FullName == "System.String")
		{
			if (value != nullptr)
			{
				auto fi = value->GetType()->GetField(value->ToString());
				if (fi != nullptr)
				{
					auto attributes = (array<Object^>^)fi->GetCustomAttributes(DescriptionAttribute::typeid, false);
					if (attributes->Length > 0)
					{
						auto descr = (DescriptionAttribute^)attributes[0];
						return ((!System::String::IsNullOrEmpty(descr->Description))) ? descr->Description : value->ToString();
					}
				}
			}

			return System::String::Empty;
		}

		return EnumConverter::ConvertTo(context, culture, value, destinationType);
	}
	/*bool CanConvertFrom(ITypeDescriptorContext^ context, Type sourceType)
	{
		return sourceType == typeof(String) || TypeDescriptor.GetConverter(typeof(Enum)).CanConvertFrom(context, sourceType);
	}*/

	//object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	//{
	//	if (value is string _str)
	//		return GetEnumValue(EnumType, _str);
	//	if (value is Enum _enum)
	//		return GetEnumDescription(_enum);
	//	return base.ConvertFrom(context, culture, value);
	//}

	//object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	//{
	//	return value is Enum _enum && destinationType == typeof(string)
	//		? GetEnumDescription(_enum)
	//		: (value is string _str && destinationType == typeof(string)
	//			? GetEnumDescription(EnumType, _str)
	//			: base.ConvertTo(context, culture, value, destinationType));
	//}

	//static string GetEnumDescription(Enum value)
	//{
	//	var fieldInfo = value.GetType().GetField(value.ToString());
	//	var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
	//	return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
	//}

	//static string GetEnumDescription(Type value, string name)
	//{
	//	var fieldInfo = value.GetField(name);
	//	var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
	//	return (attributes.Length > 0) ? attributes[0].Description : name;
	//}

	//static object GetEnumValue(Type value, string description)
	//{
	//	var fields = value.GetFields();
	//	foreach(var fieldInfo in fields)
	//	{
	//		var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
	//		if (attributes.Length > 0 && attributes[0].Description == description)
	//			return fieldInfo.GetValue(fieldInfo.Name);
	//		if (fieldInfo.Name == description)
	//			return fieldInfo.GetValue(fieldInfo.Name);
	//	}
	//	return description;
	//}
};