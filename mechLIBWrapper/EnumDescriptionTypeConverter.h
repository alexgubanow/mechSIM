using namespace System::ComponentModel;
using namespace System::Globalization;
using namespace System::Reflection;

public ref class EnumDescriptionTypeConverter : EnumConverter
{
public:
	EnumDescriptionTypeConverter(System::Type^ _type) :EnumConverter(_type)
	{
	}

	bool CanConvertFrom(ITypeDescriptorContext^ context, System::Type^ sourceType) override
	{
		return sourceType == System::String::typeid || TypeDescriptor::GetConverter(System::Enum::typeid)->CanConvertFrom(context, sourceType);
	}

	Object^ ConvertFrom(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value) override
	{
		if (value->GetType() == System::String::typeid)
			return GetEnumValue(EnumType, (System::String^)value);
		if (value->GetType() == System::Enum::typeid)
			return GetEnumDescription((System::Enum^)value);
		return EnumConverter::ConvertFrom(context, culture, value);
	}

	Object^ ConvertTo(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value, System::Type^ destinationType) override
	{
		return value->GetType()->IsEnum && destinationType == System::String::typeid
			? GetEnumDescription((System::Enum^)value)
			: (value->GetType() == System::String::typeid && destinationType == System::String::typeid
				? GetEnumDescription(EnumType, (System::String^)value)
				: EnumConverter::ConvertTo(context, culture, value, destinationType));
	}

	static System::String^ GetEnumDescription(System::Enum^ value)
	{
		return GetEnumDescription(value->GetType(), value->ToString());
	}

	static System::String^ GetEnumDescription(System::Type^ value, System::String^ name)
	{
		auto fieldInfo = value->GetField(name);
		auto attributes = (array<DescriptionAttribute^>^)fieldInfo->GetCustomAttributes(DescriptionAttribute::typeid, false);
		return (attributes->Length > 0) ? attributes[0]->Description : name;
	}

	static Object^ GetEnumValue(System::Type^ value, System::String^ description)
	{
		auto fields = value->GetFields();
		for each (auto fieldInfo in fields)
		{
			auto attributes = (array<DescriptionAttribute^>^)fieldInfo->GetCustomAttributes(DescriptionAttribute::typeid, false);
			if (attributes->Length > 0 && attributes[0]->Description == description)
				return fieldInfo->GetValue(fieldInfo->Name);
			if (fieldInfo->Name == description)
				return fieldInfo->GetValue(fieldInfo->Name);
		}
		return description;
	}
};